using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GGJ16;

public class Boss : Singleton<Boss>
{


	public List<User> users;

	void Awake()
	{
	}

	public void Update()
	{

	}


	public void SetupGame(int players)
	{
		users.Clear();
		for(int i=0; i<players; ++i)
		{
			GameObject go = new GameObject();
			go.name = "user"+i;
			User user = go.AddComponent<User>();
			users.Add(user);
		}
		StartGame();
	}

	public void StartGame()
	{
		InputMan.Instance.splashCanvas.gameObject.SetActive(false);
		for(int i=0; i<users.Count; ++i)
		{
			GameObject go = new GameObject();
			go.name = "hero"+i;
			Actor actor = go.AddComponent<Actor>();
			actor.controller = go.AddComponent<ActorController>();
			actor.engine = go.AddComponent<Engine>();
			GameObject torsoObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			GameObject headObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			torsoObject.transform.parent = actor.transform;
			headObject.transform.parent = actor.transform;
			headObject.transform.localPosition = Vector3.up*0.5f;
			actor.body = torsoObject.AddComponent<Body>();
			users[i].controlledActor = actor;	
			go.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);

		}
	}
}