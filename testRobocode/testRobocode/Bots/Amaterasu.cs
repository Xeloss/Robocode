using System;
using System.Drawing;
using Bot.Models;
using Robocode;
using Robocode.Util;
using Bot.Util;
using Bot.Movement.Strategies;
using Bot.Strategies;

namespace Bot.Bots
{
    public class Amaterasu : AdvancedRobot
    {
        private IMovementStrategy MovementStrategy;

        public EnemyBot TargetEnemy { get; set; }
        
        private int ScansSinceLastFullScan = 0;

        public override void Run()
        {
            this.InitialSetup();
            while (true)
            {
                if (this.ShouldCleanTarget())
                    this.TargetEnemy = null;

                this.AimRadar();
                MovementStrategy.Move();
                this.Execute();
            }
        }

        public override void OnScannedRobot(ScannedRobotEvent anScannedRobot)
        {
            if (this.ShouldUpdateTarget(anScannedRobot))
                this.UpdateTargetTo(anScannedRobot);

            if (anScannedRobot.Is(this.TargetEnemy))
            {
                if (this.ShouldAimRadar())
                    this.AimRadar();

                this.AimGun();
                this.WaitFor(new GunTurnCompleteCondition(this));
                this.SmartFire();
            }
        }
        public override void OnRobotDeath(RobotDeathEvent aDeadRobot)
        {
            if (aDeadRobot.Is(this.TargetEnemy))
            {
                this.TargetEnemy = null;
                this.MovementStrategy = new RandomPointMovement(this);
            }
        }

        private void InitialSetup()
        {
            this.IsAdjustGunForRobotTurn = true;
            this.IsAdjustRadarForGunTurn = true;
            this.IsAdjustRadarForRobotTurn = true;

            this.BodyColor = Color.White;
            this.GunColor = Color.OrangeRed;
            this.RadarColor = Color.FromArgb(39, 105, 66);
            this.ScanColor = Color.Orange;
            //this.BulletColor = Color.Green;

            this.TargetEnemy = null;
            this.ScansSinceLastFullScan = 0;

            this.CalculateMaxDistanceFromTarget();

            this.MovementStrategy = new RandomPointMovement(this);
        }
        
        private void SmartFire()
        {
            if (!this.TargetEnemy.Exists())
                return;

            var firePower = Configurations.DamageCoefficient / this.TargetEnemy.Distance;
            firePower = Math.Min(firePower, Configurations.MaxFirePower);
            firePower = Math.Max(firePower, Configurations.MinFirePower);

            this.Fire(firePower);
        }
        private void AimGun()
        {
            if (!this.TargetEnemy.Exists())
                return;

            this.SetTurnGunRight(this.TargetEnemy.BearingFromGun);
        }
        private void AimRadar()
        {
            if (this.RadarTurnRemaining > 0)
                return;

            if (this.ShouldPerformFullScan())
            {
                this.SetTurnRadarRight(360);
                this.ScansSinceLastFullScan = 0;
            }
            else
            {
                this.ScansSinceLastFullScan++;

                var turningDirection = Math.Sign(this.TargetEnemy.BearingFromRadar);
                var turningDistance = this.TargetEnemy.BearingFromRadar + turningDirection * Configurations.MaxRadarBearingFromTarget;

                this.SetTurnRadarRight(turningDistance);
            }
        }

        private void UpdateTargetTo(ScannedRobotEvent anScannedRobot)
        {
            this.TargetEnemy = new EnemyBot(this, anScannedRobot) { LastUpdate = this.Time };
            this.MovementStrategy = new CircularMovement(this);
        }

        private bool ShouldAimRadar()
        {
            return this.TargetEnemy.BearingFromRadar < Configurations.MaxRadarBearingFromTarget / 2
                && this.RadarTurnRemaining == 0;
        }
        private bool ShouldPerformFullScan()
        {
            return !this.TargetEnemy.Exists()
                || (this.Others > 1 && this.ScansSinceLastFullScan >= Configurations.ScansBeforeFullScan);
        }
        private bool ShouldUpdateTarget(ScannedRobotEvent anScannedRobot)
        {
            if(!this.TargetEnemy.Exists())
                return true;

            if (anScannedRobot.Is(this.TargetEnemy))
                return true;

            if (anScannedRobot.Distance < this.TargetEnemy.Distance)
                return true;

            return false;
        }
        private bool ShouldCleanTarget()
        {
            return this.TargetEnemy.Exists()
                && (this.Time - this.TargetEnemy.LastUpdate) > Configurations.TicksToCleanTarget;
        }

        private void CalculateMaxDistanceFromTarget()
        {
            var minBattleFieldDimension = Math.Min(this.BattleFieldWidth, this.BattleFieldHeight);

            Configurations.MaxDistanceFromTarget = Convert.ToInt32(Math.Min(minBattleFieldDimension, Configurations.MaxScanDistance) / 2);
        }
    }
}