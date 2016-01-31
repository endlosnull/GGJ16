using UnityEngine;
using System.Collections;

public class Swat : GameAction
{
	public float range;
    public float yRange;

    ActorController controller;

	public Swat()
	{
		name = "swat";
	}

	protected override void OnInvokeStart()
	{
		base.OnInvokeStart();

        controller = source.GetComponent<ActorController>();
        controller.actor.body.SetAnimatorThrow();
        Ball ball = Field.Instance.ball;
        controller.actor.TrySwatBall(ball, this.range, this.yRange);
    }

    protected override void OnInvokeEnd()
	{
		base.OnInvokeEnd();
	}
}
