﻿using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
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
					GameObject go = new GameObject(typeof(T).Name + " - Singleton");
					DontDestroyOnLoad(go);
					instance = go.AddComponent<T>();
				}
			}
			return instance;
		}
	}

	public static bool IsSafe { get { return !applicationIsQuitting; } }

	public static void SingletonInit(T obj)
	{
		instance = obj;
		Debug.Log("set Instance"+instance);
	}

	void OnDestroy()
	{
		applicationIsQuitting = true;	
	}
}
