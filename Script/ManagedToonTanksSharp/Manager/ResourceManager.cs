using UnrealSharp.Attributes;
using UnrealSharp.Engine;
using UnrealSharp.EnhancedInput;

namespace ManagedToonTanksSharp.Manager
{
    /// <summary>
    /// リソースマネージャ
    /// </summary>
    [UClass]
    public class AResourceManager : AActor
    {
        /// <summary>
        /// クラスの取得
        /// </summary>
        /// <returns></returns>
        public static AResourceManager Get()
        {
            return UGameplayStatics.GetActorOfClass<AResourceManager>();
        }

        /// <summary>
        /// InputMappingContext
        /// </summary>
        [UProperty(PropertyFlags.EditAnywhere)]
        public UInputMappingContext MappingContext { get; set; }

        /// <summary>
        /// 移動アクション
        /// </summary>
        [UProperty(PropertyFlags.EditAnywhere)]
        public UInputAction MoveAction { get; set; }

        /// <summary>
        /// 発射アクション
        /// </summary>
        [UProperty(PropertyFlags.EditAnywhere)]
        public UInputAction FireAction { get; set; }

        /// <summary>
        /// 視点移動アクション
        /// </summary>
        [UProperty(PropertyFlags.EditAnywhere)]
        public UInputAction LookAction { get; set; }
    }
}
