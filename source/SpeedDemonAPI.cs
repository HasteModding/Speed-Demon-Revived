using Landfall.Modding;
using Landfall.Haste;
using UnityEngine;

namespace SpeedDemon
{
    internal class SD_API
    {
        private static Fact _inRunFact = new Fact("in_SpeedDemon_run");
        public static bool InSpeedDemonRun
        {
            get { return FactSystem.GetFact(_inRunFact) == 1f; }
            set { FactSystem.SetFact(_inRunFact, value ? 1f : 0f); }
        }
    }
}
