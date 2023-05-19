using ExpressBusServices;
using HarmonyLib;
using ImprovedPublicTransport2.HarmonyPatches.XYZVehicleAIPatches;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpressBusServices_IPT2
{
    [HarmonyPatch(typeof(CanLeavePatch))]
    [HarmonyPatch("Postfix")]
    public class Patch_CanLeavePatch_PatchPostFix
    {
        public static ItemClass.SubService[] managedTransportTypes = {
            ItemClass.SubService.PublicTransportBus,
            ItemClass.SubService.PublicTransportTrolleybus,
            ItemClass.SubService.PublicTransportTram
        };

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> TranspileIpt2(IEnumerable<CodeInstruction> originalMethod)
        {
            // change of method; we will just use transpilers to inject our code at the top of the IPT2 method
            // note: since we are just appending extra code before the execution of the original method, it all becomes easy.

            // emit: TouchCanLeaveFlag(vehicleID, ref __result)
            yield return new CodeInstruction(OpCodes.Ldarg_0);
            yield return new CodeInstruction(OpCodes.Ldarg_1);
            yield return new CodeInstruction(OpCodes.Call, typeof(Patch_CanLeavePatch_PatchPostFix).GetMethod("TouchCanLeaveFlag"));

            // allow original method to continue
            foreach (CodeInstruction instruction in originalMethod)
            {
                yield return instruction;
            }
            yield break;
        }

        public static void TouchCanLeaveFlag(ushort vehicleID, ref bool ipt2CanLeave)
        {
            // check the transport type: in latest verison of IPT2, they unified all transport type to use the same method
            ref Vehicle vehicleData = ref VehicleManager.instance.m_vehicles.m_buffer[vehicleID];
            if (vehicleData.Info?.m_class?.m_service != ItemClass.Service.PublicTransport)
            {
                // nope
                return;
            }
            // is public transport
            ItemClass.SubService? vehicleTransportType = vehicleData.Info?.GetSubService();
            if (vehicleTransportType == null || !managedTransportTypes.Contains(vehicleTransportType.Value))
            {
                // nope
                return;
            }
            // we just call the code on our side one more time in case our side fails
            BusPickDropLookupTable.DetermineIfBusShouldDepart(ref ipt2CanLeave, vehicleID, ref vehicleData);
            return;
        }
    }
}
