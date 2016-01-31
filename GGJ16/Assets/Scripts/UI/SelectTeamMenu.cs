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
	public override void Start()
	{
		base.Start();
		var pos = CatsTeamLabel.localPosition;
		pos.x = LeftTeamX;
		CatsTeamLabel.localPosition = pos;

		pos = BirdsTeamLabel.localPosition;
		pos.x = RightTeamX;
		BirdsTeamLabel.localPosition = pos;
	}
	
	// Update is called once per frame
	void Update () {
		int j = 0;
		List<User> users = Boss.Instance.users;
		for(int i=0; i < users.Count; ++i)
		{
			if(users[i].isLocalHuman && j<this.Cursors.Count)
			{
				this.Cursors[j].gameObject.SetActive(true);
				++j;
			}
		}
		for(; j<this.Cursors.Count; ++j)
		{
			this.Cursors[j].gameObject.SetActive(false);
		}
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
