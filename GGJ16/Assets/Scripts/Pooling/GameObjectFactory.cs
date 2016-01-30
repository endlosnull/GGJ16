using UnityEngine;
using System.Collections.Generic;

namespace GGJ16
{
	namespace Pooling
	{
		public class GameObjectFactory : Singleton<GameObjectFactory>
		{
			readonly string RESOURCES_PREFIX = "Prefabs/";
			readonly Vector3 VECTOR3_ZERO = Vector3.zero;
			readonly Quaternion QUATERNION_IDENTITY = Quaternion.identity;

			public Transform thisTransform;

			Dictionary<GameObject, GameObjectPool> pools = new Dictionary<GameObject, GameObjectPool>();

			void Awake()
			{
				thisTransform = transform;
			}

			public void Precache(GameObject prefab, int count)
			{
				GameObjectPool pool;
				if (!pools.TryGetValue(prefab, out pool))
				{
					pool = new GameObjectPool();
					pool.Precache(prefab, count);
					pools.Add(prefab, pool);
				}
			}

			public GameObject Spawn(GameObject prefab)
			{
				return Spawn(prefab, null, VECTOR3_ZERO, QUATERNION_IDENTITY);
			}

			public GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
			{
				GameObjectPool pool;
				if (!pools.TryGetValue(prefab, out pool))
				{
					pool = new GameObjectPool();
					pools.Add(prefab, pool);
				}
				GameObject go = pool.Spawn(prefab);
				go.SetActive(true);
				Transform t = go.transform;
				t.parent = parent;
				t.position = position;
				t.rotation = rotation;
				go.BroadcastMessage("OnObjectFactorySpawn", SendMessageOptions.DontRequireReceiver);
				return go;
			}

			public GameObject Spawn(string prefabName)
			{
				return Spawn(prefabName, null, VECTOR3_ZERO, QUATERNION_IDENTITY);
			}

			public GameObject Spawn(string prefabName, Transform parent, Vector3 position, Quaternion rotation)
			{
				GameObject prefab = Resources.Load(RESOURCES_PREFIX + prefabName) as GameObject;
				if (prefab == null)
				{
					return null;
				}

				return Spawn(prefab, parent, position, rotation);
			}

			public void Despawn(GameObject go)
			{
				GameObjectPool pool;
				GameObjectPoolMeta meta = go.GetComponent<GameObjectPoolMeta>();
				if (meta != null)
				{
					if (pools.TryGetValue(meta.prefab, out pool))
					{
						go.BroadcastMessage("OnObjectFactoryDespawn", SendMessageOptions.DontRequireReceiver);
						go.transform.parent = thisTransform;
						go.SetActive(false);
						pool.Despawn(go);
					}
				}
			}
		}
	}
}
