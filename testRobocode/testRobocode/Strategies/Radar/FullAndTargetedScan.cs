using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Bots;
using Bot.Observer;
using Bot.Util;

namespace Bot.Strategies.Radar
{
    public class FullAndTargetedScan : EventObserver, IRadarStrategy
    {
        public Amaterasu self;
        private int ScansSinceLastFullScan;
        private bool PerformFullScan;

        private IRadarStrategy fullScan;
        private IRadarStrategy targetedScan;

        public FullAndTargetedScan(Amaterasu self)
        {
            self.Observers.Add(this);

            this.self = self;
            this.ScansSinceLastFullScan = 0;
            this.PerformFullScan = false;

            this.fullScan = self.Strategies.Get<FullScan>();
            this.targetedScan = self.Strategies.Get<TargetedOscillatingScan>();
        }

        public void Scan()
        {
            if (self.RadarTurnRemaining > 0)
                return;

            if (this.ShouldPerformFullScan())
            {
                this.fullScan.Scan();
                this.ScansSinceLastFullScan = 0;
                this.PerformFullScan = false;
            }
            else
            {
                this.ScansSinceLastFullScan++;
                this.targetedScan.Scan();
            }
        }

        public override void OnRobotDeath(Robocode.RobotDeathEvent aDeadRobot)
        {
            this.PerformFullScan = true;
        }

        private bool ShouldPerformFullScan()
        {
            return !self.TargetEnemy.Exists()
                || this.PerformFullScan;
                //|| (self.Others > 1 && this.ScansSinceLastFullScan >= Configurations.ScansBeforeFullScan);
        }
    }
}
