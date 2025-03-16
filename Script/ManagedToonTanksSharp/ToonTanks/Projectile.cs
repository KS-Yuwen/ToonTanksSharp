using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;
using UnrealSharp.Engine;

namespace ManagedToonTanksSharp.ToonTanks
{
    /// <summary>
    /// 発射物
    /// </summary>
    [UClass]
    class AProjectile : AActor
    {
        /// <summary>
        /// Projectile
        /// </summary>
        [UProperty(PropertyFlags.EditDefaultsOnly, Category = "Combat", DefaultComponent = true, RootComponent = true)]
        private UStaticMeshComponent ProjectileMesh { get; set; }

        /// <summary>
        /// UProjectileMovementComponent
        /// </summary>
        [UProperty(PropertyFlags.VisibleAnywhere, Category = "Movement", DefaultComponent = true)]
        private UProjectileMovementComponent? ProjectileMovement { get; set; }

        /// <summary>  
        /// Damage  
        /// </summary>  
        [UProperty(PropertyFlags.EditAnywhere)]
        private float Damage { get; set; } = 50.0f;

        /// <summary>
        /// HitParticles
        /// </summary>
        [UProperty(PropertyFlags.EditAnywhere, Category = "Combat")]
        private UParticleSystem HitParticles { get; set; }

        /// <summary>
        /// TrailParticles
        /// </summary>
        [UProperty(PropertyFlags.VisibleAnywhere, Category = "Combat", DefaultComponent = true, AttachmentComponent = nameof(ProjectileMesh))]
        private UParticleSystemComponent TrailParticles { get; set; }

        /// <summary>
        /// LanchSound
        /// </summary>
        [UProperty(PropertyFlags.EditAnywhere, Category = "Combat")]
        private USoundBase LanchSound { get; set; }

        /// <summary>
        /// HitSound
        /// </summary>
        [UProperty(PropertyFlags.EditAnywhere, Category = "Combat")]
        private USoundBase HitSound { get; set; }

        /// <summary>
        /// HitCameraShakeClass
        /// </summary>
        [UProperty(PropertyFlags.EditAnywhere, Category = "Combat")]
        private TSubclassOf<UCameraShakeBase> HitCameraShakeClass { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AProjectile()
        {
            ProjectileMovement.MaxSpeed = 1300.0f;
            ProjectileMovement.InitialSpeed = 1300.0f;
        }


        /// <summary>
        /// BeginPlay
        /// </summary>
        protected override void BeginPlay()
        {
            base.BeginPlay();

            ProjectileMesh.OnComponentHit.BindUFunction(this, nameof(OnHit));
            if (LanchSound != null)
            {
                UGameplayStatics.PlaySoundAtLocation(LanchSound, ActorLocation, ActorRotation);
            }
        }

        /// <summary>
        /// Tick
        /// </summary>
        /// <param name="deltaSeconds"></param>
        public override void Tick(float deltaSeconds)
        {
            base.Tick(deltaSeconds);
        }

        /// <summary>  
        /// Hit  
        /// </summary>  
        /// <param name="HitComp"></param>  
        /// <param name="OtherActor"></param>  
        /// <param name="OtherComp"></param>  
        /// <param name="NorlamImpulse"></param>  
        /// <param name="Hit"></param>  
        [UFunction]
        private void OnHit(UPrimitiveComponent HitComp, AActor OtherActor, UPrimitiveComponent OtherComp, FVector NorlamImpulse, FHitResult Hit)
        {
            var MyOwner = Owner;
            if (MyOwner == null)
            {
                DestroyActor();
                return;
            }

            var MyOwnerInstigator = Owner.InstigatorController;
            var DamageTypeClass = new TSubclassOf<UDamageType>();
            //var hitCameraShake = new TSubclassOf<UCameraShakeBase>(HitCameraShakeClass);

            if (OtherActor != null
                && OtherActor != this
                && OtherActor != MyOwner)
            {
                UGameplayStatics.ApplyDamage(OtherActor, Damage, MyOwnerInstigator, this, DamageTypeClass);
                if (HitParticles != null)
                {
                    UGameplayStatics.SpawnEmitterAtLocation(HitParticles, ActorLocation, ActorRotation);
                }

                if (HitSound != null)
                {
                    UGameplayStatics.PlaySoundAtLocation(HitSound, ActorLocation, ActorRotation);
                }

                //UGameplayStatics.GetPlayerController(0).PlayerCameraManager.StartCameraShake(HitCameraShakeClass, 1.0f);  // @note BP側で値を設定しているがnullになるのでコメントアウト
            }

            DestroyActor();
        }
    }
}
