using System.Reflection;
using Landfall.Haste;
using Landfall.Modding;
using SpeedDemon.CustomItems;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpeedDemon
{
    [LandfallPlugin]
    public class ItemReward
    {
        static public System.Random currentLevelRandomInstance = null!;
        static public bool currentRewardIsEpic = false;
        static public bool isRefreshing = false;

        static List<ItemInstance> rewardableItems = null!;

        static ItemReward()
        {
            Debug.Log("[SpeedDemon] ItemReward class initializing...");
            On.RunHandler.TransitionOnLevelCompleted += (orig) =>
            {
                Debug.Log("[SpeedDemon] Level Completed, preparing next scene");
                // Pass back to the original method if we aren't in Endless
                if (!RunHandler.RunData.runConfig.isEndless || !RunHandler.InRun)
                {
                    orig();
                    return;
                }

                // Per Level Healing - 25 health per level
                typeof(Player).GetMethod("AddHealth", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(Player.localPlayer, [25f]);

                // Per Level Lives - max lives per level
                Player.localPlayer.EditLives(1);

                // Counters
                // TODO : Fix this! It's supposed to be the counters for the run data, but since Stevelion, the codebase changed.
                /*RunHandler.RunData.currentLevelID++;
                RunHandler.RunData.currentLevel++;*/

                // Temporary Sparks
                Player.localPlayer.data.resource += Player.localPlayer.data.temporaryResource;

                // Reward Flag
                HasteStats.SetStat(HasteStatType.STAT_ENDLESS_HIGHSCORE, RunHandler.RunData.currentLevel, true, false, false);
                HasteStats.OnLevelComplete();

                bool reward = RunHandler.RunData.currentLevel == 1 || RunHandler.RunData.currentLevel % 2 == 0;
                currentRewardIsEpic = RunHandler.RunData.currentLevel == 1 || RunHandler.RunData.currentLevel % 10 == 0;
                // Load the next scene (whichever it is)
                if (reward)
                {
                    UI_TransitionHandler.instance.Transition(delegate
                    {
                        SceneManager.LoadScene("EndlessAwardScene");
                    }, "Dots", 0.3f, 0.5f, null);
                    return;
                }
                // Finish up and move on to the next level
                RunHandler.RunData.currentNodeStatus = NGOPlayer.PlayerNodeStatus.PostLevelScreenComplete;
            };

            On.EndlessAward.Start += (orig, self) => // Overwrite Endless mode reward scene
            {
                SpeedDemonAward(Player.localPlayer);
            };

            On.UnlockScreen.GivePlayerItem += (orig, self, player, item) => // Hook GivePlayerItem to allow for custom handling for refresh item
            {
                if (RunHandler.RunData.runConfig.isEndless) // Overwrite scene change logic for Endless mode
                {
                    if (item.name == "SD_Refresh") // If the player chose refresh, get new items and take away some sparks
                    {
                        Debug.Log("[SpeedDemon] Refreshing items");
                        isRefreshing = true;
                        PersistentPlayerData.SetSparks((int)RunHandler.RunData.playerData.resource - refreshCost.GetRefreshCost());
                        if (refreshCost.freeRefreshes > 0) refreshCost.freeRefreshes--;
                    }
                    else // If the player chose an actual item, give it to them and start the next level
                    {
                        Debug.Log($"[SpeedDemon] Player chose {item.name}");
                        currentLevelRandomInstance = null!; // reset the vars we were storing for this reward
                        currentRewardIsEpic = false;
                        isRefreshing = false;
                        orig(self, player, item);
                    }
                }
                else
                {
                    orig(self, player, item);
                }
            };

            On.GM_API.OnPlayerEnteredPortal += (orig, player) =>
            {
                refreshCost.UpdateRefreshCR();
                orig(player);
            };

            On.RunHandler.StartNewRun += (orig, setConfig, shardID, seed) =>
            {
                orig(setConfig, shardID, seed);
                refreshCost.freeRefreshes = 4;
            };
        }

        public class RefreshCost
        {
            public int GetRefreshCost()
            {
                if (freeRefreshes > 1)
                {
                    CustomItems.CustomItems.templateRefreshItem.description = new UnlocalizedString(
                        $"<color=#c896fa>Refresh</color> reward, Lose <color=#e2b96b>0</color> <color=#c896fa>Sparks</color>\n({freeRefreshes} free <color=#c896fa>Refreshes</color> left)");
                    CustomItems.CustomItems.templateRefreshItem.description.RefreshString();
                    return 0;
                }
                if (freeRefreshes == 1)
                {
                    CustomItems.CustomItems.templateRefreshItem.description = new UnlocalizedString(
                        $"<color=#c896fa>Refresh</color> reward, Lose <color=#e2b96b>0</color> <color=#c896fa>Sparks</color>\n({freeRefreshes} free <color=#c896fa>Refresh</color> left)");
                    CustomItems.CustomItems.templateRefreshItem.description.RefreshString();
                    return 0;
                }
                float luckModifier = luckRefreshCRDampenAmount / (luckRefreshCRDampenAmount + lastLuck - 1);
                Debug.Log($"[SpeedDemon] lastLuck: {lastLuck}, luckModifier: {luckModifier}, luckRefreshCRCount: {luckRefreshCRCount}, lastRefreshCostUpdate: {lastRefreshCostUpdate}");
                lastRefreshCostUpdate = Math.Max((int)(baseRefreshCost * Mathf.Pow(luckModifier, luckRefreshCRCount)), minRefreshCost);
                CustomItems.CustomItems.templateRefreshItem.description = new UnlocalizedString(
                    $"<color=#c896fa>Refresh</color> reward, Lose <color=#e2b96b>{lastRefreshCostUpdate}</color> <color=#c896fa>Sparks</color>");
                CustomItems.CustomItems.templateRefreshItem.description.RefreshString();
                return lastRefreshCostUpdate;
            }

            public void UpdateRefreshCR() // Lol (see SD_ReduceRefreshCostEffect)
            {
                lastLuck = (float)typeof(PlayerStat).GetMethod("GetValue", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(Player.localPlayer.stats.luck, null);
                int tempLuckRefreshCRCount = 0;
                foreach (ItemInstance item in Player.localPlayer.items)
                {
                    if (item.gameObject.GetComponent<SD_ReduceRefreshCostEffect>() != null)
                    {
                        tempLuckRefreshCRCount++;
                    }
                }
                luckRefreshCRCount = tempLuckRefreshCRCount;
            }

            private int baseRefreshCost = 250;
            private int minRefreshCost = 50;
            public int luckRefreshCRCount = 0;
            private float luckRefreshCRDampenAmount = 4f;
            public int lastRefreshCostUpdate = 250;
            public float lastLuck = 1f;
            public int freeRefreshes = 0;

        }
        public static RefreshCost refreshCost = new RefreshCost();

        public static void SpeedDemonAward(Player player)
        {
            ItemInstance refreshItem = CustomItems.CustomItems.templateRefreshItem;
            // It might be worth adding a sanity check to make sure we actually found the refresh item
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
            float currentRefreshCost = refreshCost.GetRefreshCost();
            Debug.Log($"[SpeedDemon] Player has {RunHandler.RunData.playerData.resource} Sparks and a refresh costs {currentRefreshCost}");
            if (RunHandler.RunData.playerData.resource >= currentRefreshCost)
            {
                Debug.Log("[SpeedDemon] Adding refresh item because player can afford it");
                UnlockScreen.me.AddItem(refreshItem);
            }

            // I have no idea what this does :)
            UnlockScreen.me.chooseItem = true;
            UnlockScreen.me.FinishAddingPhase(player, PlayLevelIfDone);
        }

        public static ItemInstance GetRandomItem(System.Random random, bool epicReward)
        {
            if (rewardableItems == null)
            {
                rewardableItems = new();
                foreach (string itemName in CustomItems.EnabledItems.rewardableItemNames)
                {
                    rewardableItems.Add(ItemDatabase.instance.items.FirstOrDefault(e => e.name == itemName));
                }
            }
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
            ItemInstance chosenItem = (ItemInstance)selectItemMethod.Invoke(null, [rewardableItems, chosenRarity, random]);
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
