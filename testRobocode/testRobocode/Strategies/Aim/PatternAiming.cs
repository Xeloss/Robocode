using Bot.Bots;
using Bot.Models;
using Bot.Observer;
using Bot.Util;
using Robocode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Strategies.Aim
{
    public class PatternAiming : EventObserver, IAimingStrategy
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

        internal List<EnemyMovements> EnemyMovements { get; set; }

        public PatternAiming(Amaterasu self)
        {
            this.self = self;
            this.self.Observers.Add(this);

            this.EnemyMovements = new List<EnemyMovements>();
        }

        public void Aim()
        {
            if (!Enemy.Exists())
                return;

            if (EnemyMovements != null && EnemyMovements.Count < 10)
                return;

            var enemy = EnemyMovements.FirstOrDefault();

            if (enemy != null && enemy.Enemy.Name != Enemy.Name)
            {
                EnemyMovements.Clear();
            }

            var prediction = this.GetPredictedAngle();
            
            if (prediction == -1)
            {
                self.Strategies.Get<LinearTargeting>().Aim();
            }
            else
            {
                self.SetTurnGunRightRadians(prediction);
                this.self.SetFireBullet(this.self.FirePower);
            }
            this.Graphicate();
        }

        private EnemyMovements NextEnemyMovement()
        {
            if(this.EnemyMovements.Count < 10)
                return null;

            var lastSeven = this.EnemyMovements.Skip(this.EnemyMovements.Count - Configurations.PatternLength).Take(Configurations.PatternLength).ToList();
            var otherMovements = this.EnemyMovements.Take(this.EnemyMovements.Count - Configurations.PatternLength).ToList();

            var index = this.GetPatternIndex(otherMovements, lastSeven);

            // no match
            if (index < 0)
                return null;

            return this.EnemyMovements[index];
        }

        private double GetPredictedAngle()
        {
            double bulletPower = self.FirePower;
            double headOnBearing = self.HeadingRadians + Enemy.BearingRadians;
            var nextMovement = this.NextEnemyMovement();

            if (nextMovement == null)
                return -1;
            
            double linearBearing = headOnBearing + Math.Asin(nextMovement.Velocity / Rules.GetBulletSpeed(bulletPower) * Math.Sin(Utils.ToRadians(nextMovement.Heading) - headOnBearing));
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

        private int GetPatternIndex(List<EnemyMovements> list, List<EnemyMovements> lastSeven)
        {
            var results = new List<double>();

            if (list.Count < Configurations.PatternLength)
                return -1;

            for (var i = 0; i <= list.Count - Configurations.PatternLength; i++)
                results.Add(this.GetPatternDifference(list.Skip(i).Take(Configurations.PatternLength).ToList(), lastSeven));
            
            var min = results.Min();
            var index = results.IndexOf(min);

            return Configurations.PatternLength + index + 1;
        }

        private double GetPatternDifference(List<EnemyMovements> listA, List<EnemyMovements> lastSeven)
        {
            double summatory = 0;
            for (var i = 0; i < listA.Count; i++)
            {
                summatory = Math.Abs(listA[i].Velocity - lastSeven[i].Velocity) + Math.Abs(listA[i].Heading - lastSeven[i].Heading);
            }

            return summatory;
        }


        public override void OnScannedRobot(ScannedRobotEvent anScannedRobot)
        {
            if (anScannedRobot.Is(Enemy))
            {
                EnemyMovements.Add(new EnemyMovements
                {
                    Heading = anScannedRobot.Heading,
                    Velocity = anScannedRobot.Velocity,

                    Enemy = new EnemyBot(self, anScannedRobot)
                });
            }
        }
    }
}

