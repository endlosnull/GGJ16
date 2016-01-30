using UnityEngine;
using System.Collections;

namespace GGJ16
{
	public class ForwardDash : GameAction
	{
		public float force;

		ActorController controller;

		public ForwardDash()
		{
			force = 1f;
			duration = 1f;
		}

		public override bool Invoke()
		{
			if (!base.Invoke())
			{
				return false;
			}

			if (source == null)
			{
				active = false;
				return false;
			}

			controller = source.GetComponent<ActorController>();

			return true;
		}

		void Update()
		{
			if (!active)
			{
				return;
			}

			if (ElapsedTime >= duration)
			{
				Stop();
				return;
			}

			controller.IndirectMove(force, 0f);
		}
	}
}
