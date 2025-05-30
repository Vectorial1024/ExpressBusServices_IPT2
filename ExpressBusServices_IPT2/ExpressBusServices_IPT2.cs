﻿using CitiesHarmony.API;
using ColossalFramework.UI;
using ICities;
using JetBrains.Annotations;
using UnityEngine;

namespace ExpressBusServices_IPT2
{
    [UsedImplicitly]
    public class ExpressBusServices_IPT2 : LoadingExtensionBase, IUserMod
    {
        public virtual string Name => "Express Bus Services (IPT2 plugin)";

        public virtual string Description => "Reads information from IPT2 unbunching settings to help the main mod.";

        /// <summary>
        /// Mod instance is created; initialize our values
        /// </summary>
        /// <param name="loading"></param>
        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            ModSettingController.Touch();
        }

        /// <summary>
        /// Executed whenever a level completes its loading process.
        /// This mod the activates and patches the game using Hramony library.
        /// </summary>
        /// <param name="mode">The loading mode.</param>
        public override void OnLevelLoaded(LoadMode mode)
        {
            /*
             * This function can still be called when loading up the asset editor,
             * so we have to check where we are right now.
             */

            switch (mode)
            {
                case LoadMode.LoadGame:
                case LoadMode.NewGame:
                case LoadMode.LoadScenario:
                case LoadMode.NewGameFromScenario:
                    break;

                default:
                    return;
            }

            // we write settings first so that settings can be updated in case harmony fails
            ModSettingController.Touch();
            UnifyHarmonyVersions();
            PatchController.Activate();
        }

        /// <summary>
        /// Executed whenever a map is being unloaded.
        /// This mod then undoes the changes using the Harmony library.
        /// </summary>
        public override void OnLevelUnloading()
        {
            // we write settings first so that settings can be updated in case harmony fails
            ModSettingController.WriteSettings();
            UnifyHarmonyVersions();
            PatchController.Deactivate();
        }

        // It seems they will dynamically find whether a certain method that matches some criteria
        // exists, and then apply UI settings to it.
        // This is kinda like an in-house Harmony Lib except it targets some very specific areas.
        [UsedImplicitly]
        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase group = helper.AddGroup("Express Bus Services: Settings");
            ModSettingController.Touch();
            int selectedIndex = (int) IPT2UnbunchingRuleReader.CurrentRuleInterpretation;
            var dropdown = group.AddDropdown("IPT2 Unbunching Interpretation",
                new string[] {
                    "First Principles",
                    "Respect IPT2 unbunching",
                    "Invert IPT2 unbunching" },
                0,
                (index) => {
                    IPT2UnbunchingRuleReader.CurrentRuleInterpretation = (IPT2UnbunchingRuleReader.InterpretationMode) index;
                    Debug.Log($"Express Bus Services IPT2 Plugin: received index {index}");
                    ModSettingController.WriteSettings();
                });
            UIDropDown properDropdownObject = dropdown as UIDropDown;
            if (properDropdownObject != null)
            {
                properDropdownObject.selectedIndex = selectedIndex;
            }
        }

        private static void UnifyHarmonyVersions()
        {
            if (HarmonyHelper.IsHarmonyInstalled)
            {
                // this code will redirect our Harmony 2.x version to the authoritative version stipulated by CitiesHarmony
                // I will make it such that the game will throw hard error if Harmony is not found,
                // as per my usual software deployment style
                // the user will have to subscribe to Harmony by themselves. I am not their parent anyways.
                // so this block will have to be empty.
            }
        }
    }
}
