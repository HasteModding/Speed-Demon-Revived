// MIT License

// Copyright (c) 2025 Stevelion

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using Landfall.Haste;
using System.Reflection;
using System.Collections;

namespace CustomItemLib
{
    public class ItemFactory
    {
        static ItemFactory()
        {
            On.ItemDatabase.Awake += (orig, self) =>
            {
                orig(self);
                LoadCustomItems();
            };

            On.SaveSystem.Load += (orig) =>
            {
                HasteSave save = orig();
                UnlockCustomItems();
                return save;
            };

            On.Player.RegisterItem += (orig, self, item) =>
            {
                GameObject cloneGameObject = item.gameObject;
                ItemInstance templateItemInstance = ItemDatabase.instance.items.FirstOrDefault(e => e.name == item.itemName);
                CloneRuntimeListeners(templateItemInstance, cloneGameObject);
                orig(self, item);
            };
        }

        public static event Action ItemsLoaded = null!;

        private static void LoadCustomItems()
        {
            Debug.Log($"[CustomItemLib] Loading custom items");
            if (ItemsLoaded is not null)
            {
                try
                {
                    ItemsLoaded();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        private static void UnlockCustomItems()
        {
            foreach (string itemName in namesToUnlock)
            {
                if (FactSystem.GetFact(new Fact($"{itemName}_ShowItem")) == 0.0f || FactSystem.GetFact(new Fact($"item_unlocked_{itemName}")) == 0.0f)
                {
                    Debug.Log($"[CustomItemLib] Unlocking item {itemName}");
                    FactSystem.SetFact(new Fact($"{itemName}_ShowItem"), 1.0f);
                    FactSystem.SetFact(new Fact($"item_unlocked_{itemName}"), 1.0f);
                }
            }
        }

        private static List<string> namesToUnlock = new List<string>();

        private static void CloneRuntimeListeners(ItemInstance template, GameObject clone)
        {
            ItemInstance cloneItemInstance = clone.GetComponent<ItemInstance>();
            // Clone effectEvent
            CloneUnityEventRuntimeListeners(template.effectEvent, cloneItemInstance.effectEvent, clone);
            // Clone keyDownEvent
            CloneUnityEventRuntimeListeners(template.keyDownEvent, cloneItemInstance.keyDownEvent, clone);
            // Clone keyUpEvent
            CloneUnityEventRuntimeListeners(template.keyUpEvent, cloneItemInstance.keyUpEvent, clone);
        }

        private static void CloneUnityEventRuntimeListeners(UnityEvent templateEvent, UnityEvent cloneEvent, GameObject cloneObject)
        {
            // InvokableCallList that holds the runtime calls
            var callListObj = typeof(UnityEventBase).GetField("m_Calls", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(templateEvent);
            // List<BaseInvokableCall>
            IList? runtimeCallList = callListObj.GetType().GetField("m_RuntimeCalls", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(callListObj) as IList;
            if (runtimeCallList == null || runtimeCallList.Count == 0) return;
            // Clone each UnityAction
            UnityAction templateAction;
            Component cloneEffectComponent;
            foreach (var call in runtimeCallList)
            {
                templateAction = (UnityAction)call.GetType().GetField("Delegate", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(call);
                // Add the MonoBehavior script as a component to the new item
                cloneEffectComponent = cloneObject.GetComponent(templateAction.Target.GetType());
                // Create a new UnityAction and add it as a listener to the new event
                UnityAction unityAction = (UnityAction)Delegate.CreateDelegate(typeof(UnityAction), cloneEffectComponent, templateAction.Method);
                cloneEvent.AddListener(unityAction);
            }
            return;
        }

        public class ActionDescriptor
        {
            public Type ClassType { get; }
            public string MethodName { get; }

            public ActionDescriptor(Type classType, string methodName)
            {
                ClassType = classType;
                MethodName = methodName;
            }
        }

        private static UnityEvent CreateUnityEvent(GameObject itemObject, List<ActionDescriptor> descriptors)
        {
            var unityEvent = new UnityEvent();
            foreach (var descriptor in descriptors)
            {
                Component effectComponent = itemObject.AddComponent(descriptor.ClassType);
                MethodInfo configureMethod = descriptor.ClassType.GetMethod("Configure", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (configureMethod is not null) configureMethod.Invoke(effectComponent, null); // Provides a way to configure effects
                MethodInfo method = descriptor.ClassType.GetMethod(descriptor.MethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (method == null)
                {
                    Debug.LogError($"[CustomItemLib] Error: Method {descriptor.MethodName} not found on type {descriptor.ClassType.Name}");
                }
                UnityAction unityAction = (UnityAction)Delegate.CreateDelegate(typeof(UnityAction), effectComponent, method);
                unityEvent.AddListener(unityAction);
            }
            return unityEvent;
        }

        public static void CleanScriptsFromItem(string itemName, Type componentType)
        {
            ItemInstance itemInstance = ItemDatabase.instance.items.FirstOrDefault(e => e.name == itemName);
            if (itemInstance == null)
            {
                Debug.LogWarning($"[CustomItemLib] No item named {itemName} found.");
                return;
            }
            Component[] components = itemInstance.GetComponents(componentType);
            if (components.Length > 0) foreach (var comp in components) UnityEngine.Object.Destroy(comp);
            else Debug.Log($"[CustomItemLib] {itemInstance.itemName} does not have component {componentType.Name}");
        }

        public static PlayerStats CreatePlayerStats(
            PlayerStat? maxHealth = null,
            PlayerStat? runSpeed = null,
            PlayerStat? airSpeed = null,
            PlayerStat? turnSpeed = null,
            PlayerStat? drag = null,
            PlayerStat? gravity = null,
            PlayerStat? fastFallSpeed = null,
            PlayerStat? fastFallLerp = null,
            PlayerStat? lives = null,
            PlayerStat? dashes = null,
            PlayerStat? boost = null,
            PlayerStat? luck = null,
            PlayerStat? startWithEnergyPercentage = null,
            PlayerStat? maxEnergy = null,
            PlayerStat? itemPriceMultiplier = null,
            PlayerStat? itemRarity = null,
            PlayerStat? sparkMultiplier = null,
            PlayerStat? startingResource = null,
            PlayerStat? energyGain = null,
            PlayerStat? damageMultiplier = null,
            PlayerStat? sparkPickupRange = null,
            PlayerStat? extraLevelSparks = null,
            PlayerStat? extraLevelDifficulty = null
        )
        {
            PlayerStats playerStats = new PlayerStats
            {   // For each field, either use the provided PlayerStat or create a new one with { baseValue = 0f, multiplier = 1f }
                maxHealth = maxHealth ?? new(),
                runSpeed = runSpeed ?? new(),
                airSpeed = airSpeed ?? new(),
                turnSpeed = turnSpeed ?? new(),
                drag = drag ?? new(),
                gravity = gravity ?? new(),
                fastFallSpeed = fastFallSpeed ?? new(),
                fastFallLerp = fastFallLerp ?? new(),
                lives = lives ?? new(),
                dashes = dashes ?? new(),
                boost = boost ?? new(),
                luck = luck ?? new(),
                startWithEnergyPercentage = startWithEnergyPercentage ?? new(),
                maxEnergy = maxEnergy ?? new(),
                itemPriceMultiplier = itemPriceMultiplier ?? new(),
                itemRarity = itemRarity ?? new(),
                sparkMultiplier = sparkMultiplier ?? new(),
                startingResource = startingResource ?? new(),
                energyGain = energyGain ?? new(),
                damageMultiplier = damageMultiplier ?? new(),
                sparkPickupRange = sparkPickupRange ?? new(),
                extraLevelSparks = extraLevelSparks ?? new(),
                extraLevelDifficulty = extraLevelDifficulty ?? new()
            };
            return playerStats;
        }

        private static ItemInstance? GetItemInstanceByItemName(string itemName) // Can return null if item does not exist
        {
            foreach (ItemInstance itemObject in ItemDatabase.instance.items)
            {
                ItemInstance itemInstanceComponent;
                if (string.Equals(itemName, itemObject.name, StringComparison.InvariantCultureIgnoreCase)
                    && itemObject.TryGetComponent<ItemInstance>(out itemInstanceComponent))
                {
                    return itemInstanceComponent;
                }
            }
            return null;
        }

        private static ItemInstance CreateNewItemInstance(string itemName)
        {
            GameObject itemObject = new(itemName);
            try
            {
                itemObject.SetActive(false);
                ItemInstance itemInstanceComponent = itemObject.AddComponent<ItemInstance>();
                ItemDatabase.instance.items.Add(itemInstanceComponent);
                UnityEngine.Object.DontDestroyOnLoad(itemObject);
                return itemInstanceComponent;
            }
            catch (Exception)
            {
                UnityEngine.Object.Destroy(itemObject);
                throw;
            }
        }

        public static void AddItemToDatabase(
            string itemName,
            bool autoUnlocked = true,
            bool? minorItem = null,
            Rarity? rarity = null,
            List<ItemTag>? itemTags = null,
            LocalizedString? title = null,
            bool? usesTriggerDescription = null,
            LocalizedString? triggerDescription = null,
            bool? usesEffectDescription = null,
            LocalizedString? description = null,
            LocalizedString? flavorText = null,
            float? iconScaleModifier = null,
            bool? statsAfterTrigger = null,
            float? cooldown = null,
            ItemTriggerType? triggerType = null,
            List<ItemTrigger>? triggerConditions = null,
            List<ItemEffect>? effects = null,
            PlayerStats? stats = null,
            List<ActionDescriptor>? effectEvent = null,
            List<ActionDescriptor>? keyDownEvent = null,
            List<ActionDescriptor>? keyUpEvent = null
        )
        {
            ItemInstance? itemInstance = GetItemInstanceByItemName(itemName);
            if (itemInstance is not null)
            {
                Debug.Log($"[CustomItemLib] Replacing item {itemName}");
                if (minorItem is not null) itemInstance.minorItem = (bool)minorItem;
                if (rarity is not null) itemInstance.rarity = (Rarity)rarity;
                if (itemTags is not null) itemInstance.itemTags = itemTags;
                if (title is not null) itemInstance.title = title;
                if (usesTriggerDescription is not null) itemInstance.usesTriggerDescription = (bool)usesTriggerDescription;
                if (triggerDescription is not null) itemInstance.triggerDescription = triggerDescription;
                if (usesEffectDescription is not null) itemInstance.usesEffectDescription = (bool)usesEffectDescription;
                if (description is not null) itemInstance.description = description;
                if (flavorText is not null) itemInstance.flavorText = flavorText;
                if (iconScaleModifier is not null) itemInstance.iconScaleModifier = (float)iconScaleModifier;
                if (statsAfterTrigger is not null) itemInstance.statsAfterTrigger = (bool)statsAfterTrigger;
                if (cooldown is not null) itemInstance.cooldown = (float)cooldown;
                if (triggerType is not null) itemInstance.triggerType = (ItemTriggerType)triggerType;
                if (triggerConditions is not null) itemInstance.triggerConditions = triggerConditions;
                if (effects is not null) itemInstance.effects = effects;
                if (stats is not null) itemInstance.stats = stats;
                if (effectEvent is not null) itemInstance.effectEvent = CreateUnityEvent(itemInstance.gameObject, effectEvent);
                if (keyDownEvent is not null) itemInstance.keyDownEvent = CreateUnityEvent(itemInstance.gameObject, keyDownEvent);
                if (keyUpEvent is not null) itemInstance.keyUpEvent = CreateUnityEvent(itemInstance.gameObject, keyUpEvent);
            }
            else
            {
                Debug.Log($"[CustomItemLib] Creating new item {itemName}");
                itemInstance = CreateNewItemInstance(itemName);
                itemInstance.itemName = itemName; // Yes we have to put the name in 2 different places
                itemInstance.minorItem = minorItem ?? false;
                itemInstance.rarity = rarity ?? Rarity.Common;
                itemInstance.itemTags = itemTags ?? new List<ItemTag>();
                itemInstance.title = title ?? new UnlocalizedString("Missing Title");
                itemInstance.usesTriggerDescription = usesTriggerDescription ?? false;
                itemInstance.triggerDescription = triggerDescription ?? new UnlocalizedString("Item is missing trigger description");
                itemInstance.usesEffectDescription = usesEffectDescription ?? false;
                itemInstance.description = description ?? new UnlocalizedString("Item is missing description");
                itemInstance.flavorText = flavorText ?? new UnlocalizedString("Dalil didn't explain what this item is...");
                itemInstance.iconScaleModifier = iconScaleModifier ?? 1f;
                itemInstance.statsAfterTrigger = statsAfterTrigger ?? false;
                itemInstance.cooldown = cooldown ?? 0f;
                itemInstance.triggerType = triggerType ?? ItemTriggerType.None;
                itemInstance.triggerConditions = triggerConditions ?? new List<ItemTrigger> { };
                itemInstance.effects = effects ?? new List<ItemEffect> { };
                itemInstance.stats = stats ?? CreatePlayerStats();
                itemInstance.effectEvent = effectEvent != null ? CreateUnityEvent(itemInstance.gameObject, effectEvent) : new UnityEvent();
                itemInstance.keyDownEvent = keyDownEvent != null ? CreateUnityEvent(itemInstance.gameObject, keyDownEvent) : new UnityEvent();
                itemInstance.keyUpEvent = keyUpEvent != null ? CreateUnityEvent(itemInstance.gameObject, keyUpEvent) : new UnityEvent();
            }
            if (autoUnlocked)
            {
                namesToUnlock.Add(itemName);
            }
        }
    }
}