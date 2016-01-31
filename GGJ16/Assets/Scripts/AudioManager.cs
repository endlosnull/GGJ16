using UnityEngine;
using System.Collections;

public class AudioManager : Singleton<AudioManager>
{
	public AudioClip jump;
    public AudioClip swat;
    public AudioClip throwBall;
	public AudioClip dash;
	public AudioClip hitPlayer;
	public AudioClip goal;
	public AudioClip airhorn;

	public void PlayOneShot(AudioSource source, AudioClip clip)
	{
		//source.PlayOneShot(clip);
	}

	public void Play(AudioSource source, AudioClip clip)
	{
		source.clip = clip;
		//source.Play();
	}
}
