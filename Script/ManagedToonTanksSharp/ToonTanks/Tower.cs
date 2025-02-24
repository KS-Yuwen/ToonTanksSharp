using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;
using UnrealSharp.Engine;
using UnrealSharp.EnhancedInput;

namespace ManagedToonTanksSharp.ToonTanks
{
    [UClass]
    public class ATower : ABasePawn
    {
        /// <summary>
        /// Tank
        /// </summary>
        private ATank Tank { get; set; }

        /// <summary>
        /// FireRange
        /// </summary>
        [UProperty(PropertyFlags.EditDefaultsOnly, Category = "Combat")]
        private float FireRange { get; set; } = 300.0f;

        /// <summary>
        /// TimerHandle
        /// </summary>
        private FTimerHandle FireRateTimerHandle;

        /// <summary>
        /// 発射間隔
        /// </summary>
        private float FireRate = 2.0f;


        /// <summary>
        /// BeginPlay
        /// </summary>
        protected override void BeginPlay()
        {
            base.BeginPlay();
            ActorTickEnabled = true;    // note 何故かTickが動作していないので、無理やり有効化

            Tank = (ATank)UGameplayStatics.GetPlayerPawn(0);

            // TimerHandle作成
            FireRateTimerHandle = SystemLibrary.SetTimer(this, nameof(CheckFireCondition), FireRate, true);//　GetWorldTimerManager().SetTimerがSystemLibrary.SetTimer
        }

        /// <summary>
        /// Tick
        /// </summary>
        /// <param name="deltaSeconds"></param>
        public override void Tick(float deltaSeconds)
        {
            base.Tick(deltaSeconds);

            if (InFireRange())
            {
                RotateTurret(Tank.ActorLocation);
            }
        }

        /// <summary>
        /// 攻撃できるかの判定
        /// </summary>
        [UFunction]
        private void CheckFireCondition()
        {
            if (InFireRange())
            {
                FInputActionValue fInputActionValue = new FInputActionValue();
                UInputAction uInputAction = new();
                Fire(fInputActionValue, 0, 0, uInputAction);
            }
        }

        /// <summary>
        ///  射程範囲かの確認
        /// </summary>
        /// <returns></returns>
        [UFunction]
        private bool InFireRange()
        {
            if (Tank)
            {
                // Tower と Tank の距離を算出
                var tankLocation = Tank.ActorLocation;
                double Distance = FVector.Distance(ActorLocation, tankLocation);
                return Distance <= FireRange;
            }

            return false;
        }

        /// <summary>
        /// HandleDestruction
        /// </summary>
        public void HandleDestruction()
        {
            base.HandleDestruction();
            DestroyActor();
        }
    }
}
