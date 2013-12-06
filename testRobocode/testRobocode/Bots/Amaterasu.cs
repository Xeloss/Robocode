using System;
using System.Drawing;
using Bot.Models;
using Robocode;
using Robocode.Util;
using Bot.Util;
using Bot.Movement.Strategies;
using Bot.Strategies;
using Bot.Strategies.Radar;
using Bot.Strategies.Aim;

namespace Bot.Bots
{
    public class Amaterasu : AdvancedRobot
    {
        private IMovementStrategy MovementStrategy;
        private IRadarStrategy RadarStrategy;
        private IAimingStrategy AimingStrategy;

        internal Graphic Grapher { get; private set; }
        internal EnemyBot TargetEnemy { get; private set; }
        internal StrategiesFactory Strategies { get; private set; }

        public override void Run()
        {
            this.InitialSetup();
            while (true)
            {
                if (this.ShouldCleanTarget())
                    this.TargetEnemy = null;

                RadarStrategy.Scan();
                MovementStrategy.Move();
                this.Execute();

                this.Graficate();
            }
        }

        public override void OnScannedRobot(ScannedRobotEvent anScannedRobot)
        {
            if (this.ShouldUpdateTarget(anScannedRobot))
                this.UpdateTargetTo(anScannedRobot);

            if (anScannedRobot.Is(this.TargetEnemy))
            {
                if (this.ShouldReAimRadar())
                    this.RadarStrategy.Scan();

                this.AimingStrategy.Aim();
                this.SmartFire();
            }
        }
        public override void OnRobotDeath(RobotDeathEvent aDeadRobot)
        {
            if (aDeadRobot.Is(this.TargetEnemy))
            {
                this.TargetEnemy = null;
                this.MovementStrategy = this.Strategies.Get<RandomPointMovement>();
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
            this.Grapher = new Graphic(this);

            #if !DEBUG
            this.Grapher.DrawingIsEnabled = false;
            #endif

            this.CalculateMaxDistanceFromTarget();
            this.Strategies = new StrategiesFactory(this);

            this.MovementStrategy = this.Strategies.Get<RandomPointMovement>();
            this.RadarStrategy = this.Strategies.Get<FullAndTargetedScan>();
            this.AimingStrategy = this.Strategies.Get<DirectAiming>();

        }
        
        private void SmartFire()
        {
            if (!this.TargetEnemy.Exists())
                return;

            var firePower = Configurations.DamageCoefficient / this.TargetEnemy.Distance;
            firePower = Math.Min(firePower, Configurations.MaxFirePower);
            firePower = Math.Max(firePower, Configurations.MinFirePower);

            this.SetFire(firePower);
        }

        private void UpdateTargetTo(ScannedRobotEvent anScannedRobot)
        {
            this.TargetEnemy = new EnemyBot(this, anScannedRobot) { LastUpdate = this.Time };
            this.MovementStrategy = this.Strategies.Get<CircularMovement>();
        }

        private bool ShouldReAimRadar()
        {
            return this.TargetEnemy.BearingFromRadar < Configurations.MaxRadarBearingFromTarget / 2
                && this.RadarTurnRemaining == 0;
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
        private void Graficate()
        {
            if (this.TargetEnemy.Exists())
            {
                Grapher.Rectangle(Color.Red, this.TargetEnemy.X - 20, this.TargetEnemy.Y - 20, 40, 40);
            }
        }
    }
}