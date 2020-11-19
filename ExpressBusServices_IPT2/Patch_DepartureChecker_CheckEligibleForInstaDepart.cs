using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using ImprovedPublicTransport2;

namespace ExpressBusServices_IPT2
{
    [HarmonyPatch(typeof(BusAI))]
    [HarmonyPatch("CanLeave", MethodType.Normal)]
    public class Patch_DepartureChecker_CheckEligibleForInstaDepart
    {
        [HarmonyPostfix]
        public static void PostFix(ref bool __result, ushort vehicleID, ref Vehicle vehicleData)
        {
            if (IPT2UnbunchingRuleReader.CurrentRuleInterpretation == IPT2UnbunchingRuleReader.InterpretationMode.FIRST_PRINCIPLES)
            {
                // this is the original default.
                return;
            }
            // Determine if we are allowed to depart immediately by reading the IPT2 settings.
            if (IPT2UnbunchingRuleReader.ReadAndInterpretCanInstantlyDepartNow(vehicleID, ref vehicleData))
            {
                __result = true;
            }
            return;
        }
    }
}
