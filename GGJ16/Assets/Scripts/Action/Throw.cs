using UnityEngine;
using System.Collections;

public class Throw : GameAction
{
	public float forceForward;
    public float forceUp;

    ActorController controller;

	public override bool Invoke()
	{
		if (!base.Invoke())
		{
			return false;
		}

		if (source == null)
		{
			return false;
		}

		OnInvokeStart();

		return true;
	}

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
		}
	}

	public override bool OnTick(float deltaTime)
	{
		if (!active)
		{
			return false;
		}

		if (base.OnTick(deltaTime))
		{
			OnInvokeEnd();
			return true;
		}
		return false;
	}
}
