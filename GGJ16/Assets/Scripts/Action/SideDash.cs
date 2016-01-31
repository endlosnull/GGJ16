using UnityEngine;
using System.Collections;
using Pooling;

public class SideDash : GameAction
{
	public float force;
    public bool left;

	ActorController controller;
	FXGroup vfx;

	protected override void OnInvokeStart()
	{
		base.OnInvokeStart();

		controller = source.GetComponent<ActorController>();
        controller.actor.physics.velocity += (left ? controller.actor.Left : controller.actor.Right) * force;

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
