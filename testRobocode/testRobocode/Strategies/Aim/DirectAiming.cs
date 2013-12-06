using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Bots;
using Bot.Models;
using Bot.Util;

namespace Bot.Strategies.Aim
{
    public class DirectAiming : IAimingStrategy
    {
        private Amaterasu self;
        private EnemyBot Enemy 
        {
            get { return self.TargetEnemy; }
        }

        public DirectAiming(Amaterasu self)
        {
            this.self = self;
        }

        public void Aim()
        {
            if (!Enemy.Exists())
                return;

            self.SetTurnGunRight(Enemy.BearingFromGun);
        }
    }
}
