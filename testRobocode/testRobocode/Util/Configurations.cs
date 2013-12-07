using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace Bot.Util
{
    internal static class Configurations
    {
        public static double MinDistanceFromWall
        {
            get { return 50; }
        }

        public static double MaxFirePower 
        { 
            get { return Rules.MAX_BULLET_POWER; } 
        }
        public static double MinFirePower
        {
            get { return Rules.MIN_BULLET_POWER; }
        }

        public static double MaxRadarBearingFromTarget
        {
            get { return (Rules.RADAR_TURN_RATE - 1) / 2; }
        }

        public static double MaxScanDistance
        {
            get { return Rules.RADAR_SCAN_RADIUS; }
        }
        public static int ScansBeforeFullScan
        {
            get { return 15; }
        }

        public static int MaxDistanceFromTarget 
        {
            get;
            set;
        }
        public static int MinDistanceFromTarget
        {
            get { return 200; }
        }

        public static int TicksToCleanTarget
        {
            get { return 10; }
        }

        public static int DamageCoefficient
        {
            get { return (MinDistanceFromTarget + 50) * 3; }
        }
    }
}
