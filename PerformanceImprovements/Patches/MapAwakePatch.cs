using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PerformanceImprovements.Patches
{
    [HarmonyPatch(typeof(Map), "Start")]
    public class MapAwakePatch
    {
        public static void Prefix()
        {
            List<SpriteMasking.SpriteMask> spriteMaskList = Component.FindObjectsOfType<SpriteMasking.SpriteMask>().ToList();

            foreach (SpriteMasking.SpriteMask spriteMask in spriteMaskList)
            {
                Transform currentParent = spriteMask.transform; // Start with the immediate parent
                Transform topMostParent = null;

                while (currentParent != null)
                {
                    topMostParent = currentParent; // Update the topMostParent to the current parent
                    currentParent = currentParent.parent; // Move to the next parent

                    if (currentParent != null && currentParent.name == "Map")
                    {
                        spriteMask.showMaskGraphics = true;
                        break; // Exit the loop once "Map" parent is found
                    }
                }
            }
        }
    }
}
