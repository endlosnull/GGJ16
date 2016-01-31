using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Pooling;

public class Boss : HardSingleton<Boss>
{
	public ScoreUpdateEvent ScoreUpdate = new ScoreUpdateEvent();
	public List<User> users = new List<User>();
	public Field field;
	public List<User> Users { get { return users; } }
	List<Team> teams  = new List<Team>();
	public List<Team> Teams { get { return teams; } }
	public SequenceUpdatedEvent SequenceUpdated = new SequenceUpdatedEvent();

	User aiuser; 

	[System.Serializable]
	public enum State
	{
		None,
		SettingUp,
		PreGame,
		Loadout,
		StartingGame,
		InGame,
		EndingGame,
	}

	public State state = State.None;
	public bool IsSettingUp { get { return state == State.SettingUp; } }
	public bool IsInGame { get { return state == State.InGame; } }

	public Canvas startGameCanvas;
	public UpdateTimeEvent UpdateTime = new UpdateTimeEvent();
	public ChangeScreenEvent ChangeScreen = new ChangeScreenEvent();
	public MoveCursorEvent MoveCursor = new MoveCursorEvent();
	public AddActionEvent AddAction = new AddActionEvent();

	public Texture masterPaletteMain;
	public Texture masterPaletteAlt;

	float inputLock = -1f;


	void Awake()
	{
		ChangeState(State.SettingUp);
		ChangeScreen.Invoke("SelectTeam");
	}

	public void Update()
	{
		float deltaTime = Time.deltaTime;
		if( inputLock > 0f)
		{
			inputLock -= deltaTime;
	}
		if( Field.HasInstance )
		{
			UpdateTime.Invoke(Field.Instance.GameTime);
		}
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
			user.isLocalHuman = true;
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
			user.isLocalHuman = true;
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

	public void RefreshScore()
	{
		ScoreUpdate.Invoke(teams[0].score.ToString(), teams[1].score.ToString());
	}


	public void GotoStartGame()
	{
		ChangeState(State.StartingGame);
	}

	public void GotoLoadout()
	{
		ChangeState(State.Loadout);
	}

	public void ChangeState(State nextState)
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
			case State.Loadout:
				ChangeScreen.Invoke("SelectActions");
				StartLoadout();
				break;
			case State.StartingGame:
				ChangeScreen.Invoke("Play");
				StartGame();
				break;
			case State.EndingGame:
				ClearActors();
				ClearUsers();
				ChangeState(State.SettingUp);
				break;
			default:
				break;
		}
	}

	void ClearActors()
	{
		foreach(Team team in teams)
		{
			team.WipeActors();
		}
	}

	void ClearUsers()
	{
		foreach(User user in users)
		{
			Destroy(user);
		}
		users.Clear();
		aiuser = null;
	}

	public void StartSetup()
	{
		users.Clear();
		StartTeams();
        GameObjectFactory.Instance.Precache("p-Field",2);
	}

	void StartGame()
	{
		GameObject go = new GameObject();
		go.name = "aiuser";
		aiuser = go.AddComponent<User>();
		aiuser.isLocalHuman = false;
		aiuser.DefaultAISequences();
		users.Add(aiuser);

        StartField();
        StartUserActors();
        StartAgentActors();
        foreach(Team team in Boss.Instance.Teams)
		{
			team.SetScore(0);
        }
        field.SetState(Field.State.SettingUp);


    }

	void StartLoadout()
	{

	}

    void StartTeams()
    {
    	teams.Clear();
    	GameObject goLeft = new GameObject();
		goLeft.name = "TeamLeft";
		Team teamLeft = goLeft.AddComponent<Team>();
		teamLeft.teamIndex = 0;
		teamLeft.teamColor = Color.red;
		teams.Add(teamLeft);
    	GameObject goRight = new GameObject();
		goRight.name = "TeamRight";
		Team teamRight = goRight.AddComponent<Team>();
		teamRight.teamIndex = 1;
		teamRight.teamColor = Color.blue;
		teams.Add(teamRight);
    }


	void RegisterActor(Actor actor, Team team)
	{
		actor.team = team;
		actor.positionIndex = team.actors.Count;
		team.actors.Add(actor);
		field.allActors.Add(actor);
	}

	public void SetOffenseTeam(Team team)
	{
		bool freeBall = team == null;
		for(int i=0; i<teams.Count; ++i)
		{
			if(freeBall)
			{
				teams[i].isOffense = false;
				teams[i].isDefense = false;
			}
			else if( teams[i] == team )
			{
				teams[i].isOffense = true;
				teams[i].isDefense = false;
			}
			else if( teams[i] != team )
			{
				teams[i].isOffense = false;
				teams[i].isDefense = true;
			}
			
		}
	}

    void StartUserActors()
    {
		for (int i = 0; i < users.Count; ++i)
		{
			if( !users[i].isLocalHuman )
			{
				continue;
			}
			Team team = teams[users[i].teamIndex];

			Vector2 startPos = team.GetSpawnPos(team.actors.Count);
			Vector3 startVec = new Vector3(startPos.x, 0, startPos.y);


			GameObject go = GameObjectFactory.Instance.Spawn("p-Actor", null, startVec, Quaternion.identity);
			go.name = "hero" + i;
			Actor actor = go.GetComponent<Actor>();
			GameObject bodyObject = GameObjectFactory.Instance.Spawn("p-ActorBody", null, Vector3.zero, Quaternion.identity);
			bodyObject.name = "herobody" + i;
			bodyObject.transform.SetParent(actor.transform, false);
			actor.body = bodyObject.GetComponent<ActorBody>();
			if( i == 0 )
			{
				actor.body.SetTexture(masterPaletteMain);
			}
			else
			{
				actor.body.SetTexture(masterPaletteAlt);
			}
			GameObject attachObject = null;
			switch (team.teamIndex)
			{
			case 0:
				attachObject = GameObjectFactory.Instance.Spawn("p-AttachHeaddressCat", null, Vector3.zero, Quaternion.identity);
				break;
			case 1:
				attachObject = GameObjectFactory.Instance.Spawn("p-AttachHeaddressBird", null, Vector3.zero, Quaternion.identity);
				break;
			default:
				break;
			}

			if (attachObject != null)
			{
				attachObject.name = "attachment" + i;
				attachObject.transform.parent = bodyObject.transform.FindTransformInChildren("Head");
				attachObject.transform.localPosition = Vector3.zero;
				attachObject.transform.localRotation = Quaternion.AngleAxis(-90f, Vector3.up);
				actor.body.attachments.Add(attachObject);
			}

			users[i].controlledActor = actor;
			
			actor.sequencer = go.AddComponent<ActionSequencer>();
			actor.sequencer.LoadSequence(users[i].sequences);
			GameObject.Find("Ritual_" + i).GetComponent<SequenceHud>().sequencer = actor.sequencer;

			actor.controller = go.AddComponent<ActorController>();
			actor.isHuman = true;
			
			RegisterActor(actor, team);

			go.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
		}
		
	}

	void StartAgentActors()
	{
        for (int i=0; i<teams.Count;++i)
		{
			Team team = teams[i];
			for(int j=team.actors.Count;j<4;++j)
			{

				Vector2 startPos = team.GetSpawnPos(team.actors.Count);
				Vector3 startVec = new Vector3(startPos.x, 0, startPos.y);
				GameObject go = GameObjectFactory.Instance.Spawn("p-Actor", null, startVec, Quaternion.identity) ;
				go.name = "agent["+i+"]"+team.GetName(j);
				Actor actor = go.GetComponent<Actor>();

				GameObject bodyObject = GameObjectFactory.Instance.Spawn("p-ActorBody", null, Vector3.zero, Quaternion.identity) ;
				bodyObject.name = "herobody"+i;
				bodyObject.transform.SetParent(actor.transform, false);
				actor.body = bodyObject.GetComponent<ActorBody>();
				if( i == 0 )
				{
					actor.body.SetTexture(masterPaletteMain);
				}
				else
				{
					actor.body.SetTexture(masterPaletteAlt);
				}

				actor.sequencer = go.AddComponent<ActionSequencer>();
				actor.sequencer.LoadSequence(aiuser.sequences);
				actor.controller = go.AddComponent<AgentController>();
				actor.isHuman = false;
				RegisterActor(actor,team);



				go.AddComponent<ActionSequencer>();
				go.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	void StartField()
	{
		if(!Field.HasInstance)
		{
			GameObject fieldObject = GameObjectFactory.Instance.Spawn("p-Field", null, Vector3.zero, Quaternion.identity) ;
			fieldObject.name = "Field";
			field = fieldObject.GetComponent<Field>();
			HardSingleton<Field>.SingletonInit(field);	
		}
		
	}

	public void FixedUpdate()
	{
		if (field == null || field.ball == null)
			return;

		Ball ball = field.ball;
		for (int i = 0; i < field.allActors.Count; ++i)
		{
			Actor actor = field.allActors[i];
			actor.BallHandling(ball);
		}
	}

	public void MoveUserCursor(int idx, float hAxis, float vAxis, bool btnAlpha, bool btnBravo, bool btnStart)
	{
		if (Mathf.Abs(hAxis) > 0.05f)
		{
			MoveCursor.Invoke(idx, hAxis > 0f ? MoveCursorAction.Right : MoveCursorAction.Left);
		}
		if (Mathf.Abs(vAxis) > 0.05f)
		{
			MoveCursor.Invoke(idx, vAxis > 0f ? MoveCursorAction.Up : MoveCursorAction.Down);
		}
		if (btnStart && inputLock <= 0f)
		{
			if(state == State.SettingUp)
			{
				GotoLoadout();
				inputLock = 0.5f;
			}
			else if(state == State.Loadout)
			{
				GotoStartGame();
			}
		}
		if (btnAlpha || btnBravo)
		{
			AddAction.Invoke(idx, btnAlpha ? ActionMenuAction.Alpha : ActionMenuAction.Bravo);
		}
	}

	public void UpdateSequence(int playerIdx, int sequenceIdx, int sequence)
	{
		SequenceUpdated.Invoke(playerIdx, sequenceIdx, sequence);
	}

	public void TimeScale(float value, float duration)
	{
		StartCoroutine(TimeScaleCoroutine(value, duration));
	}

	IEnumerator TimeScaleCoroutine(float value, float duration)
	{
		float oldValue = Time.timeScale;
		Time.timeScale = value;

		float scalar = value / oldValue;

		yield return new WaitForSeconds(duration * scalar);

		Time.timeScale = oldValue;
	}
}
