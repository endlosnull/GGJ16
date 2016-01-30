using UnityEngine;
using System.Collections;

namespace GGJ16
{
	public class ForwardDash : GameAction
	{
		public float force;

		ActorController controller;

		public override bool Invoke()
		{
			if (!base.Invoke())
			{
				return false;
			}

			if (source == null)
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

			controller.IndirectMove(force, 0f);

			if (base.OnTick(deltaTime))
			{
				OnInvokeEnd();
				return true;
			}
			return false;
		}
	}
}
