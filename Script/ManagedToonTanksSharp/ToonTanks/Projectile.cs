using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;
using UnrealSharp.Engine;
using UnrealSharp.Logging;

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
        /// BeginPlay
        /// </summary>
        protected override void BeginPlay()
        {
            base.BeginPlay();

            ProjectileMesh.OnComponentHit.BindUFunction(this, nameof(OnHit));
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
                return;
            }

            var MyOwnerInstigator = Owner.InstigatorController;
            var DamageTypeClass = new TSubclassOf<UDamageType>();

            if (OtherActor != null
                && OtherActor != this
                && OtherActor != MyOwner)
            {
                UGameplayStatics.ApplyDamage(OtherActor, Damage, MyOwnerInstigator, this, DamageTypeClass);
                DestroyActor();
            }
        }
    }
}
