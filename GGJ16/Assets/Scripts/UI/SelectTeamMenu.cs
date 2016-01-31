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
	List<Transform> playerObjects = new List<Transform>();
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
				AddPlayer(playerName);
				break;
			case SetPlayerStateAction.Remove:
				RemovePlayer(playerName);

				break;
		}
	}

	private void AddPlayer(string playerName)
	{
		var transform = Instantiate(playerUi);
		transform.SetParent(GetComponent<Canvas>().gameObject.transform);
		transform.localPosition = Vector3.zero;
		transform.GetComponent<Text>().text = playerName;

		players.Add(playerName);
		playerObjects.Add(transform);
	}

	private void RemovePlayer(string playerName)
	{
		int index = players.IndexOf(playerName);
		var transform = playerObjects[index];

		playerObjects.RemoveAt(index);
		players.RemoveAt(index);
		Destroy(transform);
	}
}
