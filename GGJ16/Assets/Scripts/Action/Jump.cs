using UnityEngine;
using System.Collections;

public class Jump : GameAction
{
	public float force;

	ActorController controller;

	public Jump()
	{
		name = "jump";
	}

	protected override void OnInvokeStart()
	{
		base.OnInvokeStart();

		controller = source.GetComponent<ActorController>();
        controller.actor.physics.velocity += Vector3.up * force;
		AudioManager.Instance.PlayOneShot(controller.actor.audioSource, AudioManager.Instance.jump);
    }

    protected override void OnInvokeEnd()
	{
		base.OnInvokeEnd();
	}
}
