using System;

using UnityEngine;

// Cartoon FX  - (c) 2015 Jean Moreno
//
// Script handling looped effect in the Demo Scene, so that they eventually stop

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoStopLoopedEffect : MonoBehaviour
{
	[SerializeField, Range(0f, 120f)] private float effectDuration = 2.5f;

	private ParticleSystem ps;

	private float d;

	public Action OnStop;

	private void Awake()
	{
		ps = GetComponent<ParticleSystem>();
	}

	void OnEnable()
	{
		d = effectDuration;
	}
	
	void Update()
	{
		if(d > 0)
		{
			d -= Time.deltaTime;
			if(d <= 0)
			{
				ps.Stop(true);

				OnStop?.Invoke();

				CFX_Demo_Translate translation = GetComponent<CFX_Demo_Translate>();
				if(translation != null)
				{
					translation.enabled = false;
				}
			}
		}
	}
}
