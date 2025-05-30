using System.Reflection;
using HarmonyLib;

namespace ExpressBusServices_IPT2
{
    internal static class PatchController
    {
        public static string HarmonyModID => "com.vectorial1024.cities.ebs_ipt2";

        /*
         * The "singleton" design is pretty straight-forward.
         */

        private static Harmony harmony;

        public static Harmony GetHarmonyInstance()
        {
            if (harmony == null)
            {
                harmony = new Harmony(HarmonyModID);
            }

            return harmony;
        }

        public static void Activate()
        {
            GetHarmonyInstance().PatchAll(Assembly.GetExecutingAssembly());
        }

        public static void Deactivate()
        {
            GetHarmonyInstance().UnpatchAll(HarmonyModID);
        }
    }
}
