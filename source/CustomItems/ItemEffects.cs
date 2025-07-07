using UnityEngine;
using Landfall.Haste;
using UnityEngine.Localization;
using CustomItemLib;

namespace SpeedDemon.CustomItems
{

    public class CooldownReductionPerBoostEPE : GainEffectPerEffect
    {
        public void Configure()
        {
            Debug.Log("[ModDebug] Configured CooldownReductionPerBoostEPE");
            effectDescription = new LocalizedString("SpeedDemonItems", "CooldownReductionPerBoostEPE_description");
            checkType = VariableType.Boost;
            giveType = VariableType.Cooldown;
            amount = -0.1f;
        }
    }

    public class RegenerationPerBoostEPE : GainEffectPerEffect
    {
        public void Configure()
        {
            Debug.Log("[ModDebug] Configured RegenerationPerBoostEPE");
            effectDescription = new LocalizedString("SpeedDemonItems", "RegenerationPerBoostEPE_description");
            checkType = VariableType.Boost;
            giveType = VariableType.Health;
            amount = 2f;
        }
    }

    public class BoostPerSparkEPE : GainEffectPerEffect
    {
        public void Configure()
        {
            Debug.Log("[ModDebug] Configured BoostPerSparkEPE");
            effectDescription = new LocalizedString("SpeedDemonItems", "BoostPerSparkEPE_description");
            checkType = VariableType.Resource;
            giveType = VariableType.Boost;
            amount = 0.0003f;
            selfUpdate = true;
            temporary = true;
        }
    }

    public class RandomChanceToGetLuckEPL : GainEffectPerLuck
    {
        public void Configure()
        {
            Debug.Log("[ModDebug] Configured RandomChanceToGetLuckEPE");
            effectDescription = new LocalizedString("SpeedDemonItems", "RandomChanceToGetLuckEPL_description");
            giveType = VariableType.Resource;
            amount = 100f;
        }
    }

    public class NEW_BoostIfLastLandingWasPerfectEILLP : Item_EffectIfLastLandingPerfect
    {
        public void Configure()
        {
            Debug.Log("[ModDebug] Configured NEW_BoostIfLastLandingWasPerfectEILLP");
            effectDescription = new LocalizedString("SpeedDemonItems", "NEW_BoostIfLastLandingWasPerfectEILLP_description");
            triggerTimer = 0.1f;
            effects = new List<ItemEffect>
            {
                new AddVariable_TemporaryEffect
                {
                    variableType = VariableType.Boost,
                    amount = 0.5f,
                    duration = 0.1f
                }
            };
        }
    }

    public class SD_SpeedOnPerfectLandingStreakEILLP : SpecialItemEffect
    {
        public void Configure()
        {
            Debug.Log("[ModDebug] Configured SD_SpeedOnPerfectLandingStreakEILLP");
            effectDescription = new LocalizedString("SpeedDemonItems", "SD_SpeedOnPerfectLandingStreakEILLP_description");
            effects = new List<ItemEffect>
            {
                new AddVariable_Effect
                {
                    variableType = VariableType.Velocity,
                    amount = 10f
                }
            };
        }

        public void Trigger()
        {
            if (!Player.localPlayer)
            {
                return;
            }
            if (PlayerCharacter.localPlayer.data.landingThisFrame == LandingType.Perfect && lastLanding == LandingType.Perfect)
            {
                for (int i = 0; i < effects.Count; i++)
                {
                    effects[i].TriggerEffect(Player.localPlayer, this);
                }
            }
            lastLanding = PlayerCharacter.localPlayer.data.landingThisFrame;
        }

        [SerializeReference]
        public List<ItemEffect> effects = new List<ItemEffect>();
        private LandingType lastLanding;
    }


    public class BoostPerMissingHealthEPMH : GainEffectPerMissingHealth
    {
        public void Configure()
        {
            Debug.Log("[ModDebug] Configured BoostPerMissingHealthEPMH");
            effectDescription = new LocalizedString("SpeedDemonItems", "BoostPerMissingHealthEPMH_description");
            selfUpdate = true;
            temporary = true;
            giveType = VariableType.Boost;
            amount = 0.01f;
        }
    }

    public class LuckPerMissingHealthEPMH : GainEffectPerMissingHealth
    {
        public void Configure()
        {
            Debug.Log("[ModDebug] Configured LuckPerMissingHealthEPMH");
            effectDescription = new LocalizedString("SpeedDemonItems", "LuckPerMissingHealthEPMH_description");
            selfUpdate = true;
            temporary = true;
            useEffects = false;
            useStats = true;
            stats = ItemFactory.CreatePlayerStats(luck: new PlayerStat { baseValue = 0.015f });
        }
    }

    public class Stats_LuckOnFullHealthLETE : LerpEffectToEffect
    {
        public void Configure()
        {
            Debug.Log("[ModDebug] Configured Stats_LuckOnFullHealthLETE");
            effectDescription = new LocalizedString("SpeedDemonItems", "Stats_LuckOnFullHealthLETE_description");
            selfUpdate = true;
            temporary = true;
            useEffects = false;
            useStats = true;
            checkType = VariableType.HealthPercentage;
            checkMax = 1f;
            maxStats = ItemFactory.CreatePlayerStats(luck: new PlayerStat { baseValue = 0.7f });
        }
    }

    public class GetBoostAtFullHealthLETE : LerpEffectToEffect
    {
        public void Configure()
        {
            Debug.Log("[ModDebug] Configured GetBoostAtFullHealthLETE");
            effectDescription = new LocalizedString("SpeedDemonItems", "GetBoostAtFullHealthLETE_description");
            selfUpdate = true;
            temporary = true;
            checkType = VariableType.HealthPercentage;
            checkMax = 1f;
            giveType = VariableType.Boost;
            maxAmount = 0.4f;
        }
    }


    public class BoostPerPerfectLandingStreakSLS : StackingLandingStats
    {
        public void Configure()
        {
            Debug.Log("[ModDebug] Configured BoostPerPerfectLandingStreakSLS");
            effectDescription = new LocalizedString("SpeedDemonItems", "BoostPerPerfectLandingStreakSLS_description");
            variableType = VariableType.Boost;
            amount = 0.05f;
        }
    }

    // This effect is kind of cursed. Items get destroyed and recreated every scene, so we don't actually have any way of using
    // the item's effect to reduce the cost of the refresh. The way we're doing it, is attaching a dummy script to the item and
    // when the player goes through the portal, we count the number of those items in the player's inventory.
    public class SD_ReduceRefreshCostEffect : SpecialItemEffect
    {
        public void Configure()
        {
            Debug.Log("[ModDebug] Configured SD_ReduceRefreshCostEffect");
            effectDescription = new LocalizedString("SpeedDemonItems", "SD_ReduceRefreshCostEffect_description");
        }

        public void DoNothing()
        {
            // Dummy method
        }
    }

    public class SD_EpicPermanentBoostDSE : DescriptionlessStatsEffect
    {
        public void Configure()
        {
            Debug.Log("[ModDebug] Configured SD_EpicPermanentBoostDSE");
            effectDescription = new LocalizedString("SpeedDemonItems", "SD_EpicPermanentBoostDSE_description");
            stats = ItemFactory.CreatePlayerStats(boost: new PlayerStat { multiplier = 1.25f }, damageMultiplier: new PlayerStat { multiplier = 1.1f });
        }

        public void DoNothing()
        {
            // Dummy method
        }
    }
}
