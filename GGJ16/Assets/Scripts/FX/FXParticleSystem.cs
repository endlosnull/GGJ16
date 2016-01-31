using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class FXParticleSystem : IFX 
{
	public new ParticleSystem particleSystem;

	void Reset()
	{
		particleSystem = GetComponent<ParticleSystem>();
	}

	public override void Play()
	{
		particleSystem.Play();
	}

	public override void Stop()
	{
		particleSystem.Stop();
	}
}
