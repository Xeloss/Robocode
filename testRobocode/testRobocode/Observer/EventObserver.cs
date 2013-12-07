using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace Bot.Observer
{
    public abstract class EventObserver
    {
        public virtual void OnHitByBullet(HitByBulletEvent evnt) { }
        public virtual void OnRobotDeath(RobotDeathEvent aDeadRobot) { }
        public virtual void OnScannedRobot(ScannedRobotEvent anScannedRobot) { }
        public virtual void OnHitRobot(HitRobotEvent evnt) { }
        public virtual void OnHitWall(HitWallEvent evnt) { }
    }
}
