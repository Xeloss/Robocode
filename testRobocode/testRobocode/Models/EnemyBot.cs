using System.Drawing;
using Robocode;
using Robocode.Util;
using Bot.Util;
using System;
using System.Linq;
using Bot.Bots;

namespace Bot.Models
{
    public class EnemyBot 
    {
        private ScannedRobotEvent ScannedRobot;
        private Amaterasu MyRobot;

        public EnemyBot(Amaterasu MyRobot, ScannedRobotEvent ScannedRobot)
        {
            this.Update(MyRobot, ScannedRobot);
        }

        public string Name 
        { 
            get { return this.ScannedRobot.Name; } 
        }

        public double Bearing 
        {
            get { return this.ScannedRobot.Bearing; }
        }
        public double BearingRadians
        {
            get { return ScannedRobot.BearingRadians; }
        }

        public double BearingFromGun
        {
            get 
            {
                //return Util.Utils.Bearing(this.Bearing, this.MyRobot.GunDeviation());
                var BearingFromGun = this.Bearing - this.MyRobot.GunDeviation();

                if (BearingFromGun > 180)
                    return BearingFromGun - 360;

                if (BearingFromGun < -180)
                    return BearingFromGun + 360;

                return BearingFromGun;
            }
        }

        public double BearingFromRadar 
        {
            get
            {
                //return Util.Utils.Bearing(this.Bearing, this.MyRobot.RadarDeviation());
                var BearingFromRadar = this.Bearing - this.MyRobot.RadarDeviation();

                while (BearingFromRadar > 180)
                    BearingFromRadar -= 360;

                while (BearingFromRadar < -180)
                    BearingFromRadar += 360;

                return BearingFromRadar;
            }
        }

        public double Distance 
        {
            get { return this.ScannedRobot.Distance; }
        }

        public double Energy 
        {
            get { return this.ScannedRobot.Energy; }
        }

        public double Heading
        {
            get { return this.ScannedRobot.Heading; }
        }

        public double HeadingRadians 
        {
            get { return ScannedRobot.HeadingRadians; } 
        }

        public double Velocity 
        {
            get { return this.ScannedRobot.Velocity; }
        }

        public void Update(Amaterasu MyRobot, ScannedRobotEvent ScannedRobot)
        {
            this.MyRobot = MyRobot;
            this.ScannedRobot = ScannedRobot;
        }

        public long LastUpdate { get; set; }

        public double X 
        {
            get
            {
                var absoluteBearing = this.Bearing + MyRobot.Heading;
                var radians = Robocode.Util.Utils.ToRadians(absoluteBearing);

                var dX = Math.Sin(radians) * this.Distance;

                return MyRobot.X + dX;
            }
        }
        public double Y 
        {
            get
            {
                var absoluteBearing = this.Bearing + MyRobot.Heading;
                var radians = Robocode.Util.Utils.ToRadians(absoluteBearing);

                var dY = Math.Cos(radians) * this.Distance;

                return MyRobot.Y + dY;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Name, Energy);
        }
    }
}