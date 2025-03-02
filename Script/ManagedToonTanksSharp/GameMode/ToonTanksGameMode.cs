using ManagedToonTanksSharp.ToonTanks;
using UnrealSharp.Attributes;
using UnrealSharp.Engine;

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
        /// BeginPlay
        /// </summary>
        protected override void BeginPlay()
        {
            base.BeginPlay();
            Tank = (ATank)UGameplayStatics.GetPlayerPawn(0);
            ToonTanksPlayerController = (AToonTanksPlayerController)UGameplayStatics.GetPlayerController(0);
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
