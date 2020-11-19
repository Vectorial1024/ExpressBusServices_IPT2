using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICities;
using UnityEngine;

namespace ExpressBusServices_IPT2
{
    public class ExpressBusServices_IPT2 : LoadingExtensionBase, IUserMod
    {
        public virtual string Name
        {
            get
            {
                return "Express Bus Services (IPT2 plugin)";
            }
        }

        public virtual string Description
        {
            get
            {
                return "Reads information from IPT2 unbunching settings to help the main mod.";
            }
        }

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

            PatchController.Activate();
        }

        /// <summary>
        /// Executed whenever a map is being unloaded.
        /// This mod then undoes the changes using the Harmony library.
        /// </summary>
        public override void OnLevelUnloading()
        {
            PatchController.Deactivate();
        }

        // It seems they will dynamically find whether a certain method that matches some criteria
        // exists, and then apply UI settings to it.
        // This is kinda like an in-house Harmony Lib except it targets some very specific areas.
        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase group = helper.AddGroup("Express Bus Services: Settings");
            group.AddDropdown("IPT2 Unbunching Interpretation",
                new string[] {
                    "First Principles",
                    "Respect IPT2 unbunching", 
                    "Invert IPT2 unbunching" },
                0,
                (index) => {
                    IPT2UnbunchingRuleReader.CurrentRuleInterpretation = (IPT2UnbunchingRuleReader.InterpretationMode) index;
                    Debug.Log($"Express Bus Services IPT2 Plugin: received index {index}");
                });
        }
    }
}
