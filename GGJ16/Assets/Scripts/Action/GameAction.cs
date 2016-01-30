using UnityEngine;
using System.Collections;

namespace GGJ16
{
	public class GameAction : MonoBehaviour
	{
		public GameObject source;
		public GameObject target;
		public float duration;

		protected bool active;
		protected float startTime;

		public float ElapsedTime
		{
			get
			{
				return Time.time - startTime;
			}
		}

		public virtual bool Invoke()
		{
			if (active)
			{
				return false;
			}

			startTime = Time.time;
			active = true;
			return true;
		}

		public virtual void Stop()
		{
			active = false;
		}
	}
}
