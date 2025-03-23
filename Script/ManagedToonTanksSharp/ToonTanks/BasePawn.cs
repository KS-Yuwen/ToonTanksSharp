using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;
using UnrealSharp.Engine;
using UnrealSharp.EnhancedInput;

namespace ManagedToonTanksSharp.ToonTanks
{
    /// <summary>
    /// BasePawnクラス
    /// </summary>
    [UClass]
    public class ABasePawn : APawn
    {
        /// <summary>
        /// CapsuleComponent
        /// </summary>
        [UProperty(PropertyFlags.VisibleAnywhere | PropertyFlags.BlueprintReadOnly, Category = "Components", DefaultComponent = true, RootComponent = true)]
        protected UCapsuleComponent CapsuleComp { get; set; }

        /// <summary>
        /// BaseMesh
        /// </summary>
        [UProperty(PropertyFlags.VisibleAnywhere | PropertyFlags.BlueprintReadOnly, Category = "Components", DefaultComponent = true, AttachmentComponent = nameof(CapsuleComp))]
        private UStaticMeshComponent BaseMesh { get; set; }

        /// <summary>
        /// TurretMesh
        /// </summary>
        [UProperty(PropertyFlags.VisibleAnywhere | PropertyFlags.BlueprintReadOnly, Category = "Components", DefaultComponent = true, AttachmentComponent = nameof(BaseMesh))]
        private UStaticMeshComponent TurretMesh { get; set; }

        /// <summary>
        /// ProjectileSpwanPoint
        /// </summary>
        [UProperty(PropertyFlags.VisibleAnywhere | PropertyFlags.BlueprintReadOnly, Category = "Components", DefaultComponent = true, AttachmentComponent = nameof(TurretMesh))]
        private USceneComponent ProjectileSpwanPoint { get; set; }

        /// <summary>
        /// Projectile
        /// @note 作成したクラスをパラメータとして設定したいのでTSubclassOfで宣言
        /// </summary>
        [UProperty(PropertyFlags.EditDefaultsOnly, Category = "Combat")]
        private TSubclassOf<AProjectile> ProjectileClass { get; set; }

        /// <summary>
        /// DeathParticles
        /// </summary>
        [UProperty(PropertyFlags.EditAnywhere, Category = "Combat")]
        private UParticleSystem? DeathParticles { get; set; }

        /// <summary>
        /// DeathSound
        /// </summary>
        [UProperty(PropertyFlags.EditAnywhere, Category = "Combat")]
        private USoundBase? DeathSound { get; set; }

        /// <summary>
        /// DeathCameraShakeClass
        /// </summary>
        [UProperty(PropertyFlags.EditAnywhere, Category = "Combat")]
        private TSubclassOf<UCameraShakeBase> DeathCameraShakeClass { get; set; }

        /// <summary>
        /// タレットの回転
        /// </summary>
        /// <param name="LookAtTarget"></param>
        [UFunction]
        protected void RotateTurret(FVector LookAtTarget)
        {
            FVector ToTarget = LookAtTarget - TurretMesh.WorldLocation; // WorldLocation は GetComponentLocation
            FRotator LookAtRotation = new FRotator(0.0f, ToTarget.VectorToRotator().Yaw, 0.0f);
            TurretMesh.SetWorldRotation(LookAtRotation, false, out _, false);
        }

        /// <summary>
        /// 攻撃
        /// </summary>
        /// <param name="value"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="action"></param>
        [UFunction]
        public void Fire(FInputActionValue value, float arg2, float arg3, UInputAction action)
        {
            // 発射物を生成
            AProjectile Projectile = SpawnActor<AProjectile>(ProjectileClass,
                                                             ProjectileSpwanPoint.WorldTransform,
                                                             ESpawnActorCollisionHandlingMethod.AlwaysSpawn);
            Projectile.Owner = this;
        }

        /// <summary>
        /// HandleDestruction
        /// </summary>
        public void HandleDestruction()
        {
            // Visual/sound effect
            if (DeathParticles != null)
            {
                UGameplayStatics.SpawnEmitterAtLocation(DeathParticles, ActorLocation, ActorRotation);
            }

            if (DeathSound != null)
            {
                UGameplayStatics.PlaySoundAtLocation(DeathSound, ActorLocation, ActorRotation);
            }

            UGameplayStatics.GetPlayerController(0).PlayerCameraManager.StartCameraShake(DeathCameraShakeClass, 1.0f);
        }
    }
}
