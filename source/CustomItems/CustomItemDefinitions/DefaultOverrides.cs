using CustomItemLib;
using Landfall.Modding;
using Landfall.Haste;
using UnityEngine.Localization;

namespace SpeedDemon.CustomItems.CustomItemDefinitions
{
    [LandfallPlugin]
    internal class DefaultOverrides
    {
        internal static void LoadDefaultOverrides()
        {
            LoadSimpleOverrides();
            LoadComplexOverrides();
        }

        private static void LoadSimpleOverrides()
        {
            ItemFactory.AddItemToDatabase( // 0
                itemName: "SD_Active_Boost",
                rarity: Rarity.Rare,
                effects: new List<ItemEffect>
                {
                    new AddVariable_TemporaryEffect { amount = 1f, duration = 5f, variableType = VariableType.Boost },
                }
            );

            ItemFactory.AddItemToDatabase( // 2
                itemName: "SD_Active_Heal",
                rarity: Rarity.Rare,
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 1f, variableType = VariableType.HealthPercentage },
                }
            );

            ItemFactory.AddItemToDatabase( // 6
                itemName: "SD_Active_Slomo",
                rarity: Rarity.Rare
            );

            ItemFactory.AddItemToDatabase( // 17
                itemName: "SD_ChanceForFullHP",
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
                itemName: "SD_CloseCallBoost",
                effects: new List<ItemEffect>
                {
                    new AddVariable_TemporaryEffect { amount = 0.1f, duration = 3f, variableType = VariableType.Boost },
                }
            );

            ItemFactory.AddItemToDatabase( // 24
                itemName: "SD_CloseCallLuck",
                effects: new List<ItemEffect>
                {
                    new AddVariable_TemporaryStats { duration = 3f, stats = ItemFactory.CreatePlayerStats(luck: new PlayerStat { baseValue = 0.25f })},
                }
            );

            ItemFactory.AddItemToDatabase( // 26
                itemName: "SD_CooldownReductionOnPerfectLanding",
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = -0.8f, variableType = VariableType.Cooldown },
                }
            );

            ItemFactory.AddItemToDatabase( // 28
                itemName: "SD_CooldownReductionWithCooldown",
                cooldown: 0.5f,
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = -0.5f, variableType = VariableType.Cooldown },
                }
            );

            ItemFactory.AddItemToDatabase( // 30
                itemName: "SD_GetBoostOnHeal",
                effects: new List<ItemEffect>
                {
                    new AddVariable_TemporaryEffect { amount = 0.25f, duration = 2f, variableType = VariableType.Boost },
                }
            );

            ItemFactory.AddItemToDatabase( // 31
                itemName: "SD_GetBoostWhenTakeDamage",
                effects: new List<ItemEffect>
                {
                    new AddVariable_TemporaryEffect { amount = 0.15f, duration = 10f, variableType = VariableType.Boost },
                }
            );

            ItemFactory.AddItemToDatabase( // 32
                itemName: "SD_GetBoostWithCooldown",
                effects: new List<ItemEffect>
                {
                    new AddVariable_TemporaryEffect { amount = 0.2f, duration = 6f, variableType = VariableType.Boost },
                }
            );

            ItemFactory.AddItemToDatabase( // 38
                itemName: "SD_MaxHealthButDieOnOkOrBadLanding",
                stats: ItemFactory.CreatePlayerStats(maxHealth: new PlayerStat { multiplier = 1.6f }),
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 5f, variableType = VariableType.Damage },
                }

            );

            ItemFactory.AddItemToDatabase( // 42
                itemName: "SD_NEW_Active_StickToGround",
                cooldown: 45f
            );

            ItemFactory.AddItemToDatabase( // 44
                itemName: "SD_NEW_Active_TempSpeed",
                rarity: Rarity.Epic,
                cooldown: 120f,
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 100f, variableType = VariableType.Velocity },
                    new AddVariable_TemporaryEffect { amount = 1f, duration = 6f, variableType = VariableType.Boost }
                }
            );

            ItemFactory.AddItemToDatabase( // 45
                itemName: "SD_NEW_BigEffectOnSlowSpeed",
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
                itemName: "SD_NEW_HealOnRunning",
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 4f, variableType = VariableType.Health },
                }

            );

            ItemFactory.AddItemToDatabase( // 55
                itemName: "SD_NEW_RegenWithSpeed_Threshold",
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
                itemName: "SD_NEW_StartWithBoost",
                effects: new List<ItemEffect>
                {
                    new AddVariable_TemporaryEffect { amount = 0.5f, duration = 10f, variableType = VariableType.Boost }
                }
            );

            ItemFactory.AddItemToDatabase( // 60
                itemName: "SD_NEW_StartWithHeal",
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 25f, variableType = VariableType.Health }
                }
            );

            ItemFactory.AddItemToDatabase( // 61
                itemName: "SD_NEW_StartWithMoney",
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 50f, variableType = VariableType.Resource }
                }
            );

            ItemFactory.AddItemToDatabase( // 62
                itemName: "SD_OnGetCoin_Boost",
                effects: new List<ItemEffect>
                {
                    new AddVariable_TemporaryEffect { amount = 0.03f, duration = 5f, variableType = VariableType.Boost }
                }
            );

            ItemFactory.AddItemToDatabase( // 65
                itemName: "SD_OnGetCoin_Invuln",
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
                itemName: "SD_PermanentBoost",
                stats: ItemFactory.CreatePlayerStats(boost: new PlayerStat { baseValue = 0.3f })
            );

            ItemFactory.AddItemToDatabase( // 67
                itemName: "SD_RandomChanceToBoostOnPerfectLanding",
                effects: new List<ItemEffect>
                {
                    new AddVariable_TemporaryEffect { amount = 0.5f, duration = 5f, variableType = VariableType.Boost },
                }
            );

            ItemFactory.AddItemToDatabase( // 69
                itemName: "SD_RandomChanceToRegen",
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 1f, variableType = VariableType.Health }
                }
            );

            ItemFactory.AddItemToDatabase( // 70
                itemName: "SD_RandomChanceToSaveNonPerfectLanding",
                triggerConditions: new List<ItemTrigger>
                {
                    new ChanceCheck_IT { probability = 0.2f }
                }
            );

            ItemFactory.AddItemToDatabase( // 71
                itemName: "SD_Regen",
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 1f, variableType = VariableType.Health }
                }
            );

            ItemFactory.AddItemToDatabase( // 77
                itemName: "SD_SparksChanceOverTime",
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
                itemName: "SD_SparksOverTimeDamgeOnPickUp",
                rarity: Rarity.Epic,
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 4f, variableType = VariableType.Resource }
                }
            );

            ItemFactory.AddItemToDatabase( // 82
                itemName: "SD_Stats_Luck",
                stats: ItemFactory.CreatePlayerStats(luck: new PlayerStat { baseValue = 0.5f })
            );

            ItemFactory.AddItemToDatabase( // 84
                itemName: "SD_Stats_MaxHealthFlat",
                stats: ItemFactory.CreatePlayerStats(maxHealth: new PlayerStat { baseValue = 50f })
            );

            ItemFactory.AddItemToDatabase( // 85
                itemName: "SD_Stats_MaxHealthMultiply",
                stats: ItemFactory.CreatePlayerStats(maxHealth: new PlayerStat { multiplier = 1.35f })
            );

            ItemFactory.AddItemToDatabase( // 89
                itemName: "SD_TakeDamageOnBadLandingButGetPermanentBoost",
                stats: ItemFactory.CreatePlayerStats(boost: new PlayerStat { baseValue = 1f })
            );

            ItemFactory.AddItemToDatabase( // 90
                itemName: "SD_TakeDamageOnCloseCallButGetPermanentBoost",
                rarity: Rarity.Epic,
                stats: ItemFactory.CreatePlayerStats(boost: new PlayerStat { baseValue = 0.6f }),
                effects: new List<ItemEffect>
                {
                    new AddVariable_Effect { amount = 2f, variableType = VariableType.Damage }
                }
            );
        }

        private static void LoadComplexOverrides()
        {
            ItemFactory.CleanScriptsFromItem("SD_BoostPerMissingHealth", typeof(GainEffectPerEffect));
            ItemFactory.AddItemToDatabase( // 14
                itemName: "SD_BoostPerMissingHealth",
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(BoostPerMissingHealthEPMH), "DoEffects")
                }
            );

            ItemFactory.CleanScriptsFromItem("SD_CooldownReductionPerBoost", typeof(GainEffectPerEffect));
            ItemFactory.AddItemToDatabase( // 27
                itemName: "SD_CooldownReductionPerBoost",
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(CooldownReductionPerBoostEPE), "DoEffects")
                }
            );

            ItemFactory.CleanScriptsFromItem("SD_LuckPerMissingHealth", typeof(GainEffectPerEffect));
            ItemFactory.AddItemToDatabase( // 37
                itemName: "SD_LuckPerMissingHealth",
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(LuckPerMissingHealthEPMH), "DoEffects")
                }
            );

            ItemFactory.CleanScriptsFromItem("SD_NEW_BoostIfLastLandingWasPerfect", typeof(Item_EffectIfLastLandingPerfect));
            ItemFactory.AddItemToDatabase( // 46
                itemName: "SD_NEW_BoostIfLastLandingWasPerfect",
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(NEW_BoostIfLastLandingWasPerfectEILLP), "Trigger")
                }
            );

            ItemFactory.CleanScriptsFromItem("SD_RegenerationPerBoost", typeof(GainEffectPerEffect));
            ItemFactory.AddItemToDatabase( // 72
                itemName: "SD_RegenerationPerBoost",
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(RegenerationPerBoostEPE), "DoEffects")
                }
            );

            ItemFactory.CleanScriptsFromItem("SD_Stats_LuckOnFullHealth", typeof(ConditionalStats));
            ItemFactory.AddItemToDatabase( // 83
                itemName: "SD_Stats_LuckOnFullHealth",
                usesEffectDescription: false,
                usesTriggerDescription: false,
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(Stats_LuckOnFullHealthLETE), "DoEffects")
                }
            );

            ItemFactory.AddItemToDatabase( // 29 // Make effect temporary time
                itemName: "SD_GetBoostAtFullHealth",
                usesTriggerDescription: false,
                triggerType: ItemTriggerType.None,
                triggerConditions: new(),
                effects: new(),
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(GetBoostAtFullHealthLETE), "DoEffects")
                }
            );


            ItemFactory.CleanScriptsFromItem("SD_BoostPerPerfectLandingStreak", typeof(StackingLandingStats));
            ItemFactory.AddItemToDatabase( // 15
                itemName: "SD_BoostPerPerfectLandingStreak",
                //usesEffectDescription: false,
                //usesTriggerDescription: false,
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(BoostPerPerfectLandingStreakSLS), "DoTrigger")
                }
            );

            ItemFactory.AddItemToDatabase( // 68
                itemName: "SD_RandomChanceToGetLuck",
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

            ItemFactory.CleanScriptsFromItem("SD_BoostPerSpark", typeof(GainEffectPerEffect));
            ItemFactory.AddItemToDatabase( // 16
                itemName: "SD_BoostPerSpark",
                rarity: Rarity.Legendary,
                effectEvent: new List<ItemFactory.ActionDescriptor>
                {
                    new ItemFactory.ActionDescriptor(typeof(BoostPerSparkEPE), "DoEffects")
                }
            );
        }

    }
}
