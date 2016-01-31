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

	const float LeftTeamX = -200f;
	const float CenterX = 0f;
	const float RightTeamX = 200f;

	public Transform CatsTeamLabel;
	public Transform BirdsTeamLabel;

	// Use this for initialization
	void Start () {
		var pos = CatsTeamLabel.localPosition;
		pos.x = LeftTeamX;
		CatsTeamLabel.localPosition = pos;

		pos = BirdsTeamLabel.localPosition;
		pos.x = RightTeamX;
		BirdsTeamLabel.localPosition = pos;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void MoveCursor(int idx, MoveCursorAction action)
	{
		if (!CanMoveCursor(idx))
		{
			return;
		}
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
