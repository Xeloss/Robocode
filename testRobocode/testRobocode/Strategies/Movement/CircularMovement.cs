using System;
using System.Collections.Generic;
using System.Drawing;
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
        private Graphic Draw
        {
            get { return self.Grapher; }
        }

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
                this.Graphicate();

                //// switch directions if we've stopped
                //if (self.Velocity == 0 || self.Time % TimeToNextDirectionChange == 0)
                //    this.SwitchDirection();

                this.SelectMovingDirection();

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

        private bool IAmNearAWall()
        {
            var significantWith = self.Width / 2;
            var significantHeigh = self.Height / 2;

            return self.X - significantWith < Configurations.MinDistanceFromWall // Muy cerca de la pared izquierda
                || self.X + significantWith > self.BattleFieldWidth - Configurations.MinDistanceFromWall // Muy cerca de la pared derecha
                || self.Y - significantHeigh < Configurations.MinDistanceFromWall // Muy cerca de la pared de abajo
                || self.Y + significantHeigh > self.BattleFieldHeight - Configurations.MinDistanceFromWall; // Muy cerca de la pared de arriba
        }

        private void Graphicate()
        {
            Draw.Circle(Color.GreenYellow, self.TargetEnemy.X, self.TargetEnemy.Y, self.TargetEnemy.Distance);
            Draw.Circle(Color.GreenYellow, self.TargetEnemy.X, self.TargetEnemy.Y, 5);
        }
        private void SwitchDirection()
        {
            this.MoveDirections *= -1;
            this.TimeToNextDirectionChange = 10 + new Random().Next(30);
        }

        public void SelectMovingDirection()
        {
            if (self.Velocity == 0)
                this.SwitchDirection();
            else if (IAmNearAWall())
            {
                var significantWith = self.Width / 2;
                var significantHeigh = self.Height / 2;

                var heading = self.Heading;
                var backHeading = Robocode.Util.Utils.NormalAbsoluteAngleDegrees(self.Heading + 180);

                var targetAngle = 0;

                if (self.X - significantWith < Configurations.MinDistanceFromWall) // Muy cerca de la pared izquierda
                    targetAngle = 90;
                else if (self.X + significantWith > self.BattleFieldWidth - Configurations.MinDistanceFromWall) // Muy cerca de la pared derecha
                    targetAngle = 270;
                else if (self.Y - significantHeigh < Configurations.MinDistanceFromWall) // Muy cerca de la pared de abajo
                    targetAngle = 0;
                else if (self.Y + significantHeigh > self.BattleFieldHeight - Configurations.MinDistanceFromWall) // Muy cerca de la pared de arriba
                    targetAngle = 180;

                if (Math.Abs(targetAngle - heading) < Math.Abs(targetAngle - backHeading))
                    this.MoveDirections = 1;
                else
                    this.MoveDirections = -1;
            }
        }
    }
}
