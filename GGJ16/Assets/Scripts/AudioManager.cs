﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager>
{
	public AudioClip jump;
	public AudioClip throwBall;
	public AudioClip dash;
	public AudioClip hitPlayer;
	public AudioClip goal;
	public AudioClip airhorn;
	public float normalizedVolume = 1f;

	Dictionary<AudioSource, float> volumes = new Dictionary<AudioSource, float>();

	public void PlayOneShot(AudioSource source, AudioClip clip)
	{
		float volume = 1f;
		if (!volumes.TryGetValue(source, out volume))
		{
			volumes.Add(source, source.volume);
		}
		source.volume = volume * normalizedVolume;
		source.PlayOneShot(clip);
	}

	public void Play(AudioSource source, AudioClip clip)
	{
		float volume = 1f;
		if (!volumes.TryGetValue(source, out volume))
		{
			volumes.Add(source, source.volume);
		}
		source.volume = volume * normalizedVolume;
		source.clip = clip;
		source.Play();
	}
}
