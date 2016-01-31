using UnityEngine;
using System.Collections;

public class ForwardDash : GameAction
{
	public float force;

    ActorController controller;

	protected override void OnInvokeStart()
	{
		base.OnInvokeStart();

		controller = source.GetComponent<ActorController>();
        controller.actor.physics.velocity += controller.actor.Forward * force;
    }

    protected override void OnInvokeEnd()
	{
		base.OnInvokeEnd();
	}
}
