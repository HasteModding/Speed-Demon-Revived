using CustomItemLib;
using Landfall.Haste;
using UnityEngine.Localization;

namespace SpeedDemon.CustomItems.CustomItemDefinitions
{
    internal class CustomRewardables
    {
        internal static void LoadCustomItems()
        {
            ItemFactory.AddItemToDatabase( // c0
                itemName: "SD_EpicPermanentBoost",
                rarity: Rarity.Epic,
                title: new UnlocalizedString("Plutonium Ring"),
                flavorText: new UnlocalizedString("It's probably not safe to be wearing this."),
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(SD_EpicPermanentBoostDSE), "DoNothing")
                }
            );
            ItemLoader.CopyDefaultMeshes("SD_EpicPermanentBoost", "PermanentBoost");

            ItemFactory.AddItemToDatabase( // c1
                itemName: "SD_CommonCRWC",
                title: new UnlocalizedString("Broken Stopwatch"),
                flavorText: new UnlocalizedString("A broken clock, as they say."),
                cooldown: 1f,
                triggerType: ItemTriggerType.Continious,
                triggerConditions: new List<ItemTrigger>
                {
                    new ChanceCheck_IT { probability = 0.25f }
                },
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = -0.25f, variableType = VariableType.Cooldown },
                }
            );
            ItemLoader.CopyDefaultMeshes("SD_CommonCRWC", "CloseCallCooldown");

            ItemFactory.AddItemToDatabase( // c2
                itemName: "SD_ReduceRefreshCost",
                rarity: Rarity.Legendary,
                title: new UnlocalizedString("Charisma"),
                flavorText: new UnlocalizedString("You didn’t haggle, they just <i>liked</i> you."),
                stats: ItemFactory.CreatePlayerStats(luck: new PlayerStat { baseValue = 0.3f }),
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(SD_ReduceRefreshCostEffect), "DoNothing")
                }
            );
            ItemLoader.CopyDefaultMeshes("SD_ReduceRefreshCost", "BoostPerPerfectLandingStreak");

            ItemFactory.AddItemToDatabase( // c3
                itemName: "SD_LuckAndHeal",
                title: new UnlocalizedString("Incense"),
                flavorText: new UnlocalizedString("Infused with ancient herbs and forgotten rituals."),
                stats: ItemFactory.CreatePlayerStats(luck: new PlayerStat { baseValue = 0.3f }),
                triggerType: ItemTriggerType.Continious,
                cooldown: 2f,
                triggerConditions: new List<ItemTrigger>
                {
                    new ChanceCheck_IT { probability = 0.25f }
                },
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 1f, variableType = VariableType.Health },
                }
            );
            ItemLoader.CopyDefaultMeshes("SD_LuckAndHeal", "RegenPerMissingHealth");

            ItemFactory.AddItemToDatabase( // c4
                itemName: "SD_LuckAndBoost",
                title: new UnlocalizedString("Stimulants"),
                flavorText: new UnlocalizedString("I wouldn't take too many of these..."),
                stats: ItemFactory.CreatePlayerStats(luck: new PlayerStat { baseValue = 0.3f }, boost: new PlayerStat { baseValue = 0.15f })
            );
            ItemLoader.CopyDefaultMeshes("SD_LuckAndBoost", "GetBoostOnHeal");

            ItemFactory.AddItemToDatabase( // c5
                itemName: "SD_MaxHealthAndBoost",
                title: new UnlocalizedString("Leather Chest"),
                flavorText: new UnlocalizedString("It's better than nothing."),
                stats: ItemFactory.CreatePlayerStats(maxHealth: new PlayerStat { multiplier = 1.2f }, boost: new PlayerStat { baseValue = 0.15f })
            );
            ItemLoader.CopyDefaultMeshes("SD_MaxHealthAndBoost", "MaxHealthButDieOnOkOrBadLanding");

            ItemFactory.AddItemToDatabase( // c6
                itemName: "SD_SpeedOnPerfectLandingStreak",
                rarity: Rarity.Legendary,
                title: new UnlocalizedString("Boots of the Speedster"),
                triggerDescription: new LocalizedString("SpeedDemonItems", "SD_SpeedOnPerfectLandingStreak_triggerDesc"),
                usesTriggerDescription: true,
                flavorText: new UnlocalizedString("They won't see anything but a red streak."),
                triggerType: ItemTriggerType.Landing,
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(SD_SpeedOnPerfectLandingStreakEILLP), "Trigger")
                }
            );
            ItemLoader.CopyDefaultMeshes("SD_SpeedOnPerfectLandingStreak", "Active_Boost");
        }
    }
}
