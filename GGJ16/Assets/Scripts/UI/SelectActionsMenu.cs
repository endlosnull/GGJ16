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
	public int BrickSize = 200;

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
	public override void Start()
	{
		base.Start();
		for (int c = 0; c < Cursors.Count; ++c)
		{
			cursorPositions.Add(new CursorPosition());
		}

		Transform canvasTransform = GetComponent<Canvas>().transform;
		int i = 0;
		int j = 0;

		foreach (string k in ActionBricksDictionary.Dictionary.Keys)
		{
			if (i % 3 == 0)
			{
				i = 0;
				j++;
			}

			var cube = Instantiate(ActionBrick);
			cube.SetParent(canvasTransform);
			cube.localPosition = new Vector3(i * BrickSize - BrickSize, j * BrickSize - BrickSize, 0);
			cube.Find("BrickText").GetComponent<Text>().text = ActionBricksDictionary.Dictionary[k].name;

			var sprite = Resources.Load<Sprite>(ActionBricksDictionary.Dictionary[k].image);
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
		if (idx >= this.cursorPositions.Count)
		{
			Debug.LogWarning("Invalid index " + idx + " of " + this.cursorPositions.Count);
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
		this.Cursors[idx].localPosition = new Vector3(cp.i * BrickSize - BrickSize, cp.j * BrickSize - BrickSize, 0);
	}
}
