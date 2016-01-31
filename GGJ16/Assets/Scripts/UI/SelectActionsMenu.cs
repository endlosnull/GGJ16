using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class CursorPosition
{
	public int i;
	public int j;
}

class ActionBrickVisuals
{
	public string name;
	public string image;
}

public class SelectActionsMenu : MenuBehaviour
{
	List<Transform> actions = new List<Transform>();
	public Transform ActionBrick;
	List<CursorPosition> cursorPositions = new List<CursorPosition>();
	ActionBrickVisuals[] actionBricks = new ActionBrickVisuals[]
	{
		new ActionBrickVisuals { name = "Forward dash", image = "Textures/cards/arrow_up" },
		new ActionBrickVisuals { name = "Right dash", image = "Textures/cards/arrow_right" },
		new ActionBrickVisuals { name = "Left dash", image = "Textures/cards/arrow_left" },
		new ActionBrickVisuals { name = "Jump", image = "Textures/cards/foot" },
		new ActionBrickVisuals { name = "Throw", image = "Textures/cards/throw" },
		new ActionBrickVisuals { name = "Swat", image = "Textures/cards/hand" },
	};

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
		for(int n = 0; n < Cursors.Count; ++n)
		{
			cursorPositions.Add(new CursorPosition());
		}

		Transform canvasTransform = GetComponent<Canvas>().transform;
		int i = 0;
		int j = 0;
		for(int n = 0; n < actionBricks.Length; ++n)
		{
			if(n % 3 == 0)
			{
				i = 0;
				j++;
			}

			var cube = Instantiate(ActionBrick);
			cube.SetParent(canvasTransform);
			cube.localPosition = new Vector3(i * 200 - 200, j * 200 - 200, 0);
			cube.Find("BrickText").GetComponent<Text>().text = actionBricks[n].name;

			var sprite = Resources.Load<Sprite>(actionBricks[n].image);
			cube.Find("BrickBg").GetComponent<Image>().sprite = sprite;
			actions.Add(cube);
			i++;
		}
	}

	// Update is called once per frame
	void Update()
	{
	}

	public void MoveCursor(int idx, MoveCursorAction action)
	{
		if (!CanMoveCursor(idx) || this.cursorPositions.Count == 0)
		{
			return;
		}
		if( idx >= this.cursorPositions.Count )
		{
			Debug.LogWarning("Invalid index "+idx+" of "+this.cursorPositions.Count);
			return;
		}
		CursorPosition cp = this.cursorPositions[idx];
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
		//Debug.Log("cp" + cp);
		this.Cursors[idx].localPosition = new Vector3(cp.i * 200 - 200, cp.j * 200 - 400, 0);
	}
}
