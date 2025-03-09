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
        /// TargetTowers
        /// </summary>
        private int TargetTowers = 0;

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
            TargetTowers = GetTargetTowerCount();
            Tank = (ATank)UGameplayStatics.GetPlayerPawn(0);
            ToonTanksPlayerController = (AToonTanksPlayerController)UGameplayStatics.GetPlayerController(0);

            StartGame();

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
                if (ToonTanksPlayerController != null)
                {
                    ToonTanksPlayerController.SetPlayerEnabledState(false);
                }
                GameOver(false);
                return;
            }

            if (DeadActor is ATower)
            {
                ATower DestroyTower = (ATower)DeadActor;
                DestroyTower.HandleDestruction();
                --TargetTowers;
                if (TargetTowers == 0)
                {
                    GameOver(true);
                }
            }

            UTimerDynamicDelegate TimerDel = new UTimerDynamicDelegate(this, nameof(BeginPlay));
        }

        /// <summary>
        /// StartGame
        /// </summary>
        [UFunction(FunctionFlags.BlueprintEvent)]
        protected void StartGame()
        {
            LogUnrealSharp.Log("StartGame");
        }

        /// <summary>
        /// GameOver
        /// </summary>
        /// <param name="bWonGame"></param>
        [UFunction(FunctionFlags.BlueprintEvent)]
        protected void GameOver(bool bWonGame)
        {
            LogUnrealSharp.Log("GameOver");
        }

        /// <summary>
        /// Towerの件数を返却
        /// </summary>
        /// <returns></returns>
        private int GetTargetTowerCount()
        {
            IList<ATower> Towers = null;
            UGameplayStatics.GetAllActorsOfClass<ATower>(out Towers);
            return Towers.Count;
        }
    }
}
