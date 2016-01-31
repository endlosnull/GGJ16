using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectTeamMenu : MenuBehaviour {

	protected override string MenuName
	{
		get
		{
			return "SelectTeam";
		}
	}

	const float LeftTeamX = -50f;
	const float CenterX = 0f;
	const float RightTeamX = 50f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void MoveCursor(int idx, MoveCursorAction action)
	{
		Vector3 newPosition;
		switch (action)
		{
			case MoveCursorAction.Left:
				newPosition = this.Cursors[idx].localPosition;
				newPosition.x = LeftTeamX;
				this.Cursors[idx].localPosition = newPosition;
				break;
			case MoveCursorAction.Right:
				newPosition = this.Cursors[idx].localPosition;
				newPosition.x = RightTeamX;
				this.Cursors[idx].localPosition = newPosition;
				break;
		}
	}
}
