using System.Reflection;
using Landfall.Modding;
using Landfall.Haste;
using UnityEngine;
using UnityEngine.SceneManagement;
using SpeedDemon.CustomItems.CustomItemDefinitions;

namespace SpeedDemon.RewardOverride
{
    internal class StartingItems
    {
        public static bool isChoosingStartingItems = false;
        public static StartingItemSetItems.StartingItemSet chosenSet = null!;

        private static Fact _startingItemIdxFact = new Fact("SpeedDemon_StartingItemIdx");
        public static int StartingItemsIdx
        {
            get
            {
                int value = (int)FactSystem.GetFact(_startingItemIdxFact);
                int clampedValue = Math.Clamp(value, 0, startingItemSets.Count - 1);
                return clampedValue;
            }
            set
            {
                int clampedValue = Math.Clamp(value, 0, startingItemSets.Count - 1);
                FactSystem.SetFact(_startingItemIdxFact, clampedValue);
            }
        }

        public static List<StartingItemSetItems.StartingItemSet> startingItemSets = new();

        public static void StartingItemsAward(Player player)
        {
            if (startingItemSets.Count == 0)
            {
                Debug.LogError("[SpeedDemon] No starting item sets loaded");
                return;
            }
            // Add scroll right if needed
            if (StartingItemsIdx > 0)
            {
                UnlockScreen.me.AddItem(ItemDatabase.instance.items.FirstOrDefault(e => e.itemName == "SD_UI_StartingItemsLeft"));
            }
            // Add display item
            UnlockScreen.me.AddItem(startingItemSets[StartingItemsIdx].displayItem);
            // Add scroll right if needed
            if (StartingItemsIdx < startingItemSets.Count - 1)
            {
                UnlockScreen.me.AddItem(ItemDatabase.instance.items.FirstOrDefault(e => e.itemName == "SD_UI_StartingItemsRight"));
            }
            // I have no idea what this does :)
            UnlockScreen.me.chooseItem = true;
            UnlockScreen.me.FinishAddingPhase(player, new Action(PlayLevelIfDone));
        }

        public static void RotateSelection(ItemInstance item)
        {
            if (item.itemName == "SD_UI_StartingItemsLeft") StartingItemsIdx--;
            else StartingItemsIdx++;
        }

        public static void AddSelectedStartingItems(ItemInstance item)
        {
            MethodInfo addItemMethod = typeof(Player).GetMethod("AddItem", BindingFlags.Instance | BindingFlags.NonPublic);
            //StartingItemSetItems.StartingItemSet set = startingItemSets.FirstOrDefault(e => e.displayItem.itemName == "SD_UI_StartingItemsRight");
            chosenSet = startingItemSets.FirstOrDefault(e => e.displayItem.itemName == item.itemName);
            //foreach(string itemName in set.itemNames)
            //{
            //    ItemInstance itemToAdd = ItemDatabase.instance.items.FirstOrDefault(e => e.itemName == "SD_UI_StartingItemsRight");
            //    addItemMethod.Invoke(Player.localPlayer, new object[] { itemToAdd, 0 });
            //}
            Debug.Log($"[SpeedDemon] Selected class {chosenSet.displayItem.itemName}");
            void addItemsToPlayer()
            {
                Debug.Log($"[SpeedDemon] Giving items for class {chosenSet.displayItem.itemName}");
                foreach (string itemName in chosenSet.itemNames)
                {
                    ItemInstance itemToAdd = ItemDatabase.instance.items.FirstOrDefault(e => e.itemName == itemName);
                    if (itemToAdd != null) addItemMethod.Invoke(Player.localPlayer, new object[] { itemToAdd });
                }
                GM_API.NewLevel -= addItemsToPlayer;
            }
            GM_API.NewLevel += addItemsToPlayer;
            isChoosingStartingItems = false;
        }

        public static void PlayLevelIfDone()
        {
            //Debug.Log($"[SpeedDemon] isChoosingStartingItems is {isChoosingStartingItems}");
            if (!isChoosingStartingItems)
            {
                RewardRouter.rewardScreenType = RewardRouter.RewardScreenType.None;
                RunHandler.PlayLevel();
            }
            else
            {
                SceneManager.LoadScene("EndlessAwardScene");
            }
        }
    }
}
