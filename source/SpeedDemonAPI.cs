using Landfall.Modding;
using Landfall.Haste;
using UnityEngine;

namespace SpeedDemon
{
    internal class SD_API
    {
        private static Fact _inRunFact = new Fact("SD_SpeedDemon_run");
        public static bool InSpeedDemonRun
        {
            get { return FactSystem.GetFact(_inRunFact) == 1f; }
            set { FactSystem.SetFact(_inRunFact, value ? 1f : 0f); }
        }

        private static Fact _StartingSpeedFact = new Fact("SD_StartingSpeed");
        public static float StartingSpeed
        {
            get { return FactSystem.GetFact(_StartingSpeedFact); }
            set { FactSystem.SetFact(_StartingSpeedFact, value); }
        }

        private static Fact _RampSpeedFact = new Fact("SD_RampSpeed");
        public static float RampSpeed
        {
            get { return FactSystem.GetFact(_RampSpeedFact); }
            set { FactSystem.SetFact(_RampSpeedFact, value); }
        }
    }
}
