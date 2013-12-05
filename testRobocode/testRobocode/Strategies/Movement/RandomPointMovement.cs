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
    public class RandomPointMovement : IMovementStrategy
    {
        private double DestinationX = 0;
        private double DestinationY = 0;

        private Amaterasu self;

        public RandomPointMovement(Amaterasu bot)
        {
            self = bot;

            this.SetNewDestination();
        }

        public void Move()
        {
            self.Graphics.DrawLine(new Pen(Color.White), (float)DestinationX, (float)DestinationY, (float)self.X, (float)self.Y);

            var distanceToMove = Utils.Distance(self.X, self.Y, DestinationX, DestinationY);

            var angle = CalculateAngle(distanceToMove);
            angle = Utils.ToAntiClockwiseSystem(angle);

            var bearingFromHeading = Utils.Bearing(angle, self.Heading);

            self.SetTurnRight(bearingFromHeading);
            //self.WaitFor(new TurnCompleteCondition(self));
            self.SetAhead(distanceToMove);

            if (self.Velocity != 0)
                return;

            this.SetNewDestination();

            //var bearingFromHeading = angle - self.Heading;

            //while (bearingFromHeading > 180)
            //    bearingFromHeading -= 360;

            //while (bearingFromHeading < -180)
            //    bearingFromHeading += 360;
        }

        private void SetNewDestination()
        {
            var random = new Random();
            DestinationX = random.NextDouble() * self.BattleFieldWidth;
            DestinationY = random.NextDouble() * self.BattleFieldHeight;
        }
        private double CalculateAngle(double distanceToMove)
        {
            var X = DestinationX - self.X;
            var Y = DestinationY - self.Y;

            var angle = Math.Asin(X / distanceToMove);
            angle = Math.Abs(angle);
            angle = Robocode.Util.Utils.ToDegrees(angle);

            if (X > 0)
            {
                if (Y < 0)
                    angle = 360 - angle;
            }
            else
            {
                if (Y < 0)
                    angle += 180;
                else
                    angle = 180 - angle;
            }

            return angle;
        }
    }
}
