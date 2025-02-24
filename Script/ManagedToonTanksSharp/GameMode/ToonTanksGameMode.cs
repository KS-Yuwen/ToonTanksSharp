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

        private ATank Tank { get; set; }

        private ATower Tower { get; set; }

        /// <summary>
        /// BeginPlay
        /// </summary>
        protected override void BeginPlay()
        {
            base.BeginPlay();
            Tank = (ATank)UGameplayStatics.GetPlayerPawn(0);
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
                APlayerController playerController = Tank.GetPlayerController();
                if (playerController != null)
                {
                    Tank.DisableInput(playerController);
                    playerController.ShowMouseCursor = false;
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
