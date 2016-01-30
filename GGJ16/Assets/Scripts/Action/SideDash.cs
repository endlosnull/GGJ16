using UnityEngine;
using System.Collections;

namespace GGJ16
{
	public class SideDash : GameAction
	{
		public float force;

		ActorController controller;

		public SideDash()
		{
			force = 2f;
			duration = 2f;
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

			controller.IndirectMove(0f, force);
		}
	}
}
