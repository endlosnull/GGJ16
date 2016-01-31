using UnityEngine;
using System.Collections;
using Pooling;

public class BackDash : GameAction
{
	public float force;

    ActorController controller;
	FXGroup vfx;

	public BackDash()
	{
		name = "back dash";
	}

	protected override void OnInvokeStart()
	{
		base.OnInvokeStart();

		controller = source.GetComponent<ActorController>();
        controller.actor.physics.velocity += -controller.actor.Forward * force;

		if (vfx != null)
		{
			vfx.Stop();
			vfx = null;
		}

		GameObject vfxGO = GameObjectFactory.Instance.Spawn("p-DashVFX");
		vfx = vfxGO.GetComponent<FXGroup>();
		vfx.transform.parent = controller.transform.FindTransformInChildren("Body");
		vfx.transform.localPosition = Vector3.zero;

		AudioManager.Instance.PlayOneShot(controller.actor.audioSource, AudioManager.Instance.dash);
    }

    protected override void OnInvokeEnd()
	{
		base.OnInvokeEnd();

		vfx.Stop();
		vfx = null;
	}
}
