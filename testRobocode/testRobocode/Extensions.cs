using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace Bot
{
    public static class Extensions
    {
        public static bool Exists(this object something) 
        {
            return something != null;
        }

        public static bool Is(this ScannedRobotEvent anEvent, EnemyBot aBot)
        {
            if (!aBot.Exists())
                return false;

            return aBot.Name == anEvent.Name;
        }

        public static bool Is(this RobotDeathEvent anEvent, EnemyBot aBot)
        {
            if (!aBot.Exists())
                return false;

            return aBot.Name == anEvent.Name;
        }

        public static double GunDeviation(this Robot aRobot)
        {
            var gunDeviation = aRobot.GunHeading - aRobot.Heading;

            while (gunDeviation > 180)
                gunDeviation -= 360;

            while (gunDeviation < -180)
                gunDeviation += 360;

            return gunDeviation;
        }

        public static double RadarDeviation(this Robot aRobot)
        {
            var radarDeviation = aRobot.RadarHeading - aRobot.Heading;

            while(radarDeviation > 180)
                radarDeviation -= 360;

            while(radarDeviation < -180)
                radarDeviation += 360;

            return radarDeviation;
        }
    }
}
