using Landfall.Modding;
using UnityEngine;
using Landfall.Haste;
using System.Reflection;
using Zorro.Settings;
using Unity.Mathematics;
using UnityEngine.Localization;

namespace SpeedDemon.Difficulties
{
    [LandfallPlugin]
    public class ShardSettings
    {
        static int selectedDiffID = 0;
        static bool isInEndless = false;
        static WorldShard.SelectableRunConfig[] SelectableRunConfigs = [];


        static ShardSettings()
        {
            Debug.Log("[SpeedDemon] Starting ShardSettings Overrides");
            AvailableDifficulties.LoadDifficulties();
            SelectableRunConfigs = [
                AvailableDifficulties.EasyDifficulty,
                AvailableDifficulties.NormalDifficulty,
                AvailableDifficulties.HardDifficulty
            ];

            On.ShardSettingsUI.Open += (orig, self, worldShard) =>
            {
                if (worldShard.isEndlessShard)
                {
                    isInEndless = true;
                    worldShard.SelectableRunConfigs = SelectableRunConfigs;
                } else
                {
                    isInEndless = false;
                }
                orig(self, worldShard);
            };

            On.ShardSettingsUI.ChangeDifficulty += (orig, self, delta) =>
            {
                orig(self, delta);
                if (isInEndless)
                {
                    selectedDiffID += delta;
                    if (selectedDiffID >= SelectableRunConfigs.Length)
                    {
                        selectedDiffID = SelectableRunConfigs.Length - 1;
                    }
                    if (selectedDiffID < 0)
                    {
                        selectedDiffID = 0;
                    }
                    switch (SelectableRunConfigs[selectedDiffID].runConfig.name)
                    {
                        case "SD_Easy":
                            SD_API.StartingSpeed = 80f;
                            SD_API.RampSpeed = 10f;
                            break;
                        case "SD_Normal":
                            SD_API.StartingSpeed = 90f;
                            SD_API.RampSpeed = 15f;
                            break;
                        case "SD_Hard":
                            SD_API.StartingSpeed = 95f;
                            SD_API.RampSpeed = 20f;
                            break;
                    }
                }
            };
        }
    }
}
