using UnityEngine;
using System.Collections;

namespace GGJ16
{
	public class Turn : GameAction
	{
		public float angleDegrees;

		ActorController controller;

		protected override void OnInvokeStart()
		{
			base.OnInvokeStart();

			controller = source.GetComponent<ActorController>();
            controller.actor.Turn(angleDegrees);
        }

        protected override void OnInvokeEnd()
		{
			base.OnInvokeEnd();
		}
	}
}
