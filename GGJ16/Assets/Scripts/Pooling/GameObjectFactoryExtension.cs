using UnityEngine;

namespace GGJ16
{
	namespace Pooling
	{
		public static class GameObjectExtension
		{
			public static void Precache(this GameObject prefab, int count)
			{
				if (!GameObjectFactory.IsSafe)
					return;
				GameObjectFactory.Instance.Precache(prefab, count);
			}

			public static GameObject Spawn(this GameObject prefab)
			{
				if (!GameObjectFactory.IsSafe)
					return null;
				return GameObjectFactory.Instance.Spawn(prefab);
			}

			public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
			{
				if (!GameObjectFactory.IsSafe)
					return null;
				return GameObjectFactory.Instance.Spawn(prefab, parent, position, rotation);
			}

			public static void Despawn(this GameObject go)
			{
				if (!GameObjectFactory.IsSafe)
					return;
				GameObjectFactory.Instance.Despawn(go);
			}
		}
	}
}
