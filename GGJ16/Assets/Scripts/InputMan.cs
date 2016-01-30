using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputMan : MonoBehaviour
{
	public Canvas splashCanvas;
	public List<User> users;

	public void Update()
	{
		float deltaTime = Time.deltaTime;
		if( users.Count > 0 )
		{
			Actor actor = users[0].controlledActor;
			if( actor != null )
			{
				ActorController controller = actor.controller;
				controller.InputClear();
				if (Input.GetKey(KeyCode.W))
		        {
		        	controller.InputMove(0,1f);	
		        }
		        if (Input.GetKey(KeyCode.A))
		        {
		        	controller.InputMove(-1f,0);	
		        }
		        if (Input.GetKey(KeyCode.D))
		        {
		        	controller.InputMove(1f,0);	
		        }
		        if (Input.GetKey(KeyCode.S))
		        {
		        	controller.InputMove(0,-1f);	
		        }

				controller.InputAlpha = Input.GetKey(KeyCode.Space);
				controller.InputBravo = Input.GetKey(KeyCode.LeftControl);
		        controller.InputTick(deltaTime);
		    }
	    }
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
		splashCanvas.enabled = false;
		for(int i=0; i<users.Count; ++i)
		{
			GameObject go = new GameObject();
			go.name = "hero"+i;
			Actor actor = go.AddComponent<Actor>();
			actor.controller = go.AddComponent<ActorController>();
			actor.engine = go.AddComponent<Engine>();
			actor.ui = go.AddComponent<Score>();
			GameObject bodyObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			bodyObject.transform.parent = actor.transform;
			actor.body = bodyObject.AddComponent<Body>();
			users[i].controlledActor = actor;	
			go.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);

		}
	}
}