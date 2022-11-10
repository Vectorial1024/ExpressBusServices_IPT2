using ColossalFramework;
using ExpressBusServices;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressBusServices_IPT2
{
    [HarmonyPatch(typeof(DepartureChecker))]
    [HarmonyPatch("RecheckUnbunchingCanLeave", MethodType.Normal)]
    public class Patch_PickDropLookup_RecheckUnbunching
    {
        [HarmonyPostfix]
        public static void PostFix(ref bool __result, ushort vehicleID, ref Vehicle vehicleData)
        {
            // IPT2: override the departure checking with a different value, because IPT2 handles unbunching differently and therefore expects a different value
            bool canLeave = Singleton<TransportManager>.instance.m_lines.m_buffer[vehicleData.m_transportLine].CanLeaveStop(vehicleData.m_targetBuilding, vehicleData.m_waitCounter);
            // todo check the main mod for more details
            __result = canLeave;
            return;
        }
    }
}
