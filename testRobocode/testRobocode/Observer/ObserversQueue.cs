using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace Bot.Observer
{
    public class ObserversQueue : List<EventObserver>
    {
        public void NotifyOnHitByBullet(HitByBulletEvent evnt)
        {
            this.ForEach(o => o.OnHitByBullet(evnt));
        }
        public void NotifyOnRobotDeath(RobotDeathEvent aDeadRobot) 
        {
            this.ForEach(o => o.OnRobotDeath(aDeadRobot));
        }
        public void NotifyOnScannedRobot(ScannedRobotEvent anScannedRobot) 
        {
            this.ForEach(o => o.OnScannedRobot(anScannedRobot));
        }
        public void NotifyOnHitRobot(HitRobotEvent evnt) 
        {
            this.ForEach(o => o.OnHitRobot(evnt));
        }
        public void NotifyOnHitWall(HitWallEvent evnt) 
        {
            this.ForEach(o => o.OnHitWall(evnt));
        }
    }
}
