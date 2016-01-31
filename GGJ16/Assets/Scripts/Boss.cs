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

	[System.Serializable]
	public enum State
	{
		None,
		SettingUp,
		PreGame,
		Loadout,
		StartGame,
		InGame,
	}

	public State state = State.None;
	public bool IsSettingUp { get { return state == State.SettingUp; } }
	public bool IsInGame { get { return state == State.InGame; } }

	public Canvas startGameCanvas;
	public UpdateTimeEvent UpdateTime = new UpdateTimeEvent();
	public ChangeScreenEvent ChangeScreen = new ChangeScreenEvent();
	public MoveCursorEvent MoveCursor = new MoveCursorEvent();

	public Texture masterPaletteMain;
	public Texture masterPaletteAlt;

	private float time;

	void Awake()
	{
		time = 0f;
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

	public void RefreshScore()
	{
		ScoreUpdate.Invoke("Team1:"+teams[0].score, "Team2:"+teams[1].score);
	}


	public void GotoStartGame()
	{
		ChangeState(State.StartGame);
	}

	public void GotoLoadout()
	{
		ChangeState(State.Loadout);
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
			case State.Loadout:
				ChangeScreen.Invoke("SelectActions");
				StartLoadout();
				break;
			case State.StartGame:
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
        StartField();
        StartUserActors();
        StartAgentActors();
        field.BeginRound();
        teams[0].SetScore(0);
        teams[1].SetScore(0);
        ChangeState(State.InGame);

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
		teams.Add(teamLeft);
    	GameObject goRight = new GameObject();
		goRight.name = "TeamRight";
		Team teamRight = goRight.AddComponent<Team>();
		teamRight.teamIndex = 1;
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
			//fix this to get the team
			Team team = teams[0];

			Vector2 startPos = team.GetHomePos(team.actors.Count);
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
			GameObject attachObject = GameObjectFactory.Instance.Spawn("p-AttachHeaddressBird", null, Vector3.zero, Quaternion.identity);
			attachObject.name = "attachment" + i;
			attachObject.transform.parent = bodyObject.transform.FindTransformInChildren("Head");
			attachObject.transform.localPosition = Vector3.zero;
			attachObject.transform.localRotation = Quaternion.AngleAxis(-90f, Vector3.up);
			actor.body.attachments.Add(attachObject);
            actor.boss = this;
            users[i].controlledActor = actor;
			
			actor.sequencer = go.AddComponent<ActionSequencer>();
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

				Vector2 startPos = team.GetHomePos(team.actors.Count);
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
                actor.boss = this;

                actor.sequencer = go.AddComponent<ActionSequencer>();
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
		GameObject fieldObject = GameObjectFactory.Instance.Spawn("p-Field", null, Vector3.zero, Quaternion.identity) ;
		field = fieldObject.GetComponent<Field>();
		
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
		if (btnStart && time <= 0f)
		{
			if(state == State.SettingUp)
			{
				GotoLoadout();
				time = 1f;
			}
			else if(state == State.Loadout)
			{
				GotoStartGame();
			}
		}
	}
}
