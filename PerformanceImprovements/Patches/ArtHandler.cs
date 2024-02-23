using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnboundLib;
using System.Collections;
using System.Linq;

namespace PerformanceImprovements.Patches
{
	[HarmonyPatch(typeof(ArtHandler), "NextArt")]
	internal class ArtHandler_Patch
	{
		private const int defaultParticles = 150;
		private const float defaultSimulationTime = 1.5f;

		private static GameObject Rendering => GameObject.Find("/Game/Visual/Rendering ");
		//private static GameObject Test = PerformanceImprovements.Assets.LoadAsset<GameObject>("Square");
        private static GameObject FrontParticles => Rendering?.transform?.GetChild(1)?.gameObject;
		private static GameObject BackParticles => Rendering?.transform?.GetChild(0)?.gameObject;
		private static GameObject Light => Rendering?.transform?.GetChild(3)?.GetChild(0)?.gameObject;

		private static IEnumerator InitParticles(ParticleSystem[] parts, int particles = defaultParticles)
		{
			foreach(ParticleSystem part in parts.Where(p => (bool)p?.gameObject?.activeSelf))
            {
				yield return InitParticles(part, particles);
            }
			yield break;
		}
		private void Awake()
		{
            //GameObject.DontDestroyOnLoad(Test);
        }
		private static IEnumerator InitParticles(ParticleSystem part, int particles = defaultParticles)
        {
			if (part == null) { yield break; }
			part.Clear();
			yield return new WaitForEndOfFrame();
			part.Simulate(defaultSimulationTime);
			yield return new WaitForEndOfFrame();
			part.Pause();
			yield break;
        }

		private static void Postfix()
		{
			//UnityEngine.Debug.Log("Map particle pain");
			//UnityEngine.Debug.Log(Rendering != null);
            //UnityEngine.Debug.Log(FrontParticles != null);
            foreach (ParticleSystem particleSystem in UnityEngine.Object.FindObjectsOfType<ParticleSystem>())
			{
                //UnityEngine.Debug.Log("These particles are here");
                ParticleSystem.MainModule main = particleSystem.main;
				main.maxParticles = (int)PerformanceImprovements.MaxNumberOfParticles;
			}
			foreach (Player player in PlayerManager.instance.players)
            {

				((ParticleSystem)player.gameObject.GetComponentInChildren<PlayerSkinParticle>().GetFieldValue("part")).emission.enabled.Equals(!PerformanceImprovements.DisablePlayerParticles);

				Gun gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
				GameObject spring = gun.gameObject.transform.GetChild(1).gameObject;
				GameObject handle = spring.transform.GetChild(2).gameObject;
				GameObject barrel = spring.transform.GetChild(3).gameObject;

				handle.GetComponent<SpriteMask>().enabled = !PerformanceImprovements.DisablePlayerParticles;
				handle.GetComponent<SpriteRenderer>().enabled = PerformanceImprovements.DisablePlayerParticles;
				handle.GetComponent<SpriteRenderer>().color = PerformanceImprovements.staticGunColor;
				barrel.GetComponent<SpriteMask>().enabled = !PerformanceImprovements.DisablePlayerParticles;
				barrel.GetComponent<SpriteRenderer>().enabled = PerformanceImprovements.DisablePlayerParticles;
				barrel.GetComponent<SpriteRenderer>().color = PerformanceImprovements.staticGunColor;

			}
			BackParticles?.SetActive(!PerformanceImprovements.DisableBackgroundParticles);
			//FrontParticles?.SetActive(!PerformanceImprovements.DisableMapParticles);
			FrontParticles?.GetComponentInChildren<GameObject>().SetActive(!PerformanceImprovements.DisableMapParticles);
			/*Test.GetOrAddComponent<Renderer>().sortingLayerName = "MapParticle";
			Test.GetComponent<Renderer>().sortingOrder = -1;
            if (PerformanceImprovements.DisableMapParticles)
                Test.SetActive(true);
            else
                Test.SetActive(false);
			Test.GetComponent<SpriteRenderer>().color = new Color(0.4f, 0.4f, 0.4f);*/
            //UnityEngine.Debug.Log(Mask!=null);
            //Mask.SetActive(false);
            //Mask.active = false;
            //Mask.gameObject.transform.localScale = Vector3.zero;
            Light?.SetActive(!PerformanceImprovements.DisableOverheadLightAndShadows);
			if (Light && Light.GetComponent<Screenshaker>())
			{
				Light.GetComponentInChildren<Screenshaker>().enabled = !PerformanceImprovements.DisableOverheadLightShake;
			}
			if ((bool)BackParticles?.activeSelf && PerformanceImprovements.DisableBackgroundParticleAnimations)
			{
				PerformanceImprovements.instance.StartCoroutine(InitParticles(BackParticles?.GetComponentsInChildren<ParticleSystem>()));
			}
			if ((bool)FrontParticles?.activeSelf && PerformanceImprovements.DisableForegroundParticleAnimations)
			{
				PerformanceImprovements.instance.StartCoroutine(InitParticles(FrontParticles?.GetComponentsInChildren<ParticleSystem>()));
			}
		}
	}
}
