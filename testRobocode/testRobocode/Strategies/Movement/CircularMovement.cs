using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Bots;
using Bot.Observer;
using Bot.Strategies;
using Bot.Util;
using Robocode;

namespace Bot.Movement.Strategies
{
    public class CircularMovement : EventObserver, IMovementStrategy
    {
        private const int APROACH = -1;
        private const int MOVE_AWAY = 1;
        private const int FORWARD = 1;
        private const int BACKWARD = -1;

        private Amaterasu self;
        private Graphic Draw
        {
            get { return self.Grapher; }
        }

        private int MoveDirections = FORWARD;
        private int CloseIn = APROACH;
        private bool ShoulSwitchDirection = false;

        public CircularMovement(Amaterasu bot)
        {
            MoveDirections = FORWARD;
            CloseIn = APROACH;
            ShoulSwitchDirection = false;
            
            self = bot;
            bot.Observers.Add(this);
        }

        public void Move()
        {
            if (self.TargetEnemy.Exists())
            {
                this.Graphicate();

                if (self.TargetEnemy.Distance < Configurations.MinDistanceFromTarget)
                    CloseIn = MOVE_AWAY;
                if (self.TargetEnemy.Distance > Configurations.MaxDistanceFromTarget)
                    CloseIn = APROACH;

                this.SelectMovingDirection();

                // circle our enemy
                var facingSide = 1;
                if (self.TargetEnemy.Bearing > 0)
                    facingSide = -1;
                var turningAngle = self.TargetEnemy.Bearing + facingSide * (90 + CloseIn * (30 * MoveDirections));

                self.SetTurnRight(turningAngle);
                self.SetAhead(40 * MoveDirections);
            }
        }

        public override void OnHitByBullet(HitByBulletEvent evnt)
        {
            this.ShoulSwitchDirection = true;
        }
        public override void OnHitRobot(HitRobotEvent evnt)
        {
            this.ShoulSwitchDirection = true;
        }
        public override void OnHitWall(HitWallEvent evnt)
        {
            this.ShoulSwitchDirection = true;
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

        private void SwitchDirection()
        {
            this.MoveDirections *= -1;
            this.ShoulSwitchDirection = false;
        }

        private void SelectMovingDirection()
        {
            if (IAmNearAWall())
            {
                var heading = self.Heading;
                var backHeading = Robocode.Util.Utils.NormalAbsoluteAngleDegrees(self.Heading + 180);

                var targetAngle = this.GetAbsoluteAngleToCenter();

                if (Math.Abs(Utils.Bearing(targetAngle, heading)) < Math.Abs(Utils.Bearing(targetAngle, backHeading)))
                    this.MoveDirections = FORWARD;
                else
                    this.MoveDirections = BACKWARD;

                this.CloseIn = APROACH;

                this.ShoulSwitchDirection = false;
            }
            else if (ShoulSwitchDirection)
                this.SwitchDirection();
        }

        private double GetAbsoluteAngleToCenter()
        {
            //var significantWith = self.Width / 2;
            //var significantHeigh = self.Height / 2;

            var centerX = self.BattleFieldWidth / 2;
            var centerY = self.BattleFieldHeight / 2;

            //var targetAngle = 0;

            //if (self.X - significantWith < Configurations.MinDistanceFromWall) // Muy cerca de la pared izquierda
            //    targetAngle = 90;
            //else if (self.X + significantWith > self.BattleFieldWidth - Configurations.MinDistanceFromWall) // Muy cerca de la pared derecha
            //    targetAngle = 270;
            //else if (self.Y - significantHeigh < Configurations.MinDistanceFromWall) // Muy cerca de la pared de abajo
            //    targetAngle = 0;
            //else if (self.Y + significantHeigh > self.BattleFieldHeight - Configurations.MinDistanceFromWall) // Muy cerca de la pared de arriba
            //    targetAngle = 180;

            return Utils.AbsoluteBearing(self.X, self.Y, centerX, centerY);
        }


        private void Graphicate()
        {
            Draw.Circle(Color.GreenYellow, self.TargetEnemy.X, self.TargetEnemy.Y, self.TargetEnemy.Distance);
            Draw.Circle(Color.GreenYellow, self.TargetEnemy.X, self.TargetEnemy.Y, 5);

            Draw.Rectangle(Color.Red, Configurations.MinDistanceFromWall, Configurations.MinDistanceFromWall, self.BattleFieldWidth - Configurations.MinDistanceFromWall * 2, self.BattleFieldHeight - Configurations.MinDistanceFromWall * 2);
        }
    }
}
