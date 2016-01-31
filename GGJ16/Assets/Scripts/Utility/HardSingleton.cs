using UnityEngine;

public class HardSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	static T instance;
	static bool applicationIsQuitting;

	public static T Instance
	{
		get
		{
			if (applicationIsQuitting)
				return null;
			
			if (instance == null)
			{
				instance = (T)FindObjectOfType(typeof(T));
				if (instance == null)
				{
					Debug.Log("No Instance");
				}
			}
			return instance;
		}
	}

	public static bool HasInstance { get { return instance != null; } }
	public static bool IsSafe { get { return !applicationIsQuitting; } }

	public static void SingletonInit(T obj)
	{
		instance = obj;
	}

	void OnDestroy()
	{
		applicationIsQuitting = true;	
	}
}
