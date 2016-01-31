using UnityEngine;
using System;
using System.Collections.Generic;

public class AgentController : ActorController
{
	AgentBehaviour behav;
	Timer reevaluateTimer = new Timer(2f, true);
	Timer scanTimer = new Timer(2f, true);
	Vector3 homePosition = Vector3.zero;
	List<AgentBehaviour> behavList = new List<AgentBehaviour>();
	BehaviourContext context = new BehaviourContext();

	public override void OnSpawn()
	{
		base.OnSpawn();
		homePosition = this.transform.position;
		AddBehaviour(gameObject.AddComponent<BehavSpace>());
		//AddBehaviour(gameObject.AddComponent<BehavBallHawk>());
		//AddBehaviour(gameObject.AddComponent<BehavDefendActor>());
		//AddBehaviour(gameObject.AddComponent<BehavOffenseScore>());
	}

	void AddBehaviour(AgentBehaviour beh)
	{
		behavList.Add(beh);
		beh.actor = actor;
		beh.source = actor.transform;
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
			behav = null;
		}
		if( behav == null )
		{
			float randomVal = UnityEngine.Random.value;
			int bestGoodness = -100;
			context.goal = Field.Instance.goal.transform;
			context.distToGoal = Vector3.Distance(context.goal.position, actor.transform.position);
			context.ball = Field.Instance.ball.transform;
			context.distToBall = Vector3.Distance(context.ball.position, actor.transform.position);
			context.isDefense = actor.team.isDefense;
			context.isOffense = actor.team.isOffense;
			context.hasBall = actor.ownedBall != null;
			for(int i=0; i<behavList.Count; ++i)
			{
				behavList[i].context = context;
				int goodness = behavList[i].GetGoodness();
				if( goodness > bestGoodness )
				{
					behav = behavList[i];
					bestGoodness = goodness;
				}
			}
			behav.Scan();

		}
	}
}