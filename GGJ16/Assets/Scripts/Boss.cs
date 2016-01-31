using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Pooling;

[System.Serializable]
public class ScoreUpdateEvent : UnityEvent<string, string> { }
[System.Serializable]
public class ChangeScreenEvent : UnityEvent<string> { }
[System.Serializable]
public class UpdateTimeEvent : UnityEvent<float> { }
[System.Serializable]
public class SetPlayerStateEvent : UnityEvent<string, string, SetPlayerStateAction> { }

public enum SetPlayerStateAction
{
	Add,
	Remove,
	ChangeTeam
}

public class Boss : Singleton<Boss>
{
    public ScoreUpdateEvent ScoreUpdate = new ScoreUpdateEvent();
	public List<User> users  = new List<User>();
	public List<User> Users { get { return users; } }
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
    public UpdateTimeEvent UpdateTime = new UpdateTimeEvent();
	public ChangeScreenEvent ChangeScreen = new ChangeScreenEvent();
	public SetPlayerStateEvent SetPlayerState = new SetPlayerStateEvent();

    private float time;

	void Awake()
	{
		ChangeScreen.Invoke("SelectTeam");
	}

	public void Update()
	{
        startGameButton.enabled = ( IsSettingUp && users.Count > 0 );
        time += Time.deltaTime;
        UpdateTime.Invoke(time);
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
			SetPlayerState.Invoke(go.name, "", SetPlayerStateAction.Add);
		}
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
			SetPlayerState.Invoke(go.name, "", SetPlayerStateAction.Add);
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
				ChangeScreen.Invoke("SelectTeam");
				StartSetup();
				break;
			case State.InGame:
				ChangeScreen.Invoke("Play");
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
		for(int i=0; i<users.Count; ++i)
		{
			GameObject go = GameObjectFactory.Instance.Spawn("p-Actor", null, Vector3.zero, Quaternion.identity) ;
			go.name = "hero"+i;
			Actor actor = go.GetComponent<Actor>();
			GameObject bodyObject = GameObjectFactory.Instance.Spawn("p-ActorBodyOne", null, Vector3.zero, Quaternion.identity) ;
			bodyObject.name = "herobody"+i;
			bodyObject.transform.SetParent(actor.transform);
			bodyObject.transform.localRotation = Quaternion.AngleAxis(-90f,Vector3.up);
			actor.body = bodyObject.GetComponent<ActorBody>();
			GameObject attachObject = GameObjectFactory.Instance.Spawn("p-AttachHeaddressBird", null, Vector3.zero, Quaternion.identity) ;
			attachObject.name = "attachment"+i;
			actor.body.AttachToBone(attachObject, "Head");
			if( attachObject )
			{
				actor.body.attachments.Add(attachObject);
			}
			users[i].controlledActor = actor;

			//if it is local
			actor.controller = go.AddComponent<ActorController>();

			go.AddComponent<ActionSequencer>();
			go.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
		}

		GameObject fieldObject = GameObjectFactory.Instance.Spawn("p-Field", null, Vector3.zero, Quaternion.identity) ;
		Field field = fieldObject.GetComponent<Field>();
		field.BeginRound();
		
	}
}