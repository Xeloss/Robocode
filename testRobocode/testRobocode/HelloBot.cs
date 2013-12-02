using System.Drawing;
using Robocode;
using Robocode.Util;

namespace Bot
{
    /// <summary>
    ///   Corners - a sample robot by Mathew Nelson, and maintained by Flemming N. Larsen
    ///   <p />
    ///   This robot moves to a corner, then swings the gun back and forth.
    ///   If it dies, it tries a new corner in the next round.
    /// </summary>
    public class HelloBot : Robot
    {
        private static int corner; // Which corner we are currently using
        // static so that it keeps it between rounds.
        private bool stopWhenSeeRobot = true; // See goCorner()

        /// <summary>
        ///   run:  Corners' main run function.
        /// </summary>
        public override void Run()
        {
            // Move to a corner
            //goCorner();

            // Initialize gun turn speed to 3
            int gunIncrement = 6;

            // Spin gun back and forth
            while (true)
            {
                for (int i = 0; i < 30; i++)
                {
                    TurnGunLeft(gunIncrement);
                }
                gunIncrement *= -1;
            }
        }

        /// <summary>
        ///   goCorner:  A very inefficient way to get to a corner.  Can you do better?
        /// </summary>
        public void goCorner()
        {
            // We don't want to stop when we're just turning...
            stopWhenSeeRobot = false;
            // turn to face the wall to the "right" of our desired corner.
            TurnRight(Utils.NormalRelativeAngleDegrees(corner - Heading));
            // Ok, now we don't want to crash into any robot in our way...
            stopWhenSeeRobot = true;
            // Move to that wall
            Ahead(5000);
            // Turn to face the corner
            TurnLeft(90);
            // Move to the corner
            Ahead(5000);
            // Turn gun to starting point
            TurnGunLeft(90);
        }

        /// <summary>
        ///   onScannedRobot:  Stop and Fire!
        /// </summary>
        public override void OnScannedRobot(ScannedRobotEvent e)
        {
            // Should we stop, or just Fire?
            if (stopWhenSeeRobot)
            {
                // Stop everything!  You can safely call stop multiple times.
                Stop();
                // Call our custom firing method
                smartFire(e.Distance);
                // Look for another robot.
                // NOTE:  If you call scan() inside onScannedRobot, and it sees a robot,
                // the game will interrupt the event handler and start it over
                Scan();
                // We won't get here if we saw another robot.
                // Okay, we didn't see another robot... start moving or turning again.
                Resume();
            }
            else
            {
                smartFire(e.Distance);
            }
        }

        /// <summary>
        ///   smartFire:  Custom Fire method that determines firepower based on distance.
        /// </summary>
        /// <param name="robotDistance">
        ///   the distance to the robot to Fire at
        /// </param>
        public void smartFire(double robotDistance)
        {
            if (robotDistance > 200 || Energy < 15)
            {
                Fire(1);
            }
            else if (robotDistance > 50)
            {
                Fire(2);
            }
            else
            {
                Fire(3);
            }
        }
    }
}