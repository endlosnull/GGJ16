using UnityEngine;
using System.Collections;

namespace GGJ16
{
	public class SideDash : GameAction
	{
		public float force;

		ActorController controller;

		public override bool Invoke()
		{
			if (!base.Invoke())
			{
				return false;
			}

			OnInvokeStart();

			return true;
		}

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

		public override bool OnTick(float deltaTime)
		{
			if (!active)
			{
				return false;
			}

			if (base.OnTick(deltaTime))
			{
				OnInvokeEnd();
				return true;
			}
			return false;
		}
	}
}
