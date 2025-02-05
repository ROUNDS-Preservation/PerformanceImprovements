﻿using System;
using HarmonyLib;
using Unbound;
using UnityEngine;

namespace PerformanceImprovements.Patches
{
    [HarmonyPatch(typeof(CardChoiceVisuals), "Show")]

    class CardChoiceVisualsPatchShow
    {
        private static void Postfix(CardChoiceVisuals __instance, GameObject ___currentSkin)
        {
            if (___currentSkin?.GetComponentInChildren<ParticleSystem>() != null && (PerformanceImprovements.DisablePlayerParticles || PerformanceImprovements.DisableForegroundParticleAnimations))
            {
                ___currentSkin.GetComponentInChildren<ParticleSystem>().Pause();
            }
        }
    }
}
