using UnityEngine;
using System.Collections.Generic;
using Pooling;

public class FXGroup : MonoBehaviour
{
	public List<IFX> fxList = new List<IFX>();

	public float delay;
	public float duration;
	public float expireTime;
	public bool playOnSpawn;
	public bool autoDespawn;

	bool active;
	float time;
	bool delayDone;
	bool durationDone;

	void Update()
	{
		if (!active)
		{
			return;
		}

		if (time >= delay && !delayDone)
		{
			DoPlay();
			delayDone = true;
			time = 0f;
		}

		if (durationDone && time >= expireTime)
		{
			active = false;
			if (autoDespawn)
			{
				gameObject.Despawn();
			}
		}

		if (duration > 0f)
		{
			if (time >= duration && !durationDone)
			{
				Stop();
			}
		}

		float deltaTime = Time.deltaTime;
		time += deltaTime;
	}

	void OnObjectFactorySpawn()
	{
		if (playOnSpawn)
		{
			Play();
		}
	}

	public void Play()
	{
		if (active)
		{
			return;
		}

		active = true;
		time = 0f;
		delayDone = false;
		durationDone = false;
	}

	void DoPlay()
	{
		for (int i = 0; i < fxList.Count; ++i)
		{
			fxList[i].Play();
		}
	}

	public void Stop()
	{
		if (!active)
		{
			return;
		}

		durationDone = true;
		time = 0f;

		for (int i = 0; i < fxList.Count; ++i)
		{
			fxList[i].Stop();
		}
	}
}
