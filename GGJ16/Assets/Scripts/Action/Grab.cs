using UnityEngine;
using System.Collections;

public class Grab : GameAction
{
	public float range;

    ActorController controller;

	protected override void OnInvokeStart()
	{
		base.OnInvokeStart();

		controller = source.GetComponent<ActorController>();
        Ball ball = controller.actor.boss.field.ball;
        controller.actor.TryTakePossession(ball, this.range);
    }

    protected override void OnInvokeEnd()
	{
		base.OnInvokeEnd();
	}
}
