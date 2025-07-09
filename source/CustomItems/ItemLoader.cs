using Landfall.Modding;
using CustomItemLib;
using UnityEngine;
using System.Reflection;

namespace SpeedDemon.CustomItems
{
    [LandfallPlugin]
    public class ItemLoader
    {
        static ItemLoader()
        {
            Debug.Log("[SpeedDemon] Queuing custom items...");
            ItemFactory.ItemsLoaded += CopyDefaultItems;
            ItemFactory.ItemsLoaded += CustomItemDefinitions.DefaultOverrides.LoadDefaultOverrides;
            ItemFactory.ItemsLoaded += CustomItemDefinitions.CustomRewardables.LoadCustomItems;
            ItemFactory.ItemsLoaded += CustomItemDefinitions.UtilityItems.LoadUtilityItems;
            ItemFactory.ItemsLoaded += CustomItemDefinitions.StartingItemSetItems.LoadStartingItemSetItems;
            ItemFactory.ItemsLoaded += CustomItemDefinitions.StartingItemSetItems.LoadStartingItemSets;
            ItemFactory.ItemsLoaded += DestroyUnusedItems;
        }

        private static void CopyDefaultItems()
        {
            // Get CIL's list of names to unlock
            List<string> namesToUnlock = (List<string>)typeof(ItemFactory).GetField("namesToUnlock", BindingFlags.Static | BindingFlags.NonPublic)
                .GetValue(null); // This list is currently private in the library, should probably change this in the future
            // Create a container so the overrides aren't just thrown into the scene unparented
            GameObject defaultOverrideRoot = new("DefaultItemOverrides");
            UnityEngine.Object.DontDestroyOnLoad(defaultOverrideRoot);
            // Copy the items
            List <ItemInstance> defaultItems = new(ItemDatabase.instance.items);
            foreach (ItemInstance defaultItem in defaultItems)
            {
                if (defaultItem.minorItem) continue;
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(defaultItem.gameObject, defaultOverrideRoot.transform);
                gameObject.SetActive(false);
                ItemInstance itemInstance = gameObject.GetComponent<ItemInstance>();
                ItemDatabase.instance.items.Add(itemInstance);
                string newItemName = $"SD_{itemInstance.itemName}";
                gameObject.name = newItemName;
                itemInstance.itemName = newItemName;
                CopyDefaultMeshes(newItemName, defaultItem.itemName);
                namesToUnlock.Add(newItemName);
            }
        }

        internal static void CopyDefaultMeshes(string newItemName, string oldItemName, bool minorItem = false)
        {
            GameObject itemVisuals = (minorItem ? ItemDatabase.instance.minorItemsVisuals : ItemDatabase.instance.majorItemsVisuals);
            GameObject? oldMesh = null;
            GameObject? oldMesh001 = null;
            for (int i = 0; i < itemVisuals.transform.childCount; i++)
            {
                GameObject child = itemVisuals.transform.GetChild(i).gameObject;
                if (child.name == oldItemName) oldMesh = child;
                if (child.name == $"{oldItemName}.001") oldMesh001 = child;
            }
            if (oldMesh == null || oldMesh001 == null)
            {
                Debug.LogWarning($"[SpeedDemon] Could not find mesh for default item {oldItemName}, using fallback");
                return;
            }
            GameObject newMesh = UnityEngine.Object.Instantiate<GameObject>(oldMesh);
            newMesh.transform.SetParent(itemVisuals.transform);
            newMesh.name = newItemName;
            GameObject newMesh001 = UnityEngine.Object.Instantiate<GameObject>(oldMesh001);
            newMesh001.transform.SetParent(itemVisuals.transform);
            newMesh001.name = $"{newItemName}.001";
        }

        private static void DestroyUnusedItems()
        {
            List<ItemInstance> itemsToKeep = new();
            itemsToKeep.AddRange(SpeedDemonItems.RewardableItems);
            itemsToKeep.AddRange(SpeedDemonItems.UtilityItems);
            itemsToKeep.AddRange(SpeedDemonItems.StartingItemSetItems);
            itemsToKeep.AddRange(SpeedDemonItems.QuestItems);
            List<ItemInstance> toRemove = new();
            foreach (ItemInstance templateItem in ItemDatabase.instance.items)
            {
                if (!templateItem.minorItem && !itemsToKeep.Contains(templateItem)) toRemove.Add(templateItem);
            }
            foreach (ItemInstance templateItem in toRemove)
            {
                Debug.Log($"[SpeedDemon] Removing unused item {templateItem.name} from ItemDatabase");
                ItemDatabase.instance.items.Remove(templateItem);
                UnityEngine.Object.Destroy(templateItem.gameObject);
            }
        }
    }
}
