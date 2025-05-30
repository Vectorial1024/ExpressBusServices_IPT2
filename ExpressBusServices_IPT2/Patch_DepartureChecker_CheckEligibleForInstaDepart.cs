using ExpressBusServices;
using HarmonyLib;
using JetBrains.Annotations;

namespace ExpressBusServices_IPT2
{
    [HarmonyPatch(typeof(DepartureChecker))]
    [HarmonyPatch(nameof(DepartureChecker.StopIsConsideredAsTerminus), MethodType.Normal)]
    [UsedImplicitly]
    public class Patch_DepartureChecker_CheckEligibleForInstaDepart
    {
        [HarmonyPostfix]
        [UsedImplicitly]
        public static void OverrideTerminusStatus(ref bool __result, ushort stopID, ushort transportLineID)
        {
            if (IPT2UnbunchingRuleReader.CurrentRuleInterpretation == IPT2UnbunchingRuleReader.InterpretationMode.FIRST_PRINCIPLES)
            {
                // this is the original default.
                return;
            }
            // Determine if we are allowed to depart immediately by reading the IPT2 settings.
            __result = IPT2UnbunchingRuleReader.ReadAndInterpretIsConsideredAsTerminus(stopID, transportLineID);
        }
    }
}
