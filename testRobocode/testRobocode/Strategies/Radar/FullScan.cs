using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Bots;

namespace Bot.Strategies.Radar
{
    public class FullScan : IRadarStrategy
    {
        public Amaterasu self;

        public FullScan(Amaterasu self)
        {
            this.self = self;
        }

        public void Scan()
        {
            if (self.RadarTurnRemaining > 0)
                return;

            self.SetTurnRadarRight(360);
        }
    }
}
