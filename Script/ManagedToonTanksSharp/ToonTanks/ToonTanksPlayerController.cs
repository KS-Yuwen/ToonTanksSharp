using UnrealSharp.Attributes;
using UnrealSharp.Engine;

namespace ManagedToonTanksSharp.ToonTanks
{
    /// <summary>
    /// PlayerController
    /// </summary>
    [UClass]
    public class AToonTanksPlayerController : APlayerController
    {
        /// <summary>
        /// PlayerEnabledState
        /// </summary>
        /// <param name="bPlayerEnabled"></param>
        [UFunction]
        public void SetPlayerEnabledState(bool bPlayerEnabled)
        {
            if (bPlayerEnabled)
            {
                ControlledPawn.EnableInput(this);
            }
            else
            {
                ControlledPawn.DisableInput(this);
            }
            ShowMouseCursor = bPlayerEnabled;
        }
    }
}
