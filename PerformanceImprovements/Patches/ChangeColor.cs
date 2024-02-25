using System;
using HarmonyLib;
using UnityEngine;

namespace PerformanceImprovements.Patches
{
    /*[HarmonyPatch(typeof(ChangeColor), "ChangeColor")]
   
    class ChangeColorPatchStart
    {
        private static void Postfix(ChangeColor __instance)
        {
            if (PerformanceImprovements.DisableBulletHitSurfaceParticleEffects)
            {
                if (__instance != null && __instance.gameObject != null) { UnityEngine.GameObject.Destroy(__instance.gameObject); }
            }
        }
    }*/
}

