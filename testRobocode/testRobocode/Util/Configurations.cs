using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace Bot.Util
{
    public static class Configurations
    {
        public const double MaxFirePower = 3;
        public const double MinFirePower = 0.1;

        public const byte MaxRadarBearingFromTarget = 22;
        
        public const int MaxScanDistance = 1200;
        public const int ScansBeforeFullScan = 10;

        public static int MaxDistanceFromTarget 
        {
            get;
            set;
        }
        public const int MinDistanceFromTarget = 150;

        public const int TicksToCleanTarget = 5;

        public const int DamageCoefficient = (MinDistanceFromTarget + 50) * 3;
    }
}
