using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Bots;
using Bot.Util;

namespace Bot.Strategies.Radar
{
    public class TargetedOscillatingScan : IRadarStrategy
    {
        public Amaterasu self;

        public TargetedOscillatingScan(Amaterasu self)
        {
            this.self = self;
        }

        public void Scan()
        {
            if (self.RadarTurnRemaining > 0)
                return;

            var turningDirection = Math.Sign(self.TargetEnemy.BearingFromRadar);
            var turningDistance = self.TargetEnemy.BearingFromRadar + turningDirection * Configurations.MaxRadarBearingFromTarget;

            self.SetTurnRadarRight(turningDistance);
        }
    }
}
