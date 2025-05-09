﻿using ManagedToonTanksSharp.GameMode;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.Engine;

namespace ManagedToonTanksSharp.ToonTanks
{
    /// <summary>
    /// HealthComponent クラス
    /// @note 継承元クラスがUで始まるのでクラス名もUで始める必要がある
    /// </summary>
    [UClass(ClassFlags.CustomConstructor)]
    [BlueprintSpawnableComponent]
    public class UHealthComponent : UActorComponent
    {
        /// <summary>
        /// 最大Health
        /// </summary>
        [UProperty(PropertyFlags.EditAnywhere)]
        private float MaxHealth { get; set; } = 100.0f;

        /// <summary>
        /// 現在のHealth
        /// </summary>
        private float Health { get; set; } = 0.0f;

        private AToonTanksGameMode ToonTanksGameMode { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UHealthComponent()
        {
            ComponentTickEnabled = true;
        }

        /// <summary>
        /// BeginPlay
        /// </summary>
        public override void BeginPlay()
        {
            base.BeginPlay();
            Health = MaxHealth;

            Owner.OnTakeAnyDamage.Add(DamageTaken);
            ToonTanksGameMode = (AToonTanksGameMode)UGameplayStatics.GameMode;
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
        /// ダメージを与える
        /// </summary>
        /// <param name="DamagedActor"></param>
        /// <param name="Damage"></param>
        /// <param name="DamageType"></param>
        /// <param name="Instigator"></param>
        /// <param name="DamageCauser"></param>
        [UFunction]
        private void DamageTaken(AActor DamagedActor, float Damage, UDamageType DamageType, AController Instigator, AActor DamageCauser)
        {
            if (Damage <= 0.0f)
            {
                return;
            }

            Health -= Damage;
            if (Health <= 0.0f && ToonTanksGameMode)
            {
                ToonTanksGameMode.ActorDied(DamagedActor);
            }
        }
    }
}
