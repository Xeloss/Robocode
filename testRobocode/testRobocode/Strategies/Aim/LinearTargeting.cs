using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Bots;
using Bot.Models;
using Bot.Util;
using Robocode;

namespace Bot.Strategies.Aim
{
    public class LinearTargeting : IAimingStrategy
    {
        private Amaterasu self;
        private EnemyBot Enemy
        {
            get { return self.TargetEnemy; }
        }

        private Graphic Draw
        {
            get { return self.Grapher; }
        }

        public LinearTargeting(Amaterasu self)
        {
            this.self = self;
        }

        public void Aim()
        {
            if (!Enemy.Exists())
                return;

            var prediction = this.GetPredictedAngle();
            self.SetTurnGunRightRadians(prediction);

            this.Graphicate();
        }

        private double GetPredictedAngle()
        {
            double bulletPower = self.FirePower;
            double headOnBearing = self.HeadingRadians + Enemy.BearingRadians;
            double linearBearing = headOnBearing + Math.Asin(Enemy.Velocity / Rules.GetBulletSpeed(bulletPower) * Math.Sin(Enemy.HeadingRadians - headOnBearing));
            return Utils.NormalizedRelativeAngleInRadians(linearBearing - self.GunHeadingRadians);
        }
        private void Graphicate()
        {
            if (!Draw.DrawingIsEnabled)
                return;

            var dX = Math.Sin(self.GunHeadingRadians) * this.self.BattleFieldWidth;
            var dY = Math.Cos(self.GunHeadingRadians) * this.self.BattleFieldHeight;

            var X = self.X + dX;
            var Y = self.Y + dY;

            Draw.Line(Color.Yellow, self.X, self.Y, X, Y);
        }
    }
}