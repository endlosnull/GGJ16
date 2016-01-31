using UnityEngine;
using System.Collections;

public class Jump : GameAction
{
	public float force;

	ActorController controller;

	protected override void OnInvokeStart()
	{
		base.OnInvokeStart();

		controller = source.GetComponent<ActorController>();
        controller.actor.physics.velocity += Vector3.up * force;
    }

    protected override void OnInvokeEnd()
	{
		base.OnInvokeEnd();
	}
}
