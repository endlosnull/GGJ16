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

	List<string> players = new List<string>();
	public Transform playerUi;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SetPlayerState(string playerName, string teamName, SetPlayerStateAction action)
	{
		switch (action)
		{
			case SetPlayerStateAction.Add:
				players.Add(playerName);

				var go = Instantiate(playerUi);
				go.SetParent(GetComponent<Canvas>().gameObject.transform);
				go.localPosition = Vector3.zero;
				go.GetComponent<Text>().text = playerName;
				break;
		}
	}
}
