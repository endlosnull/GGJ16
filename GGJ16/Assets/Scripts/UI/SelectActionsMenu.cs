using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pooling;

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

	public List<CornerAnchor> cornerAnchors = new List<CornerAnchor>();


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
		cornerAnchors.Clear();
		for(int r=0; r<4; ++r)
		{
			RectTransform localTransform = this.transform as RectTransform;
			GameObject cornerObj = GameObjectFactory.Instance.SpawnUI("UI/CornerAnchor", localTransform.parent);
			cornerObj.name = "CornerAnchor "+r;
			cornerObj.transform.SetParent(localTransform);
			CornerAnchor cornerAnchor = cornerObj.GetComponent<CornerAnchor>();
			cornerAnchors.Add(cornerAnchor);

			RectTransform tr = cornerAnchor.transform as RectTransform;
			tr.anchoredPosition = GetCornerPos(r);
			Vector2 offsetMin = Vector2.zero;
			bool bottomSide = tr.anchoredPosition.y < -100;
			int padding = 20;

			tr = cornerAnchors[r].cursor.transform as RectTransform;
			tr.anchoredPosition = bottomSide ? new Vector2(32,41) : new Vector2(32,-41);
			tr = cornerAnchors[r].cursorText.transform as RectTransform;
			tr.anchoredPosition = offsetMin;
			offsetMin.y += padding*(bottomSide?1:-1);
			tr = cornerAnchors[r].brickAlpha0Text.transform as RectTransform;
			tr.anchoredPosition = offsetMin;
			offsetMin.y += padding*(bottomSide?1:-1);
			tr = cornerAnchors[r].brickAlpha1Text.transform as RectTransform;
			tr.anchoredPosition = offsetMin;
			offsetMin.y += padding*(bottomSide?1:-1);
			tr = cornerAnchors[r].brickAlpha2Text.transform as RectTransform;
			tr.anchoredPosition = offsetMin;
			offsetMin.y += padding*(bottomSide?1:-1);
			tr = cornerAnchors[r].brickAlpha3Text.transform as RectTransform;
			tr.anchoredPosition = offsetMin;
			offsetMin.y += padding*(bottomSide?1:-1);

			offsetMin = Vector2.zero;
			offsetMin.x += padding*4;
			offsetMin.y += padding*(bottomSide?1:-1);
			tr = cornerAnchors[r].brickBravo0Text.transform as RectTransform;
			tr.anchoredPosition = offsetMin;
			offsetMin.y += padding*(bottomSide?1:-1);
			tr = cornerAnchors[r].brickBravo1Text.transform as RectTransform;
			tr.anchoredPosition = offsetMin;
			offsetMin.y += padding*(bottomSide?1:-1);
			tr = cornerAnchors[r].brickBravo2Text.transform as RectTransform;
			tr.anchoredPosition = offsetMin;
			offsetMin.y += padding*(bottomSide?1:-1);
			tr = cornerAnchors[r].brickBravo3Text.transform as RectTransform;
			tr.anchoredPosition = offsetMin;
			offsetMin.y += padding*(bottomSide?1:-1);

			
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
				cornerAnchors[j].gameObject.SetActive(true);
				++j;
			}
		}
		for (; j < this.Cursors.Count; ++j)
		{
			this.Cursors[j].gameObject.SetActive(false);
			cornerAnchors[j].gameObject.SetActive(false);
		}

		for (int r = 0; r < 4; ++r)
		{
			cornerAnchors[r].cursorText.text = "P"+r;
			cornerAnchors[r].brickAlpha0Text.text = "";
			cornerAnchors[r].brickAlpha1Text.text = "";
			cornerAnchors[r].brickAlpha2Text.text = "";
			cornerAnchors[r].brickAlpha3Text.text = "";
			cornerAnchors[r].brickBravo0Text.text = "";
			cornerAnchors[r].brickBravo1Text.text = "";
			cornerAnchors[r].brickBravo2Text.text = "";
			cornerAnchors[r].brickBravo3Text.text = "";
			if( r < Boss.Instance.Users.Count )
			{
				string fullString = Boss.Instance.Users[r].sequenceString[0];
				string[] split = fullString.Split(',');

				if( split.Length > 0 ) { cornerAnchors[r].brickAlpha0Text.text = split[0]; }
				if( split.Length > 1 ) { cornerAnchors[r].brickAlpha1Text.text = split[1]; }
				if( split.Length > 2 ) { cornerAnchors[r].brickAlpha2Text.text = split[2]; }
				if( split.Length > 3 ) { cornerAnchors[r].brickAlpha3Text.text = split[3]; }

				fullString = Boss.Instance.Users[r].sequenceString[1];
				split = fullString.Split(',');

				if( split.Length > 0 ) { cornerAnchors[r].brickBravo0Text.text = split[0]; }
				if( split.Length > 1 ) { cornerAnchors[r].brickBravo1Text.text = split[1]; }
				if( split.Length > 2 ) { cornerAnchors[r].brickBravo2Text.text = split[2]; }
				if( split.Length > 3 ) { cornerAnchors[r].brickBravo3Text.text = split[3]; }
			}
		}

	}

	Vector2 GetCornerPos(int idx)
	{
		switch(idx)
		{
			default:
			case 0: return new Vector2(-300, -190); 
			case 1: return new Vector2(300, -190); 
			case 2: return new Vector2(300, -10);
			case 3: return new Vector2(-300, -10);
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
			Boss.Instance.Users[idx].UpdateMethods();

		}
		else
		{
			Type actionBricks = typeof(ActionBricks);
			var method = actionBricks.GetMethod(actionMethods[actionIndex]);
			object[] p = new object[] { null, Boss.Instance.Users[idx].sequences[1] };
			method.Invoke(null, p);
			Boss.Instance.Users[idx].UpdateMethods();


		}
	}

	private void SetCursorPosition(int idx, CursorPosition cp)
	{
		this.Cursors[idx].localPosition = new Vector3((cp.i - MaxI / 2) * BrickSize, -cp.j * BrickSize + BrickSize, 0);
	}
}
