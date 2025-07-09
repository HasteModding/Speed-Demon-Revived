using Landfall.Modding;
using UnityEngine;
using Landfall.Haste;
using System.Reflection;
using Zorro.Settings;
using Unity.Mathematics;
using UnityEngine.Localization;
using SpeedDemon.Difficulties;

namespace SpeedDemon
{
    [LandfallPlugin]
    public class SpeedDemonMain
    {
        static public float playerEndLevelSpeed = 0f;

        static SpeedDemonMain()
        {
            Debug.Log("[SpeedDemon] SpeedDemonMain class initializing...");

            On.RunHandler.GetLevelSpeed += (orig) => // Custom corruption speed
            {
                if (SD_API.InSpeedDemonRun)
                {
                    float startingSpeed = SD_API.StartingSpeed;
                    float rampSpeed = SD_API.RampSpeed;
                    float speed = (startingSpeed + rampSpeed * (RunHandler.RunData.currentLevel / 2));
                    return speed;
                }
                else
                {
                    return orig();
                }
            };

            On.PlayerCharacter.PlayerData.GetBoost += (orig, self) => // Override GetBoost to raise the speed limit
            {
                if (SD_API.InSpeedDemonRun)
                {
                    // A couple of reflections to get the method
                    PlayerCharacter player = (PlayerCharacter)typeof(PlayerCharacter.PlayerData).GetField("player", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
                    MethodInfo getValueMethod = typeof(PlayerStat).GetMethod("GetValue", BindingFlags.Instance | BindingFlags.NonPublic);
                    float boostValue = (float)getValueMethod.Invoke(player.player.stats.boost, null);
                    return Mathf.Clamp(self.speedBoost + boostValue, 0f, 25f);
                }
                else return orig(self);
            };

            On.ItemInstance.EditCooldown += (orig, self, delta) => // Rough patch for cooldown exploit
            {
                if (SD_API.InSpeedDemonRun)
                {
                    // Don't let a cooldown reduction lower the cooldown to less than a certain percent remaining
                    float currentCooldown = self.instanceData.GetEntry<CooldownCounterEntry>(null).Value;
                    float newCooldown = Mathf.Max(Mathf.Min(currentCooldown - delta, self.cooldown * 0.9f), currentCooldown);
                    self.instanceData.GetEntry<CooldownCounterEntry>(null).Value = newCooldown;
                    //Debug.Log($"[SpeedDemon] Reduction {delta} increased cooldown counter from {currentCooldown} to {newCooldown}");
                }
                else orig(self, delta);
            };

            On.LevelGenerator.SetPortalPos += (orig, self) => // Hijacking this method to grab a reference to the portal object as it's being placed
            {                                                 // (There are probably better ways to do this)
                if (SD_API.InSpeedDemonRun)
                {
                    Portal portal = self.GetComponentInChildren<Portal>();
                    if (portal != null)
                    {
                        // Make the portal bigger
                        portal.transform.localScale = new Vector3(4f, 4f, 4f);
                        // Continue with normal method
                        SplineGenerator splines = (SplineGenerator)typeof(LevelGenerator).GetField("splines", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
                        typeof(SplineGenerator).GetMethod("SetTransformAtEnd", BindingFlags.Instance | BindingFlags.NonPublic)
                            .Invoke(splines, [portal.transform]);
                    }
                }
                else orig(self);
            };

            On.PlayerCharacter.Awake += (orig, self) => // Ensure necessary scripts are on the PlayerCharacter
            {
                orig(self);
                if (SD_API.InSpeedDemonRun)
                {
                    Debug.Log($"[SpeedDemon] PlayerCharacter awoke, adding boundary scripts");
                    self.gameObject.AddComponent<SpeedDemonCeiling>();
                    self.gameObject.AddComponent<SpeedDemonWalls>();
                }
            };


            GM_API.NewLevel += OnNewLevel;
            GM_API.LevelRestart += OnNewLevel;

            GM_API.SpawnedInHub += SetSDEndRunFact;
            GM_API.StartNewRun += SetSDStartRunFact;
            

            On.PersistentPlayerData.ResetToLevelStart += (orig, self) => // See above, we also have to do it if the player dies and restarts the level
            {
                orig(self);
                if (SD_API.InSpeedDemonRun)
                {   // The game doesn't normally trigger "EnterRunningLevel" triggers on a level restart
                    // That's a problem for us, because builds relying on slingshot will break if you die, so we just call it manually here
                    Player.localPlayer.TriggerItemsOfType(ItemTriggerType.EnterRunningLevel);
                }
            };

            On.GM_API.OnPlayerEnteredPortal += (orig, player) =>
            {
                if (SD_API.InSpeedDemonRun) playerEndLevelSpeed = player.character.refs.rig.velocity.magnitude;
                orig(player);
            };

            On.RunHandler.WinRun += (orig, transitionOutOverride, transTime) =>
            {
                if (SD_API.InSpeedDemonRun) playerEndLevelSpeed = 0f;
                orig(transitionOutOverride, transTime);
            };

            On.RunHandler.LoseRun += (orig, transitionOutOverride, transTime) =>
            {
                if (SD_API.InSpeedDemonRun) playerEndLevelSpeed = 0f;
                orig(transitionOutOverride, transTime);
            };

            On.UI_UnlockedItemsScreen.GetItems += (orig, self) =>
            {
                self.allItems.Clear();
                self.allItems.AddRange(CustomItems.SpeedDemonItems.RewardableItems);
                self.unlockedItems.Clear();
                self.unlockedItems.AddRange(CustomItems.SpeedDemonItems.RewardableItems);
            };

            On.WorldShard.PlayLevel += (orig, self) =>
            {
                if (self.isEndlessShard)
                {
                    var config = self.SelectableRunConfigs[2];

                    string jsonConfig = JsonUtility.ToJson(config);
                    Debug.Log("[SpeedDemon] Selectable Run Config : " + jsonConfig);

                    string jsonRunConfig = JsonUtility.ToJson(config.runConfig);
                    Debug.Log("[SpeedDemon] Run Config : " + jsonRunConfig);

                    foreach (LevelGenConfig genConf in config.runConfig.categories)
                    {
                        string jsonLevenGenConfig = JsonUtility.ToJson(genConf);
                        Debug.Log("[SpeedDemon] genConf : " + genConf.name + jsonLevenGenConfig);
                    }

                    string jsonMusicPlaylist = JsonUtility.ToJson(config.runConfig.musicPlaylist);
                    Debug.Log("[SpeedDemon] MusicPlaylist : " + config.runConfig.musicPlaylist.name + jsonMusicPlaylist);
                }
                orig(self);
            };
        }

        private static void ShardSettingsUI_Open(On.ShardSettingsUI.orig_Open orig, ShardSettingsUI self, WorldShard worldShard)
        {
            throw new NotImplementedException();
        }

        private static void OnNewLevel()
        {
            if (SD_API.InSpeedDemonRun)
            {
                float startLevelSpeed = Mathf.Max(playerEndLevelSpeed, PlayerCharacter.localPlayer.refs.rig.velocity.magnitude);
                Vector3 newVelocity = PlayerCharacter.localPlayer.refs.rig.velocity.normalized * startLevelSpeed;
                PlayerCharacter.localPlayer.refs.rig.velocity = newVelocity;
                Debug.Log($"[SpeedDemon] Starting level, setting velocity vector to {newVelocity}");
            }
        }

        private static void SetSDStartRunFact()
        {
            if (RunHandler.RunData.runConfig.isEndless)
            {
                Debug.Log($"[SpeedDemon] Speed Demon run is starting, setting fact");
                SD_API.InSpeedDemonRun = true;
            }
        }

        private static void SetSDEndRunFact()
        {
            if (SD_API.InSpeedDemonRun)
            {
                Debug.Log($"[SpeedDemon] Speed Demon run has ended, setting fact");
                SD_API.InSpeedDemonRun = false;
            }
        }
    }
}
