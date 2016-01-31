using UnityEngine;
using System;

public class AgentController : ActorController
{
	AgentBehaviour behav;
	Timer reevaluateTimer = new Timer(2f, true);
	Timer scanTimer = new Timer(2f, true);
	Vector3 homePosition = Vector3.zero;


	public override void OnSpawn()
	{
		base.OnSpawn();
		homePosition = this.transform.position;
	}

	public void Update()
	{
		float deltaTime = Time.deltaTime;
		InputClear();

		Reevaluate(deltaTime);
		Scan(deltaTime);

		Vector2 moveDir = behav.GetMove();
			
		InputMove(moveDir.x,moveDir.y);	
        
		InputAlpha = behav.GetAlpha();

		InputBravo = behav.GetBravo();

        InputTick(deltaTime);
		
	}

	void Scan(float deltaTime)
	{
		if( scanTimer.Tick(deltaTime) )
		{
			behav.Scan();
		}
	}

	void Reevaluate(float deltaTime)
	{

		if( reevaluateTimer.Tick(deltaTime) )
		{
			Destroy(behav);
			behav = null;
		}
		if( behav == null )
		{
			float randomVal = UnityEngine.Random.value;
			if( randomVal < 0.5f )
			{
				behav = gameObject.AddComponent<BehavDefendSpace>();
			}
			else if( randomVal < 0.75f )
			{
				behav = gameObject.AddComponent<BehavDefendActor>();
			}
			else
			{
				behav = gameObject.AddComponent<BehavDefendBall>();
			}
			behav.sourceTeamIndex = actor.team.teamIndex;
			behav.source = this.transform;
			behav.target = this.transform;
			behav.homePos = new Vector2(homePosition.x, homePosition.z);
			behav.Scan();

		}
	}
}