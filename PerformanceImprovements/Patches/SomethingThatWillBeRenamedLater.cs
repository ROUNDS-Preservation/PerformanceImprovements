using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib;

namespace PerformanceImprovements.Patches
{
    internal class SomethingThatWillBeRenamedLater
    {
        /*
        On.MainMenuHandler.Awake += (orig, self) =>
            {
                PerformanceImprovements.GameInProgress = false;

                orig(self);
            };
        */

        [HarmonyPatch(typeof(MainMenuHandler), "Awake")] //you act like im smart lol
        [HarmonyPrefix]
        static void OnButDifLMAO()
        {
            PerformanceImprovements.GameInProgress = false;
        }
        //On.Screenshaker.OnGameFeel += this.Screenshaker_OnGameFeel;
        [HarmonyPatch(typeof(Screenshaker), nameof(Screenshaker.OnGameFeel))] //this is a fucking pain oh my god wth
        [HarmonyPrefix]
        static void OnButDif2bcsimunorignal(Screenshaker __instance, ref Vector2 feelDirection)
        {
            feelDirection *= (float)(PerformanceImprovements.instance.ScreenShakeValue) / 100f; //REDO DOESNT WORK AGHHHHHHH
        }
        //On.ChomaticAberrationFeeler.OnGameFeel += this.ChomaticAberrationFeeler_OnGameFeel;
        [HarmonyPatch(typeof(ChomaticAberrationFeeler), nameof(ChomaticAberrationFeeler.OnGameFeel))] //this is a fucking pain oh my god wth
        [HarmonyPrefix]
        static void OnButDif3(ChomaticAberrationFeeler __instance, ref Vector2 feelDirection)
        {
            feelDirection *= (float)(PerformanceImprovements.instance.AberrationValue) / 100f; //fineeeeeeeeee
        }
    }
}
