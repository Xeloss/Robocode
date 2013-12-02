using System.Drawing;
using Robocode;
using Robocode.Util;

namespace Bot
{
    public class EnemyBot 
    {
        private ScannedRobotEvent ScannedRobot;
        private Robot MyRobot; 

        public EnemyBot(Robot MyRobot, ScannedRobotEvent ScannedRobot)
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

        public double BearingFromGun
        {
            get 
            {
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

        public double Velocity 
        {
            get { return this.ScannedRobot.Velocity; }
        }

        public void Update(Robot MyRobot, ScannedRobotEvent ScannedRobot)
        {
            this.MyRobot = MyRobot;
            this.ScannedRobot = ScannedRobot;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Name, Energy);
        }
    }
}