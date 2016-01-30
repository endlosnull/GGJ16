using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputMan : Singleton<InputMan>
{
	public Canvas splashCanvas;

	private System.Text.StringBuilder sb = new System.Text.StringBuilder();


	public void Update()
	{
		if( Boss.Instance.IsSettingUp )
		{
			if( Input.GetButton("KeyBtn0") )
			{
				Boss.Instance.AddKeyboardPlayer();
			}
			if( Input.GetButton("Joy1Btn0") )
			{
				Boss.Instance.AddJoystickPlayer(1);
			}
			if( Input.GetButton("Joy2Btn0") )
			{
				Boss.Instance.AddJoystickPlayer(2);
			}
			if( Input.GetButton("Joy3Btn0") )
			{
				Boss.Instance.AddJoystickPlayer(3);
			}
			if( Input.GetButton("Joy4Btn0") )
			{
				Boss.Instance.AddJoystickPlayer(4);
			}
		}

		if( Boss.Instance.IsInGame )
		{
			GameInput(Time.deltaTime);
			
		}
	}


	void GameInput(float deltaTime)
	{
		List<User> users = Boss.Instance.users;
		if( users.Count > 0 )
		{
			string inputPrefix = users[0].inputPrefix;
			Actor actor = users[0].controlledActor;
			if( actor != null )
			{
				ActorController controller = actor.controller;
				controller.InputClear();
				sb.Length = 0;
				sb.Append(inputPrefix);
				sb.Append("Horizontal");
				float hAxis = Input.GetAxis(sb.ToString());
				sb.Length = 0;
				controller.InputMove(hAxis,0f);	
		        sb.Append(inputPrefix);
				sb.Append("Vertical");
				float vAxis = Input.GetAxis(sb.ToString());
				sb.Length = 0;
				controller.InputMove(0f,vAxis);	
		        
		        sb.Append(inputPrefix);
				sb.Append("Btn0");
				bool btnAlpha = Input.GetButton(sb.ToString());
				sb.Length = 0;
				controller.InputAlpha = btnAlpha;

				sb.Append(inputPrefix);
				sb.Append("Btn1");
				bool btnBravo = Input.GetButton(sb.ToString());
				sb.Length = 0;
				controller.InputBravo = btnBravo;

		        controller.InputTick(deltaTime);
		    }
	    }
	}
}