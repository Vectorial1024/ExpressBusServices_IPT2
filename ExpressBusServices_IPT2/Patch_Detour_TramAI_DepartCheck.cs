﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpressBusServices;
using HarmonyLib;
using ImprovedPublicTransport2;
using ImprovedPublicTransport2.Detour.Vehicles;

namespace ExpressBusServices_IPT2
{
    [HarmonyPatch(typeof(TramAIDetour))]
    [HarmonyPatch("CanLeave", MethodType.Normal)]
    public class Patch_Detour_TramAI_DepartCheck
    {
        [HarmonyPostfix]
        public static void PostFix(ref bool __result, ushort vehicleID, ref Vehicle vehicleData)
        {
            // we just call the code on our side one more time in case our side fails
            BusPickDropLookupTable.DetermineIfBusShouldDepart(ref __result, vehicleID, ref vehicleData);
        }
    }
}
