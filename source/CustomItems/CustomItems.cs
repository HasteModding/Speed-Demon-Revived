using CustomItemLib;
using Landfall.Modding;
using Landfall.Haste;
using UnityEngine;
using UnityEngine.Localization;

namespace SpeedDemon.CustomItems
{
    [LandfallPlugin]
    public class CustomItems
    {
        static CustomItems()
        {
            Debug.Log("[SpeedDemon] CustomItems class initializing...");
            ItemFactory.ItemsLoaded += LoadDefaultOverrides;
            ItemFactory.ItemsLoaded += LoadSpecialOverrides;
            ItemFactory.ItemsLoaded += LoadCustomItems;
            //ItemFactory.ItemsLoaded += TestSetRefresh;
            ItemFactory.ItemsLoaded += DestroyUnusedItems;
        }

        public static ItemInstance templateRefreshItem = null!;

        private static void TestSetRefresh()
        {
            templateRefreshItem = ItemDatabase.instance.items.FirstOrDefault(e => e.name == "SD_Refresh");
        }

        private static void DestroyUnusedItems()
        {
            // Grab the refresh item
            templateRefreshItem = ItemDatabase.instance.items.FirstOrDefault(e => e.name == "SD_Refresh");
            // Destroy anything that isn't in the enabled items (or the refresh item)
            List<ItemInstance> toRemove = new();
            foreach (ItemInstance templateItem in ItemDatabase.instance.items)
            {
                if (!templateItem.minorItem && !EnabledItems.rewardableItemNames.Contains(templateItem.name) && templateItem.name != templateRefreshItem.name)
                {
                    toRemove.Add(templateItem);
                }
            }
            foreach (ItemInstance templateItem in toRemove)
            {
                Debug.Log($"[SpeedDemon] Removing unused item {templateItem.name} from ItemDatabase");
                ItemDatabase.instance.items.Remove(templateItem);
                UnityEngine.Object.Destroy(templateItem.gameObject);
            }
        }

        private static void LoadDefaultOverrides()
        {

            ItemFactory.AddItemToDatabase( // 0
                itemName: "Active_Boost",
                rarity: Rarity.Rare,
                effects: new List<ItemEffect>
                {
                    new AddVariable_TemporaryEffect { amount = 1f, duration = 5f, variableType = VariableType.Boost },
                }
            );

            ItemFactory.AddItemToDatabase( // 2
                itemName: "Active_Heal",
                rarity: Rarity.Rare,
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 1f, variableType = VariableType.HealthPercentage },
                }
            );

            ItemFactory.AddItemToDatabase( // 6
                itemName: "Active_Slomo",
                rarity: Rarity.Rare
            );

            ItemFactory.AddItemToDatabase( // 17
                itemName: "ChanceForFullHP",
                triggerConditions: new List<ItemTrigger>
                {
                    new ChanceCheck_IT { probability = 0.2f },
                },
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 0.5f, variableType = VariableType.HealthPercentage },
                }
            );

            ItemFactory.AddItemToDatabase( // 18
                itemName: "CloseCallBoost",
                effects: new List<ItemEffect>
                {
                    new AddVariable_TemporaryEffect { amount = 0.1f, duration = 3f, variableType = VariableType.Boost },
                }
            );

            ItemFactory.AddItemToDatabase( // 24
                itemName: "CloseCallLuck",
                effects: new List<ItemEffect>
                {
                    new AddVariable_TemporaryStats { duration = 3f, stats = ItemFactory.CreatePlayerStats(luck: new PlayerStat { baseValue = 0.25f })},
                }
            );

            ItemFactory.AddItemToDatabase( // 26
                itemName: "CooldownReductionOnPerfectLanding",
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = -0.8f, variableType = VariableType.Cooldown },
                }
            );

            ItemFactory.AddItemToDatabase( // 28
                itemName: "CooldownReductionWithCooldown",
                cooldown: 0.5f,
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = -0.5f, variableType = VariableType.Cooldown },
                }
            );

            ItemFactory.AddItemToDatabase( // 30
                itemName: "GetBoostOnHeal",
                effects: new List<ItemEffect>
                {
                    new AddVariable_TemporaryEffect { amount = 0.25f, duration = 2f, variableType = VariableType.Boost },
                }
            );

            ItemFactory.AddItemToDatabase( // 31
                itemName: "GetBoostWhenTakeDamage",
                effects: new List<ItemEffect>
                {
                    new AddVariable_TemporaryEffect { amount = 0.15f, duration = 10f, variableType = VariableType.Boost },
                }
            );

            ItemFactory.AddItemToDatabase( // 32
                itemName: "GetBoostWithCooldown",
                effects: new List<ItemEffect>
                {
                    new AddVariable_TemporaryEffect { amount = 0.2f, duration = 6f, variableType = VariableType.Boost },
                }
            );

            ItemFactory.AddItemToDatabase( // 38
                itemName: "MaxHealthButDieOnOkOrBadLanding",
                stats: ItemFactory.CreatePlayerStats(maxHealth: new PlayerStat { multiplier = 1.6f }),
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 5f, variableType = VariableType.Damage },
                }

            );

            ItemFactory.AddItemToDatabase( // 42
                itemName: "NEW_Active_StickToGround",
                cooldown: 45f
            );

            ItemFactory.AddItemToDatabase( // 44
                itemName: "NEW_Active_TempSpeed",
                rarity: Rarity.Epic,
                cooldown: 120f,
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 100f, variableType = VariableType.Velocity },
                    new AddVariable_TemporaryEffect { amount = 1f, duration = 6f, variableType = VariableType.Boost }
                }
            );

            ItemFactory.AddItemToDatabase( // 45
                itemName: "NEW_BigEffectOnSlowSpeed",
                cooldown: 10f,
                triggerConditions: new List<ItemTrigger> {
                    new Speed_IT { allowInSlowmode = false, cooldown = 0f, maxSpeed = 100f, minSpeed = 0f, timeToActivate = 0f },
                },
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 30f, variableType = VariableType.Velocity },
                    new AddVariable_TemporaryEffect { amount = 1f, duration = 5f, variableType = VariableType.Boost }
                }
            );

            ItemFactory.AddItemToDatabase( // 48
                itemName: "NEW_HealOnRunning",
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 4f, variableType = VariableType.Health },
                }

            );

            ItemFactory.AddItemToDatabase( // 55
                itemName: "NEW_RegenWithSpeed_Threshold",
                triggerDescription: new LocalizedString("SpeedDemonItems", "NEW_RegenWithSpeed_Threshold_triggerDesc"),
                usesTriggerDescription: true,
                triggerConditions: new List<ItemTrigger>
                {
                    new StatCheck_IT { max = float.PositiveInfinity, min = 1f, varType = VariableType.Boost },
                },
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 1f, variableType = VariableType.Health }
                }
            );

            ItemFactory.AddItemToDatabase( // 58
                itemName: "NEW_StartWithBoost",
                effects: new List<ItemEffect>
                {
                    new AddVariable_TemporaryEffect { amount = 0.5f, duration = 10f, variableType = VariableType.Boost }
                }
            );

            ItemFactory.AddItemToDatabase( // 60
                itemName: "NEW_StartWithHeal",
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 25f, variableType = VariableType.Health }
                }
            );

            ItemFactory.AddItemToDatabase( // 61
                itemName: "NEW_StartWithMoney",
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 50f, variableType = VariableType.Resource }
                }
            );

            ItemFactory.AddItemToDatabase( // 62
                itemName: "OnGetCoin_Boost",
                effects: new List<ItemEffect>
                {
                    new AddVariable_TemporaryEffect { amount = 0.03f, duration = 5f, variableType = VariableType.Boost }
                }
            );

            ItemFactory.AddItemToDatabase( // 65
                itemName: "OnGetCoin_Invuln",
                rarity: Rarity.Rare,
                triggerConditions: new List<ItemTrigger>
                {
                    new ChanceCheck_IT { probability = 0.5f }
                },
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 1f, variableType = VariableType.Resource }
                }
            );

            ItemFactory.AddItemToDatabase( // 66
                itemName: "PermanentBoost",
                stats: ItemFactory.CreatePlayerStats(boost: new PlayerStat { baseValue = 0.3f })
            );

            ItemFactory.AddItemToDatabase( // 67
                itemName: "RandomChanceToBoostOnPerfectLanding",
                effects: new List<ItemEffect>
                {
                    new AddVariable_TemporaryEffect { amount = 0.5f, duration = 5f, variableType = VariableType.Boost },
                }
            );

            ItemFactory.AddItemToDatabase( // 69
                itemName: "RandomChanceToRegen",
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 1f, variableType = VariableType.Health }
                }
            );

            ItemFactory.AddItemToDatabase( // 70
                itemName: "RandomChanceToSaveNonPerfectLanding",
                triggerConditions: new List<ItemTrigger>
                {
                    new ChanceCheck_IT { probability = 0.2f }
                }
            );

            ItemFactory.AddItemToDatabase( // 71
                itemName: "Regen",
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 1f, variableType = VariableType.Health }
                }
            );

            ItemFactory.AddItemToDatabase( // 77
                itemName: "SparksChanceOverTime",
                triggerConditions: new List<ItemTrigger>
                {
                    new ChanceCheck_IT { probability = 0.2f }
                },
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 1f, variableType = VariableType.Resource }
                }
            );

            // Still need to figure out how the damage on spark pickup works
            ItemFactory.AddItemToDatabase( // 80
                itemName: "SparksOverTimeDamgeOnPickUp",
                rarity: Rarity.Epic,
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 4f, variableType = VariableType.Resource }
                }
            );

            ItemFactory.AddItemToDatabase( // 82
                itemName: "Stats_Luck",
                stats: ItemFactory.CreatePlayerStats(luck: new PlayerStat { baseValue = 0.5f })
            );

            ItemFactory.AddItemToDatabase( // 84
                itemName: "Stats_MaxHealthFlat",
                stats: ItemFactory.CreatePlayerStats(maxHealth: new PlayerStat { baseValue = 50f })
            );

            ItemFactory.AddItemToDatabase( // 85
                itemName: "Stats_MaxHealthMultiply",
                stats: ItemFactory.CreatePlayerStats(maxHealth: new PlayerStat { multiplier = 1.35f })
            );

            ItemFactory.AddItemToDatabase( // 89
                itemName: "TakeDamageOnBadLandingButGetPermanentBoost",
                stats: ItemFactory.CreatePlayerStats(boost: new PlayerStat { baseValue = 1f })
            );

            ItemFactory.AddItemToDatabase( // 90
                itemName: "TakeDamageOnCloseCallButGetPermanentBoost",
                rarity: Rarity.Epic,
                stats: ItemFactory.CreatePlayerStats(boost: new PlayerStat { baseValue = 0.6f }),
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 2f, variableType = VariableType.Damage }
                }
            );





            
        }


        private static void LoadSpecialOverrides()
        {
            ItemFactory.CleanScriptsFromItem("BoostPerMissingHealth", typeof(GainEffectPerEffect));
            ItemFactory.AddItemToDatabase( // 14
                itemName: "BoostPerMissingHealth",
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(BoostPerMissingHealthEPMH), "DoEffects")
                }
            );

            ItemFactory.CleanScriptsFromItem("CooldownReductionPerBoost", typeof(GainEffectPerEffect));
            ItemFactory.AddItemToDatabase( // 27
                itemName: "CooldownReductionPerBoost",
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(CooldownReductionPerBoostEPE), "DoEffects")
                }
            );

            ItemFactory.CleanScriptsFromItem("LuckPerMissingHealth", typeof(GainEffectPerEffect));
            ItemFactory.AddItemToDatabase( // 37
                itemName: "LuckPerMissingHealth",
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(LuckPerMissingHealthEPMH), "DoEffects")
                }
            );

            ItemFactory.CleanScriptsFromItem("NEW_BoostIfLastLandingWasPerfect", typeof(Item_EffectIfLastLandingPerfect));
            ItemFactory.AddItemToDatabase( // 46
                itemName: "NEW_BoostIfLastLandingWasPerfect",
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(NEW_BoostIfLastLandingWasPerfectEILLP), "Trigger")
                }
            );

            ItemFactory.CleanScriptsFromItem("RegenerationPerBoost", typeof(GainEffectPerEffect));
            ItemFactory.AddItemToDatabase( // 72
                itemName: "RegenerationPerBoost",
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(RegenerationPerBoostEPE), "DoEffects")
                }
            );

            ItemFactory.CleanScriptsFromItem("Stats_LuckOnFullHealth", typeof(ConditionalStats));
            ItemFactory.AddItemToDatabase( // 83
                itemName: "Stats_LuckOnFullHealth",
                usesEffectDescription: false,
                usesTriggerDescription: false,
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(Stats_LuckOnFullHealthLETE), "DoEffects")
                }
            );

            ItemFactory.AddItemToDatabase( // 29 // Make effect temporary time
                itemName: "GetBoostAtFullHealth",
                usesTriggerDescription: false,
                triggerType: ItemTriggerType.None,
                triggerConditions: new(),
                effects: new(),
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(GetBoostAtFullHealthLETE), "DoEffects")
                }
            );


            ItemFactory.CleanScriptsFromItem("BoostPerPerfectLandingStreak", typeof(StackingLandingStats));
            ItemFactory.AddItemToDatabase( // 15
                itemName: "BoostPerPerfectLandingStreak",
                //usesEffectDescription: false,
                //usesTriggerDescription: false,
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(BoostPerPerfectLandingStreakSLS), "DoTrigger")
                }
            );

            ItemFactory.AddItemToDatabase( // 68
                itemName: "RandomChanceToGetLuck",
                title: new UnlocalizedString("Enchanted Coin"),
                cooldown: 0f,
                triggerType: ItemTriggerType.EnterRunningLevel,
                triggerConditions: new(),
                effects: new(),
                stats: ItemFactory.CreatePlayerStats(luck: new PlayerStat { baseValue = 0.5f }),
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(RandomChanceToGetLuckEPL), "DoEffects")
                }
            );

            ItemFactory.CleanScriptsFromItem("BoostPerSpark", typeof(GainEffectPerEffect));
            ItemFactory.AddItemToDatabase( // 16
                itemName: "BoostPerSpark",
                rarity: Rarity.Legendary,
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(BoostPerSparkEPE), "DoEffects")
                }
            );

        }



        private static void LoadCustomItems()
        {
            ItemFactory.AddItemToDatabase( // s0
                itemName: "SD_Refresh",
                title: new UnlocalizedString("Refresh"),
                flavorText: new UnlocalizedString("There are plenty more where that came from."),
                triggerType: ItemTriggerType.BoughtItem,
                description: new UnlocalizedString("<color=#c896fa>Refresh</color> reward, Lose <color=#e2b96b>250</color> <color=#c896fa>Sparks</color>"),
                usesEffectDescription: true
            );

            ItemFactory.AddItemToDatabase( // s1
                itemName: "SD_EpicPermanentBoost",
                rarity: Rarity.Epic,
                title: new UnlocalizedString("Plutonium Ring"),
                flavorText: new UnlocalizedString("It's probably not safe to be wearing this."),
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(SD_EpicPermanentBoostDSE), "DoNothing")
                }
            );

            ItemFactory.AddItemToDatabase( // s2
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

            ItemFactory.AddItemToDatabase( // s3
                itemName: "SD_ReduceRefreshCost",
                rarity: Rarity.Legendary,
                title: new UnlocalizedString("Charisma"),
                flavorText: new UnlocalizedString("Infused with ancient herbs and forgotten rituals."),
                stats: ItemFactory.CreatePlayerStats(luck: new PlayerStat { baseValue = 0.3f }),
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(SD_ReduceRefreshCostEffect), "DoNothing")
                }
            );

            ItemFactory.AddItemToDatabase( // s4
                itemName: "SD_LuckAndHeal",
                title: new UnlocalizedString("Incense"),
                flavorText: new UnlocalizedString("You didn’t haggle, they just <i>liked</i> you."),
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

            ItemFactory.AddItemToDatabase( // s5
                itemName: "SD_LuckAndBoost",
                title: new UnlocalizedString("Stimulants"),
                flavorText: new UnlocalizedString("I wouldn't take too many of these..."),
                stats: ItemFactory.CreatePlayerStats(luck: new PlayerStat { baseValue = 0.3f }, boost: new PlayerStat { baseValue = 0.15f })
            );

            ItemFactory.AddItemToDatabase( // s6
                itemName: "SD_MaxHealthAndBoost",
                title: new UnlocalizedString("Leather Chest"),
                flavorText: new UnlocalizedString("It's better than nothing."),
                stats: ItemFactory.CreatePlayerStats(maxHealth: new PlayerStat { multiplier = 1.2f }, boost: new PlayerStat { baseValue = 0.15f })
            );

            ItemFactory.AddItemToDatabase( // s7
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



        }



    }
}
