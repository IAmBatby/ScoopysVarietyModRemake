using System;
using System.Collections.Generic;
using System.Text;

namespace ScoopysVarietyMod
{
    internal static class Extensions
    {
        public static double NextDouble(this Random random, double min, double max)
        {
            return (random.NextDouble() * (max - min)) + min;
        }

        public static float NextFloat(this Random random, float min, float max)
        {
            return (float)random.NextDouble(min, max);
        }
    }
}
