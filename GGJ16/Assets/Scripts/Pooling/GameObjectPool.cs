using UnityEngine;
using System.Collections.Generic;

namespace Pooling
{
	public class GameObjectPool
	{
		const int DEFAULT_CACHE_SIZE = 10;

		List<GameObject> pool = new List<GameObject>();
		int cacheSize = DEFAULT_CACHE_SIZE;

		public void Precache(GameObject prefab, int count)
		{
			cacheSize = count;
			for (int i = 0; i < cacheSize; ++i)
			{
				GameObject go = GameObject.Instantiate(prefab);
				GameObjectPoolMeta meta = go.AddComponent<GameObjectPoolMeta>();
				meta.prefab = prefab;
				go.SetActive(false);
				go.transform.SetParent(GameObjectFactory.Instance.thisTransform);
				pool.Add(go);
			}
		}

		public GameObject Spawn(GameObject prefab)
		{
			if (pool.Count == 0)
			{
				Precache(prefab, cacheSize);
			}
			GameObject go = pool[0];
			pool.RemoveAt(0);
			return go;
		}

		public void Despawn(GameObject go)
		{
			pool.Add(go);
		}
	}
}
