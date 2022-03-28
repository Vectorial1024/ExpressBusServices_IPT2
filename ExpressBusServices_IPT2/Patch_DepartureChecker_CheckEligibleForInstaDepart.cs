using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpressBusServices;
using HarmonyLib;
using ImprovedPublicTransport2;

namespace ExpressBusServices_IPT2
{
    [HarmonyPatch(typeof(DepartureChecker))]
    [HarmonyPatch("StopIsConsideredAsTerminus", MethodType.Normal)]
    public class Patch_DepartureChecker_CheckEligibleForInstaDepart
    {
        [HarmonyPostfix]
        public static void PostFix(ref bool __result, ushort stopID, ushort transportLineID)
        {
            if (IPT2UnbunchingRuleReader.CurrentRuleInterpretation == IPT2UnbunchingRuleReader.InterpretationMode.FIRST_PRINCIPLES)
            {
                // this is the original default.
                return;
            }
            // Determine if we are allowed to depart immediately by reading the IPT2 settings.
            __result = IPT2UnbunchingRuleReader.ReadAndInterpretIsConsideredAsTerminus(stopID, transportLineID);
            return;
        }
    }
}
