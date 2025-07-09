using Landfall.Haste;
using Landfall.Haste.Music;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Zorro.Core;

namespace SpeedDemon.Difficulties
{
    class AvailableDifficulties
    {
        //private static Dictionary<String, LevelGenConfig> LoadedLevelGenConfigs = new();
        //private static Dictionary<String, MusicPlaylist> LoadedMusicPlaylists = new();

        private static RunConfig _easyDifficulty = new RunConfig();
        public static WorldShard.SelectableRunConfig EasyDifficulty = new WorldShard.SelectableRunConfig();

        private static RunConfig _normalDifficulty = new RunConfig();
        public static WorldShard.SelectableRunConfig NormalDifficulty = new WorldShard.SelectableRunConfig();

        private static RunConfig _hardDifficulty = new RunConfig();
        public static WorldShard.SelectableRunConfig HardDifficulty = new WorldShard.SelectableRunConfig();

        /*public static void LoadLevelGenerators()
        {
            LevelGenConfig[] LevelGenConfigs = ScriptableObject.FindObjectsOfType<LevelGenConfig>();
            foreach (var config in LevelGenConfigs)
            {
                LoadedLevelGenConfigs.Add(
                    config.name,
                    config
                );
            }
        }

        public static void LoadMusicPlaylists()
        {
            MusicPlaylist[] MusicPlaylists = MusicPlaylist.FindObjectsOfType<MusicPlaylist>();
            foreach (var playlist in MusicPlaylists)
            {
                LoadedMusicPlaylists.Add(
                    playlist.name,
                    playlist
                );
            }
        }*/

        public static void LoadDifficulties()
        {
            //LoadLevelGenerators();
            //LoadMusicPlaylists();

            _easyDifficulty = ScriptableObject.CreateInstance<RunConfig>();
            _easyDifficulty.name = "SD_Easy";
            _easyDifficulty.title = "Walking Demon";
            _easyDifficulty.ShardName = new UnlocalizedString("Speed Demon's Endless Shard");
            _easyDifficulty.nrOfLevels = 10;
            _easyDifficulty.startingItems = [];
            _easyDifficulty.StartUIColorOverride = new UnityEngine.Color(0.3857526183128357f, 0.24056601524353027f, 1f, 1f);
            _easyDifficulty.startDifficulty = 10;
            _easyDifficulty.endDifficulty = 17.5f;
            _easyDifficulty.maxSlope = 0;
            _easyDifficulty.minSlope = 0;
            _easyDifficulty.minLength = 12;
            _easyDifficulty.maxLegnth = 12;
            _easyDifficulty.minSpeed = 85f;
            _easyDifficulty.maxSpeed = 88.75f;
            _easyDifficulty.keepRunningDifficultyBump = 3;
            _easyDifficulty.keepRunningDifficultyIncreasePerLevel = 2;
            _easyDifficulty.keepRunningSpeedBump = 0;
            _easyDifficulty.keepRunningSpeedIncreasePerLevel = 1.5f;
            _easyDifficulty.bossScene = "";
            _easyDifficulty.isEndless = true;
            _easyDifficulty.isAscension = false;
            _easyDifficulty.addHealthPerLevelCompleted = 0;
            _easyDifficulty.addLifeEveryNLevels = 5;
            _easyDifficulty.noiseAdditions = [];
            _easyDifficulty.propAdditions = [];
            _easyDifficulty.genObjects = [];
            _easyDifficulty.keyProps = [];
            _easyDifficulty.bossTeir = 2;
            _easyDifficulty.musicPlaylist = GetMusicPlaylistById(58814);
            _easyDifficulty.categories = [
                GetCategoryById(10044),
                GetCategoryById(10046),
                GetCategoryById(10050),
                GetCategoryById(10052),
                GetCategoryById(10048),
            ];

            _normalDifficulty = ScriptableObject.CreateInstance<RunConfig>();
            _normalDifficulty.name = "SD_Normal";
            _normalDifficulty.title = "Speed Demon";
            _normalDifficulty.ShardName = new UnlocalizedString("Speed Demon's Endless Shard");
            _normalDifficulty.nrOfLevels = 10;
            _normalDifficulty.startingItems = [];
            _normalDifficulty.StartUIColorOverride = new UnityEngine.Color(0.3857526183128357f, 0.24056601524353027f, 1f, 1f);
            _normalDifficulty.startDifficulty = 10;
            _normalDifficulty.endDifficulty = 17.5f;
            _normalDifficulty.maxSlope = 0;
            _normalDifficulty.minSlope = 0;
            _normalDifficulty.minLength = 12;
            _normalDifficulty.maxLegnth = 12;
            _normalDifficulty.minSpeed = 85f;
            _normalDifficulty.maxSpeed = 88.75f;
            _normalDifficulty.keepRunningDifficultyBump = 3;
            _normalDifficulty.keepRunningDifficultyIncreasePerLevel = 2;
            _normalDifficulty.keepRunningSpeedBump = 0;
            _normalDifficulty.keepRunningSpeedIncreasePerLevel = 1.5f;
            _normalDifficulty.bossScene = "";
            _normalDifficulty.isEndless = true;
            _normalDifficulty.isAscension = false;
            _normalDifficulty.addHealthPerLevelCompleted = 0;
            _normalDifficulty.addLifeEveryNLevels = 5;
            _normalDifficulty.noiseAdditions = [];
            _normalDifficulty.propAdditions = [];
            _normalDifficulty.genObjects = [];
            _normalDifficulty.keyProps = [];
            _normalDifficulty.bossTeir = 2;
            _normalDifficulty.musicPlaylist = GetMusicPlaylistById(58814);
            _normalDifficulty.categories = [
                GetCategoryById(10044),
                GetCategoryById(10046),
                GetCategoryById(10050),
                GetCategoryById(10052),
                GetCategoryById(10048),
            ];

            _hardDifficulty = ScriptableObject.CreateInstance<RunConfig>();
            _hardDifficulty.name = "SD_Hard";
            _hardDifficulty.title = "Lightspeed Demon";
            _hardDifficulty.ShardName = new UnlocalizedString("Speed Demon's Endless Shard");
            _hardDifficulty.nrOfLevels = 10;
            _hardDifficulty.startingItems = [];
            _hardDifficulty.StartUIColorOverride = new UnityEngine.Color(0.3857526183128357f, 0.24056601524353027f, 1f, 1f);
            _hardDifficulty.startDifficulty = 10;
            _hardDifficulty.endDifficulty = 17.5f;
            _hardDifficulty.maxSlope = 0;
            _hardDifficulty.minSlope = 0;
            _hardDifficulty.minLength = 12;
            _hardDifficulty.maxLegnth = 12;
            _hardDifficulty.minSpeed = 85f;
            _hardDifficulty.maxSpeed = 88.75f;
            _hardDifficulty.keepRunningDifficultyBump = 3;
            _hardDifficulty.keepRunningDifficultyIncreasePerLevel = 2;
            _hardDifficulty.keepRunningSpeedBump = 0;
            _hardDifficulty.keepRunningSpeedIncreasePerLevel = 1.5f;
            _hardDifficulty.bossScene = "";
            _hardDifficulty.isEndless = true;
            _hardDifficulty.isAscension = false;
            _hardDifficulty.addHealthPerLevelCompleted = 0;
            _hardDifficulty.addLifeEveryNLevels = 5;
            _hardDifficulty.noiseAdditions = [];
            _hardDifficulty.propAdditions = [];
            _hardDifficulty.genObjects = [];
            _hardDifficulty.keyProps = [];
            _hardDifficulty.bossTeir = 2;
            _hardDifficulty.musicPlaylist = GetMusicPlaylistById(58814);
            _hardDifficulty.categories = [
                GetCategoryById(10044),
                GetCategoryById(10046),
                GetCategoryById(10050),
                GetCategoryById(10052),
                GetCategoryById(10048),
            ];

            EasyDifficulty = new WorldShard.SelectableRunConfig()
            {
                name = new UnlocalizedString("Walking Demon"),
                descriptions = [
                    new UnlocalizedString("An easier Speed Demon difficulty"),
                    new UnlocalizedString("Starting collapse speed is 80m/s"),
                    new UnlocalizedString("Collapse Ramp Up speed is 10m/s every 2 fragments")
                ],
                runConfig = _easyDifficulty
            };

            NormalDifficulty = new WorldShard.SelectableRunConfig()
            {
                name = new UnlocalizedString("Speed Demon"),
                descriptions = [
                    new UnlocalizedString("The regular Speed Demon difficulty"),
                    new UnlocalizedString("Starting collapse speed is 90m/s"),
                    new UnlocalizedString("Collapse Ramp Up speed is 15m/s every 2 fragments")
                ],
                runConfig = _normalDifficulty
            };

            HardDifficulty = new WorldShard.SelectableRunConfig()
            {
                name = new UnlocalizedString("Lightspeed Demon"),
                descriptions = [
                    new UnlocalizedString("A harder Speed Demon difficulty"),
                    new UnlocalizedString("Starting collapse speed is 95m/s"),
                    new UnlocalizedString("Collapse Ramp Up speed is 20m/s every 2 fragments")
                ],
                runConfig = _hardDifficulty
            };
        }

        private static MusicPlaylist? GetMusicPlaylistById(int id)
        {
            UnityEngine.Object obj = Resources.InstanceIDToObject(id);
            if (obj is MusicPlaylist)
            {
                return (MusicPlaylist)obj;
            }
            else
            {
                Debug.LogWarning($"MusicPlaylist with ID {id} not found.");
                return null;
            }
        }

        private static LevelGenConfig? GetCategoryById(int id)
        {
            UnityEngine.Object obj = Resources.InstanceIDToObject(id);
            if(obj is LevelGenConfig)
            {
                return (LevelGenConfig)obj;
            } else
            {
                Debug.LogWarning($"LevelGenConfig with ID {id} not found.");
                return null;
            }
        }
    }
}