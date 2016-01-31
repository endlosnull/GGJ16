using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectActionsMenu : MenuBehaviour {

	protected override string MenuName
	{
		get
		{
			return "SelectActions";
		}
	}

	List<string> players = new List<string>();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
}
