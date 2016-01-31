using UnityEngine;
using System.Collections;

public class Throw : GameAction
{
	public float forceForward;
    public float forceUp;

    ActorController controller;

	protected override void OnInvokeStart()
	{
		base.OnInvokeStart();

		controller = source.GetComponent<ActorController>();
		controller.actor.body.SetAnimatorThrow();
    }

    protected override void OnInvokeEnd()
	{
		base.OnInvokeEnd();
	}

	protected override void OnActionTime()
	{
		Ball ball = controller.actor.ownedBall;
		if (ball != null)
		{
			controller.actor.LosePossession();
			ball.physics.velocity += controller.actor.Forward * forceForward;
			ball.physics.velocity += Vector3.up * forceUp;
			AudioManager.Instance.PlayOneShot(controller.actor.audioSource, AudioManager.Instance.throwBall);
		}
	}
}
