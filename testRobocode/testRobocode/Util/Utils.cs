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

        public static double AbsoluteBearing(double X1, double Y1, double X2, double Y2)
        {
            double xo = X2 - X1;
            double yo = Y2 - Y1;
            double hyp = Utils.Distance(X1, Y1, X2, Y2);
            double arcSin = Robocode.Util.Utils.ToDegrees(Math.Asin(xo / hyp));
            double bearing = 0;

            if (xo > 0 && yo > 0)
            { // both pos: lower-Left
                bearing = arcSin;
            }
            else if (xo < 0 && yo > 0)
            { // x neg, y pos: lower-right
                bearing = 360 + arcSin; // arcsin is negative here, actuall 360 - ang
            }
            else if (xo > 0 && yo < 0)
            { // x pos, y neg: upper-left
                bearing = 180 - arcSin;
            }
            else if (xo < 0 && yo < 0)
            { // both neg: upper-right
                bearing = 180 - arcSin; // arcsin is negative here, actually 180 + ang
            }

            return bearing;
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
