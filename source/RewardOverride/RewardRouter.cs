using System.ComponentModel.Composition;
using System.Reflection;
using Landfall.Haste;
using Landfall.Modding;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpeedDemon.RewardOverride
{
    [LandfallPlugin]
    public class RewardRouter
    {
        public enum RewardScreenType
        {
            None,
            SpeedDemonReward,
            StartingItems,
        }

        public static RewardScreenType rewardScreenType = RewardScreenType.None;

        static RewardRouter()
        {
            Debug.Log("[SpeedDemon] ItemReward class initializing...");
            On.RunHandler.TransitionOnLevelCompleted += (orig) =>
            {
                if (!SD_API.InSpeedDemonRun || !RunHandler.InRun)
                {
                    orig();
                    return;
                }
                // Overwrite
                Debug.Log("[SpeedDemon] Level Completed, preparing next scene");

                // Per Level Healing - 25 health per level
                typeof(Player).GetMethod("AddHealth", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(Player.localPlayer, [25f]);

                // Per Level Lives - max lives per level
                Player.localPlayer.EditLives(1);

                // Temporary Sparks
                Player.localPlayer.data.resource += Player.localPlayer.data.temporaryResource;

                // Finish up and move on to the next level
                HasteStats.SetStat(HasteStatType.STAT_ENDLESS_HIGHSCORE, RunHandler.RunData.currentLevel, true, false, false);
                HasteStats.OnLevelComplete();

                // Reward Flag
                bool reward = RunHandler.RunData.currentLevel == 1 || RunHandler.RunData.currentLevel % 2 == 0;
                SpeedDemonReward.currentRewardIsEpic = RunHandler.RunData.currentLevel == 1 || RunHandler.RunData.currentLevel % 10 == 0;
                // Load the next scene (whichever it is)
                if (reward)
                {
                    rewardScreenType = RewardScreenType.SpeedDemonReward;
                    SceneManager.LoadScene("EndlessAwardScene");
                    return;
                }
                RunHandler.RunData.currentNodeStatus = NGOPlayer.PlayerNodeStatus.PostLevelScreenComplete;
            };

            On.EndlessAward.Start += (orig, self) => // Overwrite Endless mode reward scene
            {
                if (!SD_API.InSpeedDemonRun)
                {
                    orig(self);
                    return;
                }
                // Overwrite
                switch (rewardScreenType)
                {
                    case RewardScreenType.SpeedDemonReward:
                        SpeedDemonReward.SpeedDemonAward(Player.localPlayer);
                        return;
                    case RewardScreenType.StartingItems:
                        StartingItems.StartingItemsAward(Player.localPlayer);
                        return;
                    default:
                        Debug.LogError($"[SpeedDemon] Tried to load custom EndlessReward scene with rewardSceneType {RewardScreenType.None}");
                        orig(self);
                        return;
                };
            };

            On.RunHandler.StartAndPlayNewRun += (orig, runConfig, shardID, seed) =>
            {
                if (shardID == 100)
                {
                    RunHandler.StartNewRun(runConfig, shardID, seed);
                    rewardScreenType = RewardScreenType.StartingItems;
                    StartingItems.isChoosingStartingItems = true;
                    SceneManager.LoadScene("EndlessAwardScene");
                }
                else
                {
                    orig(runConfig, shardID, seed);
                }
            };

            On.UnlockScreen.GivePlayerItem += (orig, self, player, item) => // Hook GivePlayerItem to allow for custom handling of UI items
            {                                                       // This should not handle scene management
                if (!SD_API.InSpeedDemonRun)
                {
                    orig(self, player, item);
                    return;
                }
                // Overwrite
                if (item.itemName == SpeedDemonReward.RewardRefresh.TemplateRefreshItem.itemName) // If the player is refreshing reward
                {
                    SpeedDemonReward.OnRefreshItemChosen(item);
                }
                else if(item.itemName == "SD_UI_StartingItemsLeft" || item.itemName == "SD_UI_StartingItemsRight")
                {
                    StartingItems.RotateSelection(item);
                }
                else if(CustomItems.SpeedDemonItems.StartingItemSetItems.FirstOrDefault(obj => obj.name == item.itemName) != null)
                {
                    StartingItems.AddSelectedStartingItems(item);
                }
                else // If the player chose an actual item, give it to them normally
                {
                    SpeedDemonReward.OnRefreshItemNotChosen(item);
                    orig(self, player, item);
                }
            };

            On.GM_API.OnPlayerEnteredPortal += (orig, player) =>
            {
                SpeedDemonReward.RewardRefresh.UpdateRefreshCR();
                orig(player);
            };

            On.RunHandler.StartNewRun += (orig, setConfig, shardID, seed) =>
            {
                orig(setConfig, shardID, seed);
                SpeedDemonReward.RewardRefresh.freeRefreshes = 4;
                StartingItems.chosenSet = null!;
            };
        }
    }
}
