using ManagedToonTanksSharp.Manager;
using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;
using UnrealSharp.Engine;
using UnrealSharp.EnhancedInput;

namespace ManagedToonTanksSharp.ToonTanks
{
    /// <summary>
    /// Tankクラス
    /// </summary>
    [UClass]
    public class ATank : ABasePawn
    {
        /// <summary>
        /// SpringArm
        /// </summary>
        [UProperty(PropertyFlags.VisibleAnywhere | PropertyFlags.BlueprintReadOnly, Category = "Components", DefaultComponent = true, AttachmentComponent = nameof(CapsuleComp))]
        private USpringArmComponent SpringArm { get; set; }

        /// <summary>
        /// Camera
        /// </summary>
        [UProperty(PropertyFlags.VisibleAnywhere | PropertyFlags.BlueprintReadOnly, Category = "Components", DefaultComponent = true, AttachmentComponent = nameof(SpringArm))]
        private UCameraComponent Camera { get; set; }

        /// <summary>
        /// Speed
        /// </summary>
        [UProperty(PropertyFlags.EditAnywhere, Category = "Movement")]
        private float Speed { get; set; } = 200.0f;

        /// <summary>
        /// TurnRate
        /// </summary>
        [UProperty(PropertyFlags.EditAnywhere, Category = "Movement")]
        private float TurnRate { get; set; } = 200.0f;

        /// <summary>
        /// ResourceManager
        /// </summary>
        private AResourceManager resourceManager;

        /// <summary>
        /// PlayerController
        /// </summary>
        private APlayerController TankPlayerController { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ATank()
        {
            ActorTickEnabled = true;
        }

        /// <summary>
        /// BeginPlay
        /// </summary>
        protected override void BeginPlay()
        {
            base.BeginPlay();

            resourceManager = AResourceManager.Get();
            if (InputComponent is not UEnhancedInputComponent enhancedInputComponent)
            {
                throw new Exception("Player input is not configured for EnhancedInput");
            }

            if (Controller is not APlayerController playerController)
            {
                throw new Exception("Controller is not player");
            }
            TankPlayerController = playerController;

            // InputSubsystemを取得
            var enhancedInputSubsystem = GetLocalPlayerSubsystem<UEnhancedInputLocalPlayerSubsystem>(TankPlayerController);
            enhancedInputSubsystem.AddMappingContext(resourceManager.MappingContext, 0);

            // 移動処理をバインド
            enhancedInputComponent.BindAction(resourceManager.MoveAction, ETriggerEvent.Triggered, Move);
            enhancedInputComponent.BindAction(resourceManager.LookAction, ETriggerEvent.Triggered, Trun);

            enhancedInputComponent.BindAction(resourceManager.FireAction, ETriggerEvent.Started, Fire);     // 押した直後だけほしいのでStarted
        }

        /// <summary>
        /// Tick
        /// </summary>
        /// <param name="deltaSeconds"></param>
        public override void Tick(float deltaSeconds)
        {
            base.Tick(deltaSeconds);


            if (TankPlayerController)
            {
                TankPlayerController.GetHitResultUnderCursorByChannel(ETraceTypeQuery.TraceTypeQuery1, false, out FHitResult hitResult);

                RotateTurret(hitResult.ImpactPoint);
            }
        }

        /// <summary>
        /// 移動処理
        /// @note FInputActionValueでほしい値がXとYで逆になっている
        /// </summary>
        /// <param name="value"></param>
        /// <param name="elapsedTime"></param>
        /// <param name="triggeredTime"></param>
        /// <param name="sender"></param>
        [UFunction]
        private void Move(FInputActionValue value, float elapsedTime, float triggeredTime, UInputAction sender)
        {
            var AxisValue = value.GetAxis2D();
            FVector DeltaLocation = FVector.Zero;
            DeltaLocation.X = (AxisValue.Y * Speed * UGameplayStatics.WorldDeltaSeconds);
            DeltaLocation.Y = (AxisValue.X * Speed * UGameplayStatics.WorldDeltaSeconds);
            AddActorLocalOffset(DeltaLocation, false, out _, false);
        }

        /// <summary>
        /// 回転処理
        /// @note FInputActionValueでほしい値がXとYで逆になっている
        /// </summary>
        /// <param name="value"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="action"></param>
        /// <exception cref="NotImplementedException"></exception>
        [UFunction]
        private void Trun(FInputActionValue value, float arg2, float arg3, UInputAction action)
        {
            var AxisValue = value.GetAxis2D();
            FRotator DeltaRotation = FVector.Zero;
            DeltaRotation.Yaw = AxisValue.X * TurnRate * UGameplayStatics.WorldDeltaSeconds;
            AddActorLocalRotation(DeltaRotation, false, out _, false);
        }

        /// <summary>
        /// HandleDestruction
        /// </summary>
        public void HandleDestruction()
        {
            base.HandleDestruction();
            ActorHiddenInGame = true;
            ActorTickEnabled = false;
        }

        /// <summary>
        /// GetPlayerController
        /// </summary>
        /// <returns></returns>
        public APlayerController GetPlayerController()
        {
            return TankPlayerController;
        }
    }
}
