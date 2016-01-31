using UnityEngine;
using System.Collections;

public class Swat : GameAction
{
	public float range;

    ActorController controller;

	protected override void OnInvokeStart()
	{
		base.OnInvokeStart();

        controller = source.GetComponent<ActorController>();
        controller.actor.body.SetAnimatorThrow();
        Ball ball = Field.Instance.ball;
        controller.actor.TrySwatBall(ball, this.range);
    }

    protected override void OnInvokeEnd()
	{
		base.OnInvokeEnd();
	}
}
