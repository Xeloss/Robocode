using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Bots;
using Bot.Strategies;
using Bot.Util;
using Robocode;

namespace Bot.Movement.Strategies
{
    public class CircularMovement : IMovementStrategy
    {
        private Amaterasu self;

        private int MoveDirections = 1;
        private int CloseIn = -1;
        private int TimeToNextDirectionChange = 20;

        public CircularMovement(Amaterasu bot)
        {
            self = bot;
        }

        public void Move()
        {
            if (self.TargetEnemy.Exists())
            {
                // switch directions if we've stopped
                if (self.Velocity == 0 || self.Time % TimeToNextDirectionChange == 0)
                    this.SwitchDirection();

                if (self.TargetEnemy.Distance < Configurations.MinDistanceFromTarget)
                    CloseIn = 1;
                if (self.TargetEnemy.Distance > Configurations.MaxDistanceFromTarget)
                    CloseIn = -1;

                // circle our enemy
                var facingSide = 1;
                if (self.TargetEnemy.Bearing > 0)
                    facingSide = -1;
                var turningAngle = self.TargetEnemy.Bearing + facingSide * (90 + CloseIn * (15 * MoveDirections));

                self.SetTurnRight(turningAngle);
                self.SetAhead(40 * MoveDirections);
            }
        }

        private void SwitchDirection()
        {
            this.MoveDirections *= -1;
            this.TimeToNextDirectionChange = 10 + new Random().Next(30);
        }
    }
}
