using System;
using System.Drawing;
using Robocode;
using Robocode.Util;

namespace Bot
{
    public class RamBot : AdvancedRobot
    {
        private EnemyBot TargetEnemy { get; set; }
        private int MoveDirections = 1;

        public override void Run()
        {
            this.InitialSetup();

            while (true)
            {
                this.AimRadar();
                this.Move();
                //this.WaitFor(new RadarTurnCompleteCondition(this));
                //this.WaitFor(new MoveCompleteCondition(this));
                this.Execute();
            }
        }

        public override void OnScannedRobot(ScannedRobotEvent anScannedRobot)
        {
            if(this.ShouldUpdateTarget(anScannedRobot))
                this.TargetEnemy = new EnemyBot(this, anScannedRobot);

            if (anScannedRobot.Is(this.TargetEnemy))
            {
                this.AimGun();
                this.WaitFor(new GunTurnCompleteCondition(this));
                this.SmartFire();
            }
        }
        public override void OnRobotDeath(RobotDeathEvent aDeadRobot)
        {
            if (aDeadRobot.Is(this.TargetEnemy))
                this.TargetEnemy = null;
        }

        private void InitialSetup()
        {
            this.IsAdjustGunForRobotTurn = true;
            this.IsAdjustRadarForGunTurn = true;
            this.IsAdjustRadarForRobotTurn = true;

            this.TargetEnemy = null;
        }

        private void Move()
        {
            if (this.TargetEnemy.Exists())
            {
                // switch directions if we've stopped
                if (this.Velocity == 0)
                    MoveDirections *= -1;

                // circle our enemy
                SetTurnRight(this.TargetEnemy.Bearing + 90);
                SetAhead(40 * MoveDirections);
            }
        }
        private void SmartFire()
        {
            var firePower = 400 / this.TargetEnemy.Distance;
            firePower = Math.Min(firePower, 3);
            firePower = Math.Max(firePower, 0.1);

            this.Fire(firePower);
        }
        private void AimGun()
        {
            this.SetTurnGunRight(this.TargetEnemy.BearingFromGun);
        }
        private void AimRadar()
        {
            if (!this.TargetEnemy.Exists())
                this.SetTurnRadarRight(360);
            else
            {
                var turningDirection = Math.Sign(this.TargetEnemy.BearingFromRadar);
                var turningDistance = this.TargetEnemy.BearingFromRadar + turningDirection * 22;

                this.SetTurnRadarRight(turningDistance);
            }
        }
        
        private bool ShouldUpdateTarget(ScannedRobotEvent anScannedRobot)
        {
            if(!this.TargetEnemy.Exists())
                return true;

            if (anScannedRobot.Is(this.TargetEnemy))
                return true;

            if (anScannedRobot.Distance < TargetEnemy.Distance)
                return true;

            return false;
        }
    }
}