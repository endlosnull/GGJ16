using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class CursorPosition
{
	public int i;
	public int j;
	public float lastTime;
}

public class SelectActionsMenu : MenuBehaviour
{
	List<Transform> actions = new List<Transform>();
	public Transform ActionBrick;
	List<CursorPosition> cursorPositions = new List<CursorPosition>();

	protected override string MenuName
	{
		get
		{
			return "SelectActions";
		}
	}

	// Use this for initialization
	void Start()
	{
		InitializeTimes();

		for(int i = 0; i < Cursors.Length; ++i)
		{
			cursorPositions.Add(new CursorPosition());
		}

		Transform canvasTransform = GetComponent<Canvas>().transform;
		for (int i = 0; i < 5; ++i)
		{
			for (int j = 0; j < 3; ++j)
			{
				var cube = Instantiate(ActionBrick);
				cube.SetParent(canvasTransform);
				cube.localPosition = new Vector3(i * 120 - 240, j * 120 - 120, 0);
				actions.Add(cube);
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
	}

	public void MoveCursor(int idx, MoveCursorAction action)
	{
		if (!CanMoveCursor(idx))
		{
			return;
		}
		CursorPosition cp = this.cursorPositions[idx];
		cp.lastTime = Time.realtimeSinceStartup;
		switch (action)
		{
			case MoveCursorAction.Left:
				cp.i--;
				break;
			case MoveCursorAction.Right:
				cp.i++;
				break;
			case MoveCursorAction.Up:
				cp.j++;
				break;
			case MoveCursorAction.Down:
				cp.j--;
				break;
		}
		Debug.Log("cp" + cp);
		this.Cursors[idx].localPosition = new Vector3(cp.i * 120 - 240, cp.j * 120 - 240, 0);
	}
}
