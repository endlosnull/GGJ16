using UnityEngine;
using System.Collections;

public static class TransformExtension
{
	
	public static Transform FindTransformInChildren(this Transform transform, string name, bool allowPartial = true, bool recursive = true)
	{
		Transform t = null;
		if (allowPartial && transform.name.Contains(name))
		{
			t = transform;
		}
		else if (!allowPartial && transform.name == name)
		{
			t = transform;
		}

		if (t != null)
		{
			return t;
		}

		if (recursive)
		{
			for (int i = 0; i < transform.childCount; ++i)
			{
				t = transform.GetChild(i).FindTransformInChildren(name, recursive);
				if (t != null)
				{
					return t;
				}
			}
		}

		return null;
	}
}
