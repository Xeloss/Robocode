using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Util
{
    public static class Utils
    {
        public static double ToRadians(double angle)
        {
            return Robocode.Util.Utils.ToRadians(angle);
        }

        public static double ToDegrees(double radians)
        {
            return Robocode.Util.Utils.ToDegrees(radians);
        }

        public static double NormalizedRelativeAngleInRadians(double radians)
        {
            return Robocode.Util.Utils.NormalRelativeAngle(radians);
        }

        public static double Distance(double X1, double Y1, double X2, double Y2)
        {
            return Math.Sqrt(Math.Pow(X2 - X1, 2) + Math.Pow(Y2 - Y1, 2));
        }

        public static double Bearing(double targetAngle, double currentAngle)
        {
            var bearing = targetAngle - currentAngle;

            while (bearing > 180)
                bearing -= 360;

            while (bearing < -180)
                bearing += 360;

            return bearing;
        }

        public static double ToClockwiseSystem(double anticlockwiseAngle)
        {
            return -anticlockwiseAngle + 90;
        }

        public static double ToAntiClockwiseSystem(double clockwiseAngle)
        {
            return ToClockwiseSystem(clockwiseAngle);
        }
    }
}
