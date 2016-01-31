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
			behav.Dispose();
			behav = null;
		}
		if( behav == null )
		{
			if( UnityEngine.Random.value < 0.5 )
			{
				behav = new BehavDefendSpace();
			}
			else
			{
				behav = new BehavOffenseAdvance();
			}
			behav.source = this.transform;
			behav.target = this.transform;
			behav.homePos = new Vector2(homePosition.x, homePosition.z);

		}
	}
}