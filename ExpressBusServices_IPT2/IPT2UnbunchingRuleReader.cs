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

        public static bool ReadAndInterpretIsConsideredAsTerminus(ushort stopID, ushort transportLineID)
        {
            // if it is a terminus, then we unbunch
            // if we want to unbunch, then interpret it as a terminus
            if (stopID == 0)
            {
                // we have no idea what is going on. allow them to proceed.
                return false;
            }
            bool stopIsUsingUnbunching = CachedNodeData.m_cachedNodeData[stopID].Unbunching;
            bool lineIsUsingUnbunching = CachedTransportLineData.GetUnbunchingState(transportLineID);
            switch (CurrentRuleInterpretation)
            {
                case InterpretationMode.RESPECT_IPT2:
                    // it is a terminus if:
                    // the line uses unbunching (seems to be true for all types); and
                    // the stop uses unbunching
                    return lineIsUsingUnbunching && stopIsUsingUnbunching;
                case InterpretationMode.INVERT_IPT2:
                    // it is a terminus if:
                    // the line uses unbunching but the stop does not; OR
                    // the line does not use unbunching but the stop does (is this even possible?)
                    // this resutls in the use of XOR, which is manifested as a !=.

                    // the invert mode is mainly for convenience, so that users click less times to set up correct unbunching points.
                    // IPT2 default for all stops is that unbunch = true
                    return lineIsUsingUnbunching != stopIsUsingUnbunching;
                default:
                    // dont block if somehow cannot determine if should insta depart
                    return false;
            }
        }
    }
}
