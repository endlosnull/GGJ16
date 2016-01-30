using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GGJ16;

public class InputMan : Singleton<InputMan>
{
	public Canvas splashCanvas;


	public void Update()
	{
		float deltaTime = Time.deltaTime;
		List<User> users = Boss.Instance.users;
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
				if (Input.GetKey(KeyCode.E))
				{
					GGJ16.ActionSequencer sequencer = actor.GetComponentInChildren<GGJ16.ActionSequencer>();
					sequencer.RunSequence(sequencer.sequences[0]);
				}

				controller.InputAlpha = Input.GetKey(KeyCode.Space);
				controller.InputBravo = Input.GetKey(KeyCode.LeftControl);
		        controller.InputTick(deltaTime);
		    }
	    }
	}


}