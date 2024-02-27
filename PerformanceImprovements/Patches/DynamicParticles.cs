using System;
using HarmonyLib;
using UnityEngine;
using UnboundLib;
using FriendlyFoe;

namespace PerformanceImprovements.Patches
{
    [HarmonyPatch(typeof(DynamicParticles), "PlayBulletHit")]
    class DynamicParticlesPatchPlayBulletHit
    {
        private static bool Prefix(DynamicParticles __instance, float damage, Transform spawnerTransform, HitInfo hit, Color projectielColor, ref int ___spawnsThisFrame)
        {
            if (PerformanceImprovements.DisableBulletHitSurfaceParticleEffects)
            {
                foreach (GameObject g in GameObject.FindObjectsOfType<GameObject>())
                    if (g.GetComponent<ChangeColor>() != null)
                        UnityEngine.GameObject.Destroy(g);
            }
            if (PerformanceImprovements.DisableBulletHitSurfaceParticleEffects || (float)___spawnsThisFrame > PerformanceImprovements.MaximumBulletHitParticlesPerFrame || PerformanceImprovements.hitEffectsSpawnedThisFrame >= PerformanceImprovements.MaximumBulletHitParticlesPerFrame)
			{
				return false;
			}
			PerformanceImprovements.hitEffectsSpawnedThisFrame++;
			___spawnsThisFrame++;
			int num = 0;
			int num2 = 1;
			while (num2 < __instance.bulletHit.Length && __instance.bulletHit[num2].dmg <= damage)
			{
				num = num2;
				num2++;
			}
			PoolableWrapper[] array = ObjectsToSpawn.SpawnObject(__instance.transform, hit, __instance.bulletHit[num].objectsToSpawn, null, null, 55f, null, false);
			if (PerformanceImprovements.FixBulletHitParticleEffects)
            {
				foreach (PoolableWrapper obj in array)
                {
					if (obj.Instance != null)
					{
						obj.Instance.AddComponent<RemoveAfterPoint>();


						RemoveAfterSeconds rem = obj.Instance.GetComponent<RemoveAfterSeconds>();
						if (rem == null)
						{
							rem = obj.Instance.AddComponent<RemoveAfterSeconds>();
							rem.seconds = 2f;
						}
					}
                }
            }
			
			if (projectielColor != Color.black)
			{
				for (int i = 0; i < array.Length; i++)
				{
					for (int j = 0; j < array[i].Instance.transform.childCount; j++)
					{
						ChangeColor componentInChildren = array[i].Instance.transform.GetChild(j).GetComponentInChildren<ChangeColor>();
						if (componentInChildren)
						{
							componentInChildren.GetComponent<ParticleSystemRenderer>().material.color = projectielColor;
						}
					}
				}
			}

			return false;
		}
    }
}
