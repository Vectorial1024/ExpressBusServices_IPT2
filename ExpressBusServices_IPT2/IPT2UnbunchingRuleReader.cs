using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImprovedPublicTransport2;

namespace ExpressBusServices_IPT2
{
    public class IPT2UnbunchingRuleReader
    {
        public enum InterpretationMode
        {
            FIRST_PRINCIPLES,
            RESPECT_IPT2,
            INVERT_IPT2,
        }

        public static InterpretationMode CurrentRuleInterpretation { get; set; }

        public static bool ReadAndInterpretIsConsideredAsTerminus(ushort vehicleID, ref Vehicle vehicleData)
        {
            ushort currentStop = CachedVehicleData.m_cachedVehicleData[vehicleID].CurrentStop;
            if (currentStop == 0)
            {
                // we have no idea what is going on. allow them to proceed.
                return false;
            }
            bool stopIsUsingUnbunching = CachedNodeData.m_cachedNodeData[currentStop].Unbunching;
            bool lineIsUsingUnbunching = CachedTransportLineData.GetUnbunchingState(vehicleData.m_transportLine);
            switch (CurrentRuleInterpretation)
            {
                case InterpretationMode.RESPECT_IPT2:
                    // allow insta depart if 
                    // unbunching for line is disabled, or
                    // unbunching for line is active and unbunching at this stop is inactive
                    return lineIsUsingUnbunching && stopIsUsingUnbunching;
                case InterpretationMode.INVERT_IPT2:
                    // allow insta depart if
                    // unbunching for line is disabled, or
                    // unbunching for line is active and unbinching at this stop is allowed
                    // this is mainly for convenience where players need not go to every stop
                    // and flick the unbunching toggle to make it NOT unbunch at all the intermediate stops
                    return lineIsUsingUnbunching && !stopIsUsingUnbunching;
                default:
                    // dont block if somehow cannot determine if should insta depart
                    return false;
            }
        }
    }
}
