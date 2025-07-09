namespace SpeedDemon.CustomItems
{
    public class SpeedDemonItems
    {
        // Items that the player can actually get from rewards
        private static List<ItemInstance> rewardableItems = null!;

        private static List<string> rewardableItemNames = new()
        {
            "SD_Active_Boost",
            "SD_Active_Heal",
            "SD_Active_Slomo",
            "SD_BoostPerMissingHealth",
            "SD_BoostPerPerfectLandingStreak",
            "SD_BoostPerSpark",
            "SD_ChanceForFullHP",
            "SD_CloseCallBoost",
            "SD_CloseCallLuck",
            "SD_CooldownReductionOnPerfectLanding",
            "SD_CooldownReductionPerBoost",
            "SD_CooldownReductionWithCooldown",
            "SD_GetBoostAtFullHealth",
            "SD_GetBoostOnHeal",
            "SD_GetBoostWhenTakeDamage",
            "SD_GetBoostWithCooldown",
            "SD_LuckPerMissingHealth",
            "SD_MaxHealthButDieOnOkOrBadLanding",
            "SD_NEW_Active_StickToGround",
            "SD_NEW_BoostIfLastLandingWasPerfect",
            "SD_NEW_HealOnRunning",
            "SD_NEW_StartWithBoost",
            "SD_NEW_StartWithHeal",
            "SD_NEW_StartWithMoney",
            "SD_OnGetCoin_Boost",
            "SD_PermanentBoost",
            "SD_RandomChanceToBoostOnPerfectLanding",
            "SD_RandomChanceToRegen",
            "SD_RandomChanceToSaveNonPerfectLanding",
            "SD_Regen",
            "SD_RegenerationPerBoost",
            "SD_SparksChanceOverTime",
            "SD_SparksOverTimeDamgeOnPickUp",
            "SD_Stats_Luck",
            "SD_Stats_LuckOnFullHealth",
            "SD_Stats_MaxHealthFlat",
            "SD_Stats_MaxHealthMultiply",
            "SD_TakeDamageOnBadLandingButGetPermanentBoost",
            "SD_TakeDamageOnCloseCallButGetPermanentBoost",
            "SD_NEW_Active_TempSpeed",
            "SD_NEW_BigEffectOnSlowSpeed",
            "SD_NEW_RegenWithSpeed_Threshold",
            "SD_OnGetCoin_Invuln",
            "SD_RandomChanceToGetLuck",
            "SD_Active_Jump",
            "SD_Active_Redirection",
            "SD_Active_TradeHpForBoost",
            "SD_CloseCallCooldown",
            "SD_CloseCallEcho",
            "SD_CloseCallEnergy",
            "SD_CloseCallHeal",
            "SD_CloseCallSave",
            "SD_GetEnergyWithCooldown",
            "SD_GetHealedOnPerfectLanding",
            "SD_HealthRegenAtAlmostFullHealth",
            "SD_NEW_EnergyOnRunning",
            "SD_NEW_HealPerEnergyUsed",
            "SD_NEW_PlaceRingInFront",
            "SD_NEW_RegenIfLastLandingWasPerfect",
            "SD_NEW_SparksOnRunning",
            "SD_NEW_StartWithEnergy",
            "SD_OnGetCoin_Energy",
            "SD_OnGetCoin_Heal",
            "SD_SaveANonPerfectLandingWithCooldown",
            "SD_SparksChanceOnPerfectLanding",
            "SD_SparksOnPerfectLanding",
            "SD_SparksOverTime",
            "SD_Stats_SparkOnTakeDamage",

            "SD_EpicPermanentBoost",
            "SD_CommonCRWC",
            "SD_ReduceRefreshCost",
            "SD_LuckAndHeal",
            "SD_LuckAndBoost",
            "SD_MaxHealthAndBoost",
            "SD_SpeedOnPerfectLandingStreak",
        };
    
        public static List<ItemInstance> RewardableItems
        {
            get
            {
                if (rewardableItems == null)
                {
                    rewardableItems = new();
                    foreach (string itemName in rewardableItemNames)
                    {
                        rewardableItems.Add(ItemDatabase.instance.items.FirstOrDefault(e => e.itemName == itemName));
                    }
                }
                return rewardableItems;
            }
        }

        // Utility items for UIs and such
        private static List<ItemInstance> utilityItems = null!;

        private static List<string> utilityItemNames = new()
        {
            "SD_UI_Refresh",
            "SD_UI_StartingItemsLeft",
            "SD_UI_StartingItemsRight",
        };

        public static List<ItemInstance> UtilityItems
        {
            get
            {
                if (utilityItems == null)
                {
                    utilityItems = new();
                    foreach (string itemName in utilityItemNames)
                    {
                        utilityItems.Add(ItemDatabase.instance.items.FirstOrDefault(e => e.itemName == itemName));
                    }
                }
                return utilityItems;
            }
        }

        // Items for the quest system (coming soon)
        private static List<ItemInstance> questItems = null!;

        private static List<string> questItemNames = new()
        {

        };

        public static List<ItemInstance> QuestItems
        {
            get
            {
                if (questItems == null)
                {
                    questItems = new();
                    foreach (string itemName in questItemNames)
                    {
                        questItems.Add(ItemDatabase.instance.items.FirstOrDefault(e => e.itemName == itemName));
                    }
                }
                return questItems;
            }
        }

        // Items for the quest system (coming soon)
        private static List<ItemInstance> startingItemSetItems = null!;

        private static List<string> startingItemSetItemNames = new()
        {
            "SD_SIS_Boost",
            "SD_SIS_Luck",
            "SD_SIS_SelfDamage",
            "SD_SIS_NoItems",
        };

        public static List<ItemInstance> StartingItemSetItems
        {
            get
            {
                if (startingItemSetItems == null)
                {
                    startingItemSetItems = new();
                    foreach (string itemName in startingItemSetItemNames)
                    {
                        startingItemSetItems.Add(ItemDatabase.instance.items.FirstOrDefault(e => e.itemName == itemName));
                    }
                }
                return startingItemSetItems;
            }
        }

    }
}
