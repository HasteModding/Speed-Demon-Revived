using System.Reflection;
using UnityEngine;

// SpecialItemEffect classes for various custom items that don't use the standard effect classes

namespace SpeedDemon.CustomItems
{
    public class GainEffectPerMissingHealth : SpecialItemEffect
    {
        public void Update()
        {
            if (selfUpdate)
            {
                DoEffects();
            }
        }

        public void DoEffects()
        {
            if (!PlayerCharacter.localPlayer)
            {
                return;
            }
            if (temporary)
            {
                if (useEffects)
                {
                    typeof(HelperFunctions).GetMethod("AddStatEffects", BindingFlags.Static | BindingFlags.NonPublic)
                        .Invoke(null, new object[] { giveType, -lastValue * amount, this });
                }
                if (useStats)
                {
                    typeof(Player).GetMethod("RemoveStats", BindingFlags.Instance | BindingFlags.NonPublic)
                        .Invoke(Player.localPlayer, new object[] { stats, lastValue });
                }
            }
            float currentHealth = (float)typeof(HelperFunctions).GetMethod("GetEffectValue", BindingFlags.Static | BindingFlags.NonPublic)
                .Invoke(null, new object[] { VariableType.Health });
            float maxHealth = (float)typeof(PlayerStat).GetMethod("GetValue", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(Player.localPlayer.stats.maxHealth, null);
            float missingHealth = maxHealth - currentHealth;
            if (useEffects)
            {
                typeof(HelperFunctions).GetMethod("AddStatEffects", BindingFlags.Static | BindingFlags.NonPublic)
                    .Invoke(null, new object[] { giveType, missingHealth * amount, this });
            }
            if (useStats)
            {
                typeof(Player).GetMethod("AddStats", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(Player.localPlayer, new object[] { stats, true, missingHealth });
            }
            lastValue = missingHealth;
        }

        private void OnDestroy()
        {
            if (temporary)
            {
                if (useEffects)
                {
                    typeof(HelperFunctions).GetMethod("AddStatEffects", BindingFlags.Static | BindingFlags.NonPublic)
                        .Invoke(null, new object[] { giveType, -lastValue * amount, this });
                }
                if (useStats)
                {
                    typeof(Player).GetMethod("RemoveStats", BindingFlags.Instance | BindingFlags.NonPublic)
                        .Invoke(Player.localPlayer, new object[] { stats, lastValue });
                }
            }
        }

        public bool selfUpdate;
        public bool temporary;
        public bool useEffects = true;
        public VariableType giveType;
        public float amount;
        private float lastValue;
        public bool useStats;
        public PlayerStats stats = null!;
    }


    public class LerpEffectToEffect : SpecialItemEffect
    {
        public void Update()
        {
            if (this.selfUpdate)
            {
                this.DoEffects();
            }
        }

        public void DoEffects()
        {
            if (!PlayerCharacter.localPlayer)
            {
                return;
            }
            if (this.temporary)
            {
                if (this.useEffects)
                {
                    typeof(HelperFunctions).GetMethod("AddStatEffects", BindingFlags.Static | BindingFlags.NonPublic)
                        .Invoke(null, new object[] { giveType, -lastValue * maxAmount, this });
                }
                if (this.useStats)
                {
                    typeof(Player).GetMethod("RemoveStats", BindingFlags.Instance | BindingFlags.NonPublic)
                        .Invoke(Player.localPlayer, new object[] { maxStats, lastValue });
                }
            }
            float checkValue = (float)typeof(HelperFunctions).GetMethod("GetEffectValue", BindingFlags.Static | BindingFlags.NonPublic)
                .Invoke(null, new object[] { checkType });
            float checkValueNorm = (checkValue - checkMin) / (checkMax - checkMin);
            if (invert) checkValueNorm = 1 - checkValueNorm;
            float effectValue = Mathf.Lerp(checkMin, checkMax, checkValueNorm);
            if (this.useEffects)
            {
                typeof(HelperFunctions).GetMethod("AddStatEffects", BindingFlags.Static | BindingFlags.NonPublic)
                    .Invoke(null, new object[] { giveType, effectValue * maxAmount, this });
            }
            if (this.useStats)
            {
                //Player.localPlayer.AddStats(this.stats, true, effectValue);
                typeof(Player).GetMethod("AddStats", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(Player.localPlayer, new object[] { maxStats, true, effectValue });
            }
            this.lastValue = effectValue;
        }

        private void OnDestroy()
        {
            if (this.temporary)
            {
                if (this.useEffects)
                {
                    typeof(HelperFunctions).GetMethod("AddStatEffects", BindingFlags.Static | BindingFlags.NonPublic)
                        .Invoke(null, new object[] { giveType, -lastValue * maxAmount, this });
                }
                if (this.useStats)
                {
                    typeof(Player).GetMethod("RemoveStats", BindingFlags.Instance | BindingFlags.NonPublic)
                        .Invoke(Player.localPlayer, new object[] { maxStats, lastValue });
                }
            }
        }

        public bool selfUpdate;
        public bool temporary;
        public VariableType checkType;
        public float checkMin = 0f;
        public float checkMax;
        public bool invert = false;
        public bool useEffects = true;
        public VariableType giveType;
        public float maxAmount;
        private float lastValue;
        public bool useStats;
        public PlayerStats maxStats = null!;
    }


    public class GainEffectPerLuck : SpecialItemEffect
    {
        public void Update()
        {
            if (this.selfUpdate)
            {
                this.DoEffects();
            }
        }

        public void DoEffects()
        {
            if (!PlayerCharacter.localPlayer)
            {
                return;
            }
            if (this.temporary)
            {
                if (useEffects)
                {
                    typeof(HelperFunctions).GetMethod("AddStatEffects", BindingFlags.Static | BindingFlags.NonPublic)
                        .Invoke(null, new object[] { giveType, -lastValue * amount, this });
                }
                if (useStats)
                {
                    typeof(Player).GetMethod("RemoveStats", BindingFlags.Instance | BindingFlags.NonPublic)
                        .Invoke(Player.localPlayer, new object[] { stats, lastValue });
                }
            }
            float effectValue = (float)typeof(PlayerStat).GetMethod("GetValue", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(Player.localPlayer.stats.luck, null);
            effectValue = effectValue - 1f; // Luck base is 1
            if (this.useEffects)
            {
                typeof(HelperFunctions).GetMethod("AddStatEffects", BindingFlags.Static | BindingFlags.NonPublic)
                    .Invoke(null, new object[] { giveType, effectValue * amount, this });
            }
            if (this.useStats)
            {
                typeof(Player).GetMethod("AddStats", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(Player.localPlayer, new object[] { stats, true, effectValue });
            }
            this.lastValue = effectValue;
        }

        private void OnDestroy()
        {
            if (this.temporary)
            {
                if (useEffects)
                {
                    typeof(HelperFunctions).GetMethod("AddStatEffects", BindingFlags.Static | BindingFlags.NonPublic)
                        .Invoke(null, new object[] { giveType, -lastValue * amount, this });
                }
                if (useStats)
                {
                    typeof(Player).GetMethod("RemoveStats", BindingFlags.Instance | BindingFlags.NonPublic)
                        .Invoke(Player.localPlayer, new object[] { stats, lastValue });
                }
            }
        }

        public bool selfUpdate;
        public bool temporary;
        public bool useEffects = true;
        public VariableType giveType;
        public float amount;
        private float lastValue;
        public bool useStats;
        public PlayerStats stats = null!;
    }

    public class DescriptionlessStatsEffect : SpecialItemEffect
    {   // Apply stats in a way that avoids the automatic effect description to get around the add/mult ambiguity bug
        // You must manually set the description in the item definition
        public void Awake()
        {
            typeof(Player).GetMethod("AddStats", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(Player.localPlayer, new object[] { stats, true, 1f });
        }

        public void OnDestroy()
        {
            typeof(Player).GetMethod("RemoveStats", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(Player.localPlayer, new object[] { stats, true, 1f });
        }

        public PlayerStats stats = null!;
    }
}
