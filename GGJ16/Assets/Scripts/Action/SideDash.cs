using UnityEngine;
using System.Collections;

public class SideDash : GameAction
{
	public float force;

	ActorController controller;

	protected override void OnInvokeStart()
	{
		base.OnInvokeStart();

		controller = source.GetComponent<ActorController>();
        controller.actor.physics.velocity += controller.actor.Right * force;
    }

    protected override void OnInvokeEnd()
	{
		base.OnInvokeEnd();
	}
}
