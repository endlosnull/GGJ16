using UnityEngine;
using System.Collections;

public class LockInput : GameAction
{
	ActorController controller;

	protected override void OnInvokeStart()
	{
		base.OnInvokeStart();

		controller = target.GetComponent<ActorController>();
		controller.actionLocked = true;
	}

	protected override void OnInvokeEnd()
	{
		base.OnInvokeEnd();
		controller.actionLocked = false;
	}
}
