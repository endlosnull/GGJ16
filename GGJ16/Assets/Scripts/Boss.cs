using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Pooling;

public class Boss : Singleton<Boss>
{
	public ScoreUpdateEvent ScoreUpdate = new ScoreUpdateEvent();
	public List<User> users = new List<User>();
	public Field field;
	public List<User> Users { get { return users; } }
	List<Team> teams  = new List<Team>();
	public List<Team> Teams { get { return teams; } }
	public int[] Scores = new int[] { 0, 0 };

	[System.Serializable]
	public enum State
	{
		None,
		SettingUp,
		PreGame,
		Loadout,
		InGame
	}

	public State state = State.None;
	public bool IsSettingUp { get { return state == State.SettingUp; } }
	public bool IsInGame { get { return state == State.InGame; } }

	public Canvas startGameCanvas;
	public UpdateTimeEvent UpdateTime = new UpdateTimeEvent();
	public ChangeScreenEvent ChangeScreen = new ChangeScreenEvent();
	public MoveCursorEvent MoveCursor = new MoveCursorEvent();

	private float time;

	void Awake()
	{
		ChangeState(State.SettingUp);
		ChangeScreen.Invoke("SelectTeam");
	}

	public void Update()
	{
		time -= Time.deltaTime;
		UpdateTime.Invoke(time);
	}

	public void AddKeyboardPlayer()
	{
		string prefix = "Key";
		User existingUser = users.Find(x => x.inputPrefix == prefix);
		if (existingUser == null)
		{
			GameObject go = new GameObject();
			go.name = "user" + users.Count;
			User user = go.AddComponent<User>();
			user.inputPrefix = prefix;
			users.Add(user);
		}
	}

	public void AddJoystickPlayer(int index)
	{
		string prefix = "Joy" + index;
		User existingUser = users.Find(x => x.inputPrefix == prefix);
		if (existingUser == null)
		{
			GameObject go = new GameObject();
			go.name = "user" + users.Count;
			User user = go.AddComponent<User>();
			user.inputPrefix = prefix;
			users.Add(user);
		}
	}

	public void AddRemote(int index)
	{
		string prefix = "Rmt" + index;
		User existingUser = users.Find(x => x.inputPrefix == prefix);
		if (existingUser == null)
		{
			GameObject go = new GameObject();
			go.name = "user" + users.Count;
			User user = go.AddComponent<User>();
			user.inputPrefix = prefix;
			users.Add(user);
		}
	}


	public void GotoInGame()
	{
		ChangeState(State.InGame);
	}

	void ChangeState(State nextState)
	{
		if (state == nextState)
		{
			return;
		}
		state = nextState;
		switch (state)
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

        StartTeams();
	}

	void StartGame()
	{
        time = 5f * 60f;
        StartUserActors();
        StartAgentActors();
        StartField();

    }

    void StartTeams()
    {
    	teams.Clear();
    	GameObject goLeft = new GameObject();
		goLeft.name = "TeamLeft";
		Team teamLeft = goLeft.AddComponent<Team>();
		teamLeft.teamIndex = 1;
		teams.Add(teamLeft);
    	GameObject goRight = new GameObject();
		goRight.name = "TeamRight";
		Team teamRight = goRight.AddComponent<Team>();
		teamRight.teamIndex = 1;
		teams.Add(teamRight);
    }

    void StartUserActors()
    {
		for (int i = 0; i < users.Count; ++i)
		{
			GameObject go = GameObjectFactory.Instance.Spawn("p-Actor", null, Vector3.zero, Quaternion.identity);
			go.name = "hero" + i;
			Actor actor = go.GetComponent<Actor>();
			GameObject bodyObject = GameObjectFactory.Instance.Spawn("p-ActorBodyOne", null, Vector3.zero, Quaternion.identity);
			bodyObject.name = "herobody" + i;
			bodyObject.transform.SetParent(actor.transform);
			actor.body = bodyObject.GetComponent<ActorBody>();
			GameObject attachObject = GameObjectFactory.Instance.Spawn("p-AttachHeaddressBird", null, Vector3.zero, Quaternion.identity);
			attachObject.name = "attachment" + i;
			actor.body.AttachToBone(attachObject, "model/Armature/Root/Body/Head");
			if (attachObject)
			{
				attachObject.transform.localRotation = Quaternion.AngleAxis(-90f, Vector3.up);
				actor.body.attachments.Add(attachObject);
			}
			users[i].controlledActor = actor;

			//if it is local
			actor.controller = go.AddComponent<ActorController>();

			go.AddComponent<ActionSequencer>();
			go.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
		}
		
	}

	void StartAgentActors()
	{
		Debug.Log("teams.Count"+teams.Count);
		for(int i=0; i<teams.Count;++i)
		{
			for(int j=0;j<3;++j)
			{
				GameObject go = GameObjectFactory.Instance.Spawn("p-Actor", null, Vector3.zero, Quaternion.identity) ;
				Debug.Log("Make"+go);
				go.name = "agent["+i+"]"+j;
				Actor actor = go.GetComponent<Actor>();
				GameObject bodyObject = GameObjectFactory.Instance.Spawn("p-ActorBodyOne", null, Vector3.zero, Quaternion.identity) ;
				bodyObject.name = "herobody"+i;
				bodyObject.transform.SetParent(actor.transform);
				bodyObject.transform.localRotation = Quaternion.AngleAxis(-90f,Vector3.up);
				actor.body = bodyObject.GetComponent<ActorBody>();

				actor.controller = go.AddComponent<AgentController>();

				go.AddComponent<ActionSequencer>();
				go.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	void StartField()
	{
		GameObject fieldObject = GameObjectFactory.Instance.Spawn("p-Field", null, Vector3.zero, Quaternion.identity) ;
		Field field = fieldObject.GetComponent<Field>();
		field.BeginRound();
	}

	public void FixedUpdate()
	{
		if (field == null)
			return;

		Ball ball = field.ball;
		for (int i = 0; i < users.Count; ++i)
		{
			Actor actor = users[i].controlledActor;
			actor.BallHandling(ball);
		}
	}
	public void MoveUserCursor(int idx, float hAxis, float vAxis)
	{
		if (Mathf.Abs(hAxis) > 0.05f)
		{
			MoveCursor.Invoke(idx, hAxis > 0f ? MoveCursorAction.Right : MoveCursorAction.Left);
		}
	}
}
