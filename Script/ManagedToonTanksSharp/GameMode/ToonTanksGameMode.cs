using ManagedToonTanksSharp.ToonTanks;
using UnrealSharp.Attributes;
using UnrealSharp.Engine;
using UnrealSharp.Logging;

namespace ManagedToonTanksSharp.GameMode
{
    /// <summary>
    /// GameMode
    /// </summary>
    [UClass]
    class AToonTanksGameMode : AGameMode
    {
        /// <summary>
        /// Tank
        /// </summary>
        private ATank Tank { get; set; }

        /// <summary>
        /// PlayerController
        /// </summary>
        private AToonTanksPlayerController ToonTanksPlayerController { get; set; }

        /// <summary>
        /// StartDelay
        /// </summary>
        private float StartDelay = 3.0f;

        /// <summary>
        /// BeginPlay
        /// </summary>
        protected override void BeginPlay()
        {
            base.BeginPlay();
            HandleGameStart();
        }

        /// <summary>
        /// HandleGameStart
        /// </summary>
        [UFunction]
        private void HandleGameStart()
        {
            Tank = (ATank)UGameplayStatics.GetPlayerPawn(0);
            ToonTanksPlayerController = (AToonTanksPlayerController)UGameplayStatics.GetPlayerController(0);
            if (ToonTanksPlayerController != null)
            {
                ToonTanksPlayerController.SetPlayerEnabledState(false);
                FTimerHandle TimerHandle = SystemLibrary.SetTimer(this, nameof(EnablePlayerInput), StartDelay, false);
            }
        }

        /// <summary>
        /// EnablePlayerInput
        /// </summary>
        [UFunction]
        private void EnablePlayerInput()
        {
            LogUnrealSharp.Log("EnablePlayerInput");
            ToonTanksPlayerController.SetPlayerEnabledState(true);
        }

        /// <summary>
        /// ActorDied
        /// </summary>
        /// <param name="DeadActor"></param>
        public void ActorDied(AActor DeadActor)
        {
            if (DeadActor == Tank)
            {
                Tank.HandleDestruction();
                ToonTanksPlayerController = (AToonTanksPlayerController)Tank.GetPlayerController();
                if (ToonTanksPlayerController != null)
                {
                    ToonTanksPlayerController.SetPlayerEnabledState(false);
                }
                return;
            }

            if (DeadActor is ATower)
            {
                ATower DestroyTower = (ATower)DeadActor;
                DestroyTower.HandleDestruction();
            }
        }
    }
}
