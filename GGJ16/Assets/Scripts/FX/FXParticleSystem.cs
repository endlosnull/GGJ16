using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class FXParticleSystem : IFX 
{
	public ParticleSystem system;

	void Reset()
	{
		system = GetComponent<ParticleSystem>();
	}

	public override void Play()
	{
		system.Play();
	}

	public override void Stop()
	{
		system.Stop();
	}
}
