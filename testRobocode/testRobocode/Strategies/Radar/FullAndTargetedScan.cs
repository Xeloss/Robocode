using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Bots;
using Bot.Util;

namespace Bot.Strategies.Radar
{
    public class FullAndTargetedScan : IRadarStrategy
    {
        public Amaterasu self;
        private int ScansSinceLastFullScan;
        private IRadarStrategy fullScan;
        private IRadarStrategy targetedScan;

        public FullAndTargetedScan(Amaterasu self)
        {
            this.self = self;
            this.ScansSinceLastFullScan = 0;
            this.fullScan = new FullScan(self);
            this.targetedScan = new TargetedOscillatingScan(self);
        }

        public void Scan()
        {
            if (self.RadarTurnRemaining > 0)
                return;

            if (this.ShouldPerformFullScan())
            {
                this.fullScan.Scan();
                this.ScansSinceLastFullScan = 0;
            }
            else
            {
                this.ScansSinceLastFullScan++;
                this.targetedScan.Scan();
            }
        }

        private bool ShouldPerformFullScan()
        {
            return !self.TargetEnemy.Exists()
                || (self.Others > 1 && this.ScansSinceLastFullScan >= Configurations.ScansBeforeFullScan);
        }
    }
}
