using UnityEngine;

using System.Collections.Generic;

public class Team : MonoBehaviour
{
	public List<Actor> actors = new List<Actor>();
	public int teamIndex;
	public int score;
	public bool isDefense;
	public bool isOffense;

	public Actor bestBallHawk;
	Timer repositionTimer = new Timer(10f,true);
	Timer ballHawkScanTimer = new Timer(1f,true);

	void Update()
	{
		float deltaTime = Time.deltaTime;
		if(Boss.Instance.IsInGame)
		{

			if( repositionTimer.Tick(deltaTime) )
			{
				Reposition();	
			}
			if( ballHawkScanTimer.Tick(deltaTime) )
			{
				BallHawkScan();	
			}
		}
	}

	public void SetScore(int val)
	{
		score = val;
	}

	public void ModifyScore(int val)
	{
		score += val;
		Boss.Instance.RefreshScore();
	}

	public void RespawnActor()
	{
		foreach(Actor actor in actors)
		{
			Vector2 homePos = GetSpawnPos(actor.positionIndex);
			Vector3 homeVec = new Vector3(homePos.x, 0f, homePos.y);
			actor.SetUnityPhysics(false);
			actor.transform.position = homeVec;
			actor.SyncPhysics();
		}
	}

	void Reposition()
	{
		List<float> leashDistances = new List<float>();
		for(int i=0; i<actors.Count; ++i)
		{
			Vector2 leashPos = GetLeashPos(i);
			Vector3 leashVec = new Vector3(leashPos.x, 0f, leashPos.y);
			leashDistances.Add(99999f*99999f);
			for(int j=0; j<actors.Count; ++j)
			{
				Actor actor = actors[j];
				float curDiff = (leashVec - actor.transform.position).sqrMagnitude;
				if( curDiff < leashDistances[i] )
				{
					leashDistances[i] = curDiff;
				}
			}
		}
		leashDistances.Sort();

		List<Actor> workingActors = new List<Actor>(actors);
		for(int i=0; i<leashDistances.Count; ++i)
		{
			Vector2 leashPos = GetLeashPos(i);
			Vector3 leashVec = new Vector3(leashPos.x, 0f, leashPos.y);
			leashDistances[i] = 99999f*99999f;
			Actor bestActor = null;
			for(int j=0; j<workingActors.Count; ++j)
			{
				Actor actor = workingActors[j];
				float curDiff = (leashVec - actor.transform.position).sqrMagnitude;
				if( curDiff < leashDistances[i] )
				{
					leashDistances[i] = curDiff;
					bestActor = actor;
				}
			}
			bestActor.positionIndex = i;
			workingActors.Remove(bestActor);
		}

	}
	
	void BallHawkScan()
	{

		if( Field.Instance.ball == null )
		{
			return;
		}
		float bestDiff = 99999f*99999f;
		Transform ballTransform = Field.Instance.ball.transform;

		bestBallHawk = null;
		if( !isOffense )
		{
			for(int i=0; i<actors.Count; ++i)
			{
				Actor actor = actors[i];
				Vector2 leashPos = GetLeashPos(actor.positionIndex);
				Vector3 leashVec = new Vector3(leashPos.x, 0f, leashPos.y);
				float leashDiff = (leashVec - ballTransform.position).sqrMagnitude;

				//float sqrDiff = ( - actor.transform.position).sqrMagnitude;
				if( leashDiff < bestDiff )
				{
					bestDiff = leashDiff;
					bestBallHawk = actor;
				}
			}
		}
		
	}

	public Vector2 GetLeashPos(int slot)
	{
		if(isDefense)
		{
			return GetDefensePos(slot);
		}
		else if(isOffense)
		{
			return GetOffensePos(slot);
		}
		else
		{
			return GetHomePos(slot);
		}
	}

	public Vector2 GetHomePos(int slot)
	{
		if( teamIndex == 0 )
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector2(-10,0);
				case 1: return new Vector2(-2,-3);
				case 2: return new Vector2(-2,3);
				case 3: return new Vector2(3,1);
			}
		}
		else
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector2(10,0);
				case 1: return new Vector2(2,-3);
				case 2: return new Vector2(2,3);
				case 3: return new Vector2(-3,-1);
			}
		}
	}

	public Vector2 GetSpawnPos(int slot)
	{
		if( teamIndex == 0 )
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector2(-10,0);
				case 1: return new Vector2(-9,-3);
				case 2: return new Vector2(-9,3);
				case 3: return new Vector2(-8,1);
			}
		}
		else
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector2(10,0);
				case 1: return new Vector2(9,-3);
				case 2: return new Vector2(9,3);
				case 3: return new Vector2(8,-1);
			}
		}
	}

	public Vector2 GetOffensePos(int slot)
	{
		if( teamIndex == 0 )
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector2(-2,0);
				case 1: return new Vector2(3,-3);
				case 2: return new Vector2(3,3);
				case 3: return new Vector2(5,1);
			}
		}
		else
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector2( 2,0);
				case 1: return new Vector2(-3,-3);
				case 2: return new Vector2(-3,3);
				case 3: return new Vector2(-5,-1);
			}
		}
	}

	public Vector2 GetDefensePos(int slot)
	{
		if( teamIndex == 0 )
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector2(-10,0);
				case 1: return new Vector2(-5,-3);
				case 2: return new Vector2(-5,3);
				case 3: return new Vector2(-2,1);
			}
		}
		else
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector2(10,0);
				case 1: return new Vector2(5,-3);
				case 2: return new Vector2(5,3);
				case 3: return new Vector2(2,-1);
			}
		}
	}

	public string GetName(int slot)
	{
		if( teamIndex == 0 )
		{
			switch(slot) 
			{
				default:
				case 0: return "Roman";
				case 1: return "Rocky";
				case 2: return "Rex";
				case 3: return "Xavier";
			}
		}
		else
		{
			switch(slot) 
			{
				default:
				case 0: return "Anton";
				case 1: return "Hank";
				case 2: return "Clyde";
				case 3: return "Miles";
			}
		}
	}
	
}