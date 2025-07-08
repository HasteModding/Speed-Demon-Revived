using System.Reflection;
using Landfall.Modding;
using Landfall.Haste;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpeedDemon.RewardOverride
{
    internal class SpeedDemonReward
    {
        static public System.Random currentLevelRandomInstance = null!;
        static public bool currentRewardIsEpic = false;
        static public bool isRefreshing = false;

        public static class RewardRefresh
        {
            private static ItemInstance templateRefreshItem = null!;
            public static ItemInstance TemplateRefreshItem
            {
                get
                {
                    if (templateRefreshItem == null) templateRefreshItem = ItemDatabase.instance.items.FirstOrDefault(e => e.name == "SD_UI_Refresh");
                    return templateRefreshItem;
                }
            }
            public static int GetRefreshCost()
            {
                if (freeRefreshes > 1)
                {
                    TemplateRefreshItem.description = new UnlocalizedString(
                        $"<color=#c896fa>Refresh</color> reward, Lose <color=#e2b96b>0</color> <color=#c896fa>Sparks</color>\n({freeRefreshes} free <color=#c896fa>Refreshes</color> left)");
                    TemplateRefreshItem.description.RefreshString();
                    return 0;
                }
                if (freeRefreshes == 1)
                {
                    TemplateRefreshItem.description = new UnlocalizedString(
                        $"<color=#c896fa>Refresh</color> reward, Lose <color=#e2b96b>0</color> <color=#c896fa>Sparks</color>\n({freeRefreshes} free <color=#c896fa>Refresh</color> left)");
                    TemplateRefreshItem.description.RefreshString();
                    return 0;
                }
                float luckModifier = luckRefreshCRDampenAmount / (luckRefreshCRDampenAmount + lastLuck - 1);
                Debug.Log($"[SpeedDemon] lastLuck: {lastLuck}, luckModifier: {luckModifier}, luckRefreshCRCount: {luckRefreshCRCount}, lastRefreshCostUpdate: {lastRefreshCostUpdate}");
                lastRefreshCostUpdate = Math.Max((int)(baseRefreshCost * Mathf.Pow(luckModifier, luckRefreshCRCount)), minRefreshCost);
                TemplateRefreshItem.description = new UnlocalizedString(
                    $"<color=#c896fa>Refresh</color> reward, Lose <color=#e2b96b>{lastRefreshCostUpdate}</color> <color=#c896fa>Sparks</color>");
                TemplateRefreshItem.description.RefreshString();
                return lastRefreshCostUpdate;
            }

            public static void UpdateRefreshCR() // Lol (see SD_ReduceRefreshCostEffect)
            {
                lastLuck = (float)typeof(PlayerStat).GetMethod("GetValue", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(Player.localPlayer.stats.luck, null);
                int tempLuckRefreshCRCount = 0;
                foreach (ItemInstance item in Player.localPlayer.items)
                {
                    if (item.gameObject.GetComponent<CustomItems.SD_ReduceRefreshCostEffect>() != null)
                    {
                        tempLuckRefreshCRCount++;
                    }
                }
                luckRefreshCRCount = tempLuckRefreshCRCount;
            }

            private static int baseRefreshCost = 250;
            private static int minRefreshCost = 50;
            public static int luckRefreshCRCount = 0;
            private static float luckRefreshCRDampenAmount = 4f;
            public static int lastRefreshCostUpdate = 250;
            public static float lastLuck = 1f;
            public static int freeRefreshes = 0;

        }

        internal static void OnRefreshItemChosen(ItemInstance item)
        {
            Debug.Log("[SpeedDemon] Refreshing items");
            isRefreshing = true;
            PersistentPlayerData.SetSparks((int)RunHandler.RunData.playerData.resource - RewardRefresh.GetRefreshCost());
            if (RewardRefresh.freeRefreshes > 0) RewardRefresh.freeRefreshes--;
        }

        internal static void OnRefreshItemNotChosen(ItemInstance item)
        {
            Debug.Log($"[SpeedDemon] Player chose {item.name}");
            currentLevelRandomInstance = null!; // reset the vars we were storing for this reward
            currentRewardIsEpic = false;
            isRefreshing = false;
        }

        public static void SpeedDemonAward(Player player)
        {
            if (currentLevelRandomInstance == null)
            { // Instantiate a rng if we don't have one
                currentLevelRandomInstance = RunHandler.GetCurrentLevelRandomInstance();
            }
            if (currentRewardIsEpic)
            {
                Debug.Log("[SpeedDemon] Generating high quality reward");
                for (int i = 0; i < 3; i++)
                {
                    UnlockScreen.me.AddItem(GetRandomItem(currentLevelRandomInstance, true));
                }
            }
            else
            {
                Debug.Log("[SpeedDemon] Generating low quality reward");
                for (int i = 0; i < 3; i++)
                {
                    UnlockScreen.me.AddItem(GetRandomItem(currentLevelRandomInstance, false));
                }
            }
            float currentRefreshCost = RewardRefresh.GetRefreshCost();
            Debug.Log($"[SpeedDemon] Player has {RunHandler.RunData.playerData.resource} Sparks and a refresh costs {currentRefreshCost}");
            if (RunHandler.RunData.playerData.resource >= currentRefreshCost)
            {
                Debug.Log("[SpeedDemon] Adding refresh item because player can afford it");
                UnlockScreen.me.AddItem(RewardRefresh.TemplateRefreshItem);
            }

            // I have no idea what this does :)
            UnlockScreen.me.chooseItem = true;
            UnlockScreen.me.FinishAddingPhase(player, PlayLevelIfDone);
        }

        public static ItemInstance GetRandomItem(System.Random random, bool epicReward)
        {
            Rarity chosenRarity;
            int randInt = random.Next(100);
            if (epicReward)
            {
                if (randInt < 15) chosenRarity = Rarity.Legendary;
                else chosenRarity = Rarity.Epic;
            }
            else
            {
                if (randInt < 1) chosenRarity = Rarity.Legendary;
                else if (randInt < 6) chosenRarity = Rarity.Epic;
                else if (randInt < 31) chosenRarity = Rarity.Rare;
                else chosenRarity = Rarity.Common;
            }
            Debug.Log($"[SpeedDemon] Chose {chosenRarity}");
            MethodInfo selectItemMethod = typeof(ItemDatabase).GetMethod("SelectItemWithRarity", BindingFlags.Static | BindingFlags.NonPublic);
            ItemInstance chosenItem = (ItemInstance)selectItemMethod.Invoke(null, [CustomItems.SpeedDemonItems.RewardableItems, chosenRarity, random]);
            Debug.Log($"[SpeedDemon] Chose {chosenItem.name}");
            return chosenItem;
        }

        public static void PlayLevelIfDone()
        {
            Debug.Log($"[SpeedDemon] isRefreshing is {isRefreshing}");
            if (!isRefreshing)
            {
                RunHandler.RunData.currentNodeStatus = NGOPlayer.PlayerNodeStatus.PostLevelScreenComplete;
            }
            else
            {
                isRefreshing = false;
                SceneManager.LoadScene("EndlessAwardScene");
            }
        }
    }
}
