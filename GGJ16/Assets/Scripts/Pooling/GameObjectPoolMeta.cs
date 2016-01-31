using UnityEngine;
using System.Collections;

namespace Pooling
{
	public class GameObjectPoolMeta : MonoBehaviour
	{
		public GameObject prefab;

		void OnDestroy()
		{
			gameObject.Despawn();
		}
	}
}
