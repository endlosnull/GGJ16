using UnityEngine;
using System.Collections.Generic;
using Pooling;

public class Field : HardSingleton<Field>
{
	[System.Serializable]
	public enum State
	{
		None,
		SettingUp,
		InGame,
		AfterScore,
		Respawn,
		EndGame,
	}

	State state = State.None;

	public Goal goal;
	public Ball ball;
	public List<Actor> allActors;


	public FloorEntity[,] tiles;
	float floorHalfX = 0;
	float floorHalfZ = 0;
	int floorLengthX = 23;
	int floorLengthZ = 13;

	Timer advanceTimer = new Timer();
	bool hasFirstSetup = false;

	public Renderer[] arrows = new Renderer[2];

	private float gameTime;
	public float GameTime
	{
		get
		{
			switch(state)
			{
				case State.AfterScore:
				case State.SettingUp: return advanceTimer.TimeLeft;
				case State.InGame: return gameTime;
				default: return 0f;
			}
		}
	}

    public bool RoundActive
    {
        get
        {
            return state == State.InGame;
        }
    }



    void Update()
    {
    	float deltaTime = Time.deltaTime;

    	if( state == State.InGame )
    	{
			gameTime -= Time.deltaTime;
		}
    	if( advanceTimer.Tick(deltaTime) )
    	{
    		int currentState = (int)state;
    		SetState((State)(currentState+1));
    	}
    	List<Team> teams = Boss.Instance.Teams;
    	for(int i=0; i<teams.Count; ++i)
    	{
    		if( teams[i].isOffense )	
    		{
    			Color color = teams[i].teamColor;
    			color.a = 1.0f;
    			arrows[i].material.color = color;
    		}
    		else
    		{
    			Color color = teams[i].teamColor;
    			color.a = 0.4f;
    			arrows[i].material.color = color;
    		}
    	}
    }

    public void RespawnObjects()
	{
		Vector3 goalVec = GetGoalPos();
		goal.SetUnityPhysics(false);
		goal.transform.position = goalVec;
		goal.transform.localRotation = GetGoalRot();
		goal.SyncPhysics();

		Vector3 ballVec = GetBallPos();
		ball.SetUnityPhysics(false);
		ball.transform.position = ballVec;
		ball.transform.localRotation = Quaternion.identity;
		ball.SyncPhysics(); 
		ball.trail.enabled = false;
	}

    public void SetState(State nextState)
    {
    	if( state == nextState )
    	{
    		return;
    	}
    	Debug.Log("Field "+nextState);
    	state = nextState;
    	switch(state)
    	{
    		case State.SettingUp: Enter_SettingUp(); break;
    		case State.InGame: Enter_InGame(); break;
    		case State.AfterScore: Enter_AfterScore(); break;
    		case State.Respawn: Enter_Respawn(); break;
    		case State.EndGame: Enter_EndGame(); break;
    	}
    }

    Vector3 GetGoalPos()
    {
    	return Vector3.zero;
    }

    Quaternion GetGoalRot()
    {
    	return Quaternion.AngleAxis(-90f,Vector3.up);
    }

    Vector3 GetBallPos()
    {

    	return Vector3.forward*3f;
    }

    void Enter_SettingUp()
	{
		if( !hasFirstSetup )
		{	
			GameObject goalObject = GameObjectFactory.Instance.Spawn("p-Goal", null, GetGoalPos(), Quaternion.identity) ;
			goalObject.transform.localRotation = GetGoalRot();
			goalObject.name = "Goal";
			goal = goalObject.GetComponent<Goal>();
			goalObject.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);

			GameObject ballObject = GameObjectFactory.Instance.Spawn("p-Ball", null, GetBallPos(), Quaternion.identity) ;
			ballObject.name = "Ball";
	        ball = ballObject.GetComponent<Ball>();
	        ball.field = this;
	        ballObject.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);

	        floorHalfX = floorLengthX*0.5f;
	        floorHalfZ = floorLengthZ*0.5f;
	        tiles = new FloorEntity[floorLengthX+1,floorLengthZ+1];
	        for(int i=0; i<floorLengthX; ++i)
	        {
	        	for(int j=0; j<floorLengthZ; ++j)
		        {
		        	Vector3 startPos = GetTilePos(i,j);
		        	GameObject tileObject = GameObjectFactory.Instance.Spawn("p-Tile", this.transform, startPos, Quaternion.identity);
		        	tileObject.name = "Tile"+i+","+j;
		        	FloorEntity tile = tileObject.GetComponent<FloorEntity>();
		        	tiles[i,j] = tile;
		        	tile.SetColor(new Color(0f,0f,0f,0f));
	        		tileObject.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
		        }
	        }
	        hasFirstSetup = true;
	    }
	    else
	    {
	    	RespawnObjects();
	    	Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
	    	foreach(Renderer renderer in renderers)
	    	{
	    		renderer.gameObject.SetActive(true);
	    	}
	    }
			
		foreach(Team team in Boss.Instance.Teams)
		{
			team.GlobalLock(true);
			foreach (Actor actor in team.actors)
			{
				actor.transform.rotation = team.GetSpawnRot();
				actor.body.SetAnimatorMoveSpeed(0f);
			}
		}
		CamControl.Instance.target = ball.transform;

		Boss.Instance.RefreshScore();

        advanceTimer.SetDuration(3f);
    }

    void Enter_InGame()
    {
    	gameTime = 5f * 60f;
    	foreach(Team team in Boss.Instance.Teams)
		{
			team.GlobalLock(false);
		}
		ball.trail.enabled = true;
    	AudioManager.Instance.PlayOneShot(CamControl.Instance.audioSource, AudioManager.Instance.goal);
        Boss.Instance.ChangeState(Boss.State.InGame);
    }
    void Enter_AfterScore()
    {
    	foreach(Team team in Boss.Instance.Teams)
		{
			team.GlobalLock(true);
			if( team.score >= 3 )
			{
				SetState(State.EndGame);
				return;
			}
		}
    	advanceTimer.SetDuration(3f);
    }
    void Enter_Respawn()
    {
    	RespawnObjects();
    	foreach(Team team in Boss.Instance.Teams)
		{
			team.RespawnActors();
		}
		SetState(State.SettingUp);
    }
    void Enter_EndGame()
    {

    	Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
    	foreach(Renderer renderer in renderers)
    	{
    		renderer.gameObject.SetActive(false);
    	}
    	Boss.Instance.ChangeState(Boss.State.EndingGame);
    }


	public FloorEntity GetTile(Vector3 pos)
	{
		int x = (int)Mathf.Round(pos.x + floorHalfX-0.5f);
		int z = (int)Mathf.Round(pos.z + floorHalfZ-0.5f);
		if( x < floorLengthX && x > 0 && z < floorLengthZ && z > 0 )
		{
			return tiles[x,z];
		}
		return null;	
	}

	public Vector3 GetTilePos(int x, int z)
	{
		Vector3 pos = Vector3.zero;
		pos.x = x-floorHalfX+0.5f;
		pos.z = z-floorHalfZ+0.5f;
		return pos;
	}

    public void OnScore(int teamIndex)
    {
    	for (int i = 0; i < allActors.Count; ++i)
		{
			Actor actor = allActors[i];
			actor.SetUnityPhysics(true);
			actor.AddUnityExplosionForce(250f, Vector3.down * 10f, 250f);
		}
		ball.SetUnityPhysics(true);
		ball.AddUnityExplosionForce(250f, Vector3.down * 10f, 250f);
		goal.SetUnityPhysics(true);
		goal.AddUnityExplosionForce(250f, Vector3.down * 10f + Vector3.right*3f, 250f);
		CamControl.Instance.AddShake(0.4f);

		AudioManager.Instance.PlayOneShot(CamControl.Instance.audioSource, AudioManager.Instance.airhorn);
        CamControl.Instance.AddShake(0.2f);
        CamControl.Instance.AddZoom(5f, 0.5f);
        Boss.Instance.TimeScale(0.1f, 1f);
        Team scoringTeam = Boss.Instance.Teams.Find(x=>x.teamIndex == teamIndex);
        scoringTeam.ModifyScore(1);
        SetState(State.AfterScore);
    }
}