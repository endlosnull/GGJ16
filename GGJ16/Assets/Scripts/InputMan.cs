using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputMan : Singleton<InputMan>
{
	public Canvas splashCanvas;

	private System.Text.StringBuilder sb = new System.Text.StringBuilder();

	public void Update()
	{
		float deltaTime = Time.deltaTime;
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
		List<User> users = Boss.Instance.Users;
		for(int i=0; i<users.Count; ++i)
		{
			ProcessUserInput(i, deltaTime);
		}
		
	}

	void ProcessUserInput(int idx, float deltaTime)
	{
		List<User> users = Boss.Instance.Users;
		string inputPrefix = users[idx].inputPrefix;
		sb.Length = 0;
		sb.Append(inputPrefix);
		sb.Append("Horizontal");
		float hAxis = Input.GetAxis(sb.ToString());
		sb.Length = 0;
		sb.Append(inputPrefix);
		sb.Append("Vertical");
		float vAxis = Input.GetAxis(sb.ToString());
		sb.Length = 0;

		sb.Append(inputPrefix);
		sb.Append("Btn0");
		bool btnAlpha = Input.GetButton(sb.ToString());
		sb.Length = 0;

		sb.Append(inputPrefix);
		sb.Append("Btn1");
		bool btnBravo = Input.GetButton(sb.ToString());
		sb.Length = 0;

		sb.Append(inputPrefix);
		sb.Append("BtnStart");
		bool btnStart = Input.GetButton(sb.ToString());
		sb.Length = 0;

		if( Boss.Instance.IsInGame )
		{
			GameInput(idx, hAxis, vAxis, btnAlpha, btnBravo, deltaTime);
		}
		else
		{
			MenuInput(idx, hAxis, vAxis, btnAlpha, btnBravo, btnStart, deltaTime);
		}
	}

	void GameInput(int idx, float hAxis, float vAxis, bool btnAlpha, bool btnBravo, float deltaTime)
	{
		List<User> users = Boss.Instance.Users;
		Actor actor = users[idx].controlledActor;
		if( actor != null )
		{
			ActorController controller = actor.controller;
			controller.InputClear();
			
			controller.InputMove(hAxis,0f);	
	        
			controller.InputMove(0f,vAxis);	
	        
			controller.InputAlpha = btnAlpha;

			controller.InputBravo = btnBravo;

	        controller.InputTick(deltaTime);
	    }
	}

	void MenuInput(int idx, float hAxis, float vAxis, bool btnAlpha, bool btnBravo, bool btnStart, float deltaTime)
	{
		Boss.Instance.MoveUserCursor(idx, hAxis, vAxis, btnAlpha, btnBravo, btnStart);
	}
}