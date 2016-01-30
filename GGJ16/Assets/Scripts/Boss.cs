using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using GGJ16;

[System.Serializable]
public class ScoreUpdateEvent : UnityEvent<string, string> {}
[System.Serializable]
public class ChangeScreenEvent : UnityEvent<string> {}
[System.Serializable]
public class UpdateTimeEvent : UnityEvent<float> {}

public class Boss : Singleton<Boss>
{
    public ScoreUpdateEvent ScoreUpdate = new ScoreUpdateEvent();
    public UpdateTimeEvent UpdateTime = new UpdateTimeEvent();
    private float time;
	public List<User> users  = new List<User>();
    public int[] Scores = new int[] {0, 0};

    [System.Serializable]
	public enum State
	{
		SettingUp,
		PreGame,
		Loadout,
		InGame
	}

	public State state = State.SettingUp;
	public bool IsSettingUp{ get { return state == State.SettingUp; } }
	public bool IsInGame{ get { return state == State.InGame; } }

	public Canvas startGameCanvas;
	public UnityEngine.UI.Button startGameButton;

	void Awake()
	{

	}

	public void Update()
	{

        time += Time.deltaTime;
        UpdateTime.Invoke(time);

        startGameButton.enabled = ( IsSettingUp && users.Count > 0 );
	}

	public void AddKeyboardPlayer()
	{
		string prefix = "Key";
		User existingUser = users.Find(x=>x.inputPrefix==prefix);
		if( existingUser == null )
		{
			GameObject go = new GameObject();
			go.name = "user"+users.Count;
			User user = go.AddComponent<User>();
			user.inputPrefix = prefix;
			users.Add(user);
		}
	}
    
    public void ChooseTeam()
    {
        
    }

	public void AddJoystickPlayer(int index)
	{
		string prefix = "Joy"+index;
		User existingUser = users.Find(x=>x.inputPrefix==prefix);
		if( existingUser == null )
		{
			GameObject go = new GameObject();
			go.name = "user"+users.Count;
			User user = go.AddComponent<User>();
			user.inputPrefix = prefix;
			users.Add(user);
		}
	}
	public void AddRemote(int index)
	{
		string prefix = "Rmt"+index;
		User existingUser = users.Find(x=>x.inputPrefix==prefix);
		if( existingUser == null )
		{
			GameObject go = new GameObject();
			go.name = "user"+users.Count;
			User user = go.AddComponent<User>();
			user.inputPrefix = prefix;
			users.Add(user);
		}
	}


	public void ChangeStateInt(int nextStateIdx)
	{
		ChangeState((State)nextStateIdx);
	}

	void ChangeState(State nextState)
	{
		if( state == nextState )
		{
			return;
		}
		state = nextState;
		switch(state)
		{
			case State.SettingUp:
				StartSetup();
				break;
			case State.InGame:
				StartGame();
				break;
			default:
				break;
		}
	}

	public void StartSetup()
	{
		users.Clear();
	}


	void StartGame()
	{
        time = 0f;

		startGameCanvas.gameObject.SetActive(false);
		for(int i=0; i<users.Count; ++i)
		{
			GameObject go = new GameObject();
			go.name = "hero"+i;
			Actor actor = go.AddComponent<Actor>();
			actor.controller = go.AddComponent<ActorController>();
			actor.engine = go.AddComponent<Engine>();
			GameObject torsoObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			GameObject headObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			torsoObject.transform.parent = actor.transform;
			headObject.transform.parent = actor.transform;
			headObject.transform.localPosition = Vector3.up*0.5f;
			actor.body = torsoObject.AddComponent<Body>();
			users[i].controlledActor = actor;
			go.AddComponent<ActionSequencer>();
			go.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
		}
	}
}