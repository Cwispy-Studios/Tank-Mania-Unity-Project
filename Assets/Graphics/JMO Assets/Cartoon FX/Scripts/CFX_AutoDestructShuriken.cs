using System;
using System.Collections;

using UnityEngine;

// Cartoon FX  - (c) 2015 Jean Moreno

// Automatically destructs an object when it has stopped emitting particles and when they have all disappeared from the screen.
// Check is performed every 0.5 seconds to not query the particle system's state every frame.
// (only deactivates the object if the OnlyDeactivate flag is set, automatically used with CFX Spawn System)

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
	private ParticleSystem particleSystem;
	private GameObject parentProjectile;

	// If true, deactivate the object instead of destroying it
	public bool OnlyDeactivate;

	public Action OnDeactivate;

  private void Awake()
  {
		particleSystem = GetComponent<ParticleSystem>();
	}

  private void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}
	
	IEnumerator CheckIfAlive ()
	{		
		while (true)
		{
			yield return new WaitForSeconds(0.5f);

			if (!particleSystem.IsAlive(true))
			{
				if (OnlyDeactivate)
				{
					OnDeactivate?.Invoke();
					gameObject.SetActive(false);
				}

				else Destroy(gameObject);

				break;
			}
		}
	}
}