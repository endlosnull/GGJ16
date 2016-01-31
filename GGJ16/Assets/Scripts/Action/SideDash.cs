using UnityEngine;
using System.Collections;
using Pooling;

public class SideDash : GameAction
{
	public float force;

	ActorController controller;
	FXGroup vfx;

	protected override void OnInvokeStart()
	{
		base.OnInvokeStart();

		controller = source.GetComponent<ActorController>();
        controller.actor.physics.velocity += controller.actor.Right * force;

		GameObject vfxGO = GameObjectFactory.Instance.Spawn("p-DashVFX");
		vfx = vfxGO.GetComponent<FXGroup>();
		vfx.transform.parent = controller.transform.FindTransformInChildren("Body");
		vfx.transform.localPosition = Vector3.zero;

		AudioManager.Instance.PlayOneShot(controller.actor.audioSource, AudioManager.Instance.dash);
    }

    protected override void OnInvokeEnd()
	{
		base.OnInvokeEnd();
	}
}
