using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class CursorPosition
{
	public int i;
	public int j;
}

public class SelectActionsMenu : MenuBehaviour
{
	public int BrickSize = 25;

	List<Transform> actions = new List<Transform>();
	public Transform ActionBrick;
	List<CursorPosition> cursorPositions = new List<CursorPosition>();
	List<string> actionMethods = new List<string>();

	public int MaxI = 5;
	public int maxJ = 0;

	protected override string MenuName
	{
		get
		{
			return "SelectActions";
		}
	}

	// Use this for initialization
	public override void Start()
	{
		base.Start();
		for (int c = 0; c < Cursors.Count; ++c)
		{
			var cursorPos = new CursorPosition();
			cursorPositions.Add(cursorPos);
			SetCursorPosition(c, cursorPos);
		}

		Transform actionBricksTransform = GameObject.Find("ActionBricks").transform;
		int i = 0;
		int j = -1;

		foreach (string k in ActionBricksDictionary.Dictionary.Keys)
		{
			actionMethods.Add(ActionBricksDictionary.Dictionary[k].method);
			if (i % MaxI == 0)
			{
				i = 0;
				j++;
			}

			var cube = Instantiate(ActionBrick);
			cube.SetParent(actionBricksTransform);
			cube.localPosition = new Vector3((i - MaxI / 2) * BrickSize, -j * BrickSize + BrickSize, 0);
			cube.Find("BrickText").GetComponent<Text>().text = ActionBricksDictionary.Dictionary[k].name;

			var sprite = Resources.Load<Sprite>(ActionBricksDictionary.Dictionary[k].image);
			cube.Find("BrickBg").GetComponent<Image>().sprite = sprite;
			actions.Add(cube);
			i++;

			maxJ = j;
		}
	}

	void Update()
	{
		int j = 0;
		List<User> users = Boss.Instance.users;
		for (int i = 0; i < users.Count; ++i)
		{
			if (users[i].isLocalHuman && j < this.Cursors.Count)
			{
				this.Cursors[j].gameObject.SetActive(true);
				++j;
			}
		}
		for (; j < this.Cursors.Count; ++j)
		{
			this.Cursors[j].gameObject.SetActive(false);
		}
	}


	public void MoveCursor(int idx, MoveCursorAction action)
	{
		if (!CanMoveCursor(idx) || this.cursorPositions.Count == 0)
		{
			return;
		}
		if (idx >= this.cursorPositions.Count)
		{
			Debug.LogWarning("Invalid index " + idx + " of " + this.cursorPositions.Count);
			return;
		}
		CursorPosition cp = this.cursorPositions[idx];
		switch (action)
		{
			case MoveCursorAction.Left:
				if (cp.i > 0) cp.i--;
				break;
			case MoveCursorAction.Right:
				if (cp.i + 1 < MaxI) cp.i++;
				break;
			case MoveCursorAction.Up:
				if (cp.j > 0) cp.j--;
				break;
			case MoveCursorAction.Down:
				if (cp.j < maxJ) cp.j++;
				break;
		}
		SetCursorPosition(idx, cp);
	}

	public void AddAction(int idx, ActionMenuAction actionToAdd)
	{
		if (!CanMoveCursor(idx) || this.cursorPositions.Count == 0)
		{
			return;
		}
		if (idx >= this.cursorPositions.Count)
		{
			Debug.LogWarning("Invalid index " + idx + " of " + this.cursorPositions.Count);
			return;
		}

		int actionIndex = this.cursorPositions[idx].i + this.cursorPositions[idx].j * MaxI;
		if (actionToAdd == ActionMenuAction.Alpha)
		{
			Type actionBricks = typeof(ActionBricks);
			var method = actionBricks.GetMethod(actionMethods[actionIndex]);
			Debug.Log("method" + actionMethods[actionIndex]);

			object[] p = new object[] { null, Boss.Instance.Users[idx].sequences[0] };
			method.Invoke(null, p);
		}
		else
		{
			Type actionBricks = typeof(ActionBricks);
			var method = actionBricks.GetMethod(actionMethods[actionIndex]);
			object[] p = new object[] { null, Boss.Instance.Users[idx].sequences[0] };
			method.Invoke(null, p);
			Debug.Log("method" + actionMethods[actionIndex]);
		}
	}

	private void SetCursorPosition(int idx, CursorPosition cp)
	{
		this.Cursors[idx].localPosition = new Vector3((cp.i - MaxI / 2) * BrickSize, -cp.j * BrickSize + BrickSize, 0);
	}
}
