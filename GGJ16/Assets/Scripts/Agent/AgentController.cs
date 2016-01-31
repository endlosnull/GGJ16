using UnityEngine;
using System;
using System.Collections.Generic;

public class AgentController : ActorController
{
	public AgentStrategy strategy;
	Timer reevaluateTimer = new Timer(2f, true);
	Timer scanTimer = new Timer(0.5f, true);
	Timer decideTimer = new Timer(0.25f, true);
	List<AgentStrategy> strategyList = new List<AgentStrategy>();
	StrategyContext context = new StrategyContext();

	float energyLevel = 12f;
	float maxEnergyLevel = 12f;
	Timer recoverEnergyTimer = new Timer(1f,true);

	public override void OnSpawn()
	{
		base.OnSpawn();
		AddStrategy(gameObject.AddComponent<StrategySpace>());
		AddStrategy(gameObject.AddComponent<StrategyBallHawk>());
		//AddStrategy(gameObject.AddComponent<StrategyOffenseScore>());
	}

	void AddStrategy(AgentStrategy strategy)
	{
		strategyList.Add(strategy);
		strategy.actor = actor;
		strategy.source = actor.transform;
	}

	public void Update()
	{
        return; // PRECHECKIN: remove
		float deltaTime = Time.deltaTime;
		InputClear();

		Reevaluate(deltaTime);
		Scan(deltaTime, false);
		Decide(deltaTime, false);

		Vector2 moveDir = Vector2.zero;
		float energyConsumed = 0f;
		if( energyLevel > 0f )
		{
			strategy.GetMove(ref moveDir, ref energyConsumed);
			int rand = UnityEngine.Random.Range(0,100);
			
			InputMove(moveDir.x,moveDir.y);	
        
			InputAlpha = strategy.GetAlpha(rand, ref energyConsumed);

			InputBravo = strategy.GetBravo(rand, ref energyConsumed);
			energyLevel -= energyConsumed*Time.deltaTime;
		}
		else
		{
			if( recoverEnergyTimer.Tick(deltaTime) )
			{
				energyLevel = maxEnergyLevel;
			}
		}

        InputTick(deltaTime);
		
	}

	void Scan(float deltaTime, bool forced)
	{
		if( forced || scanTimer.Tick(deltaTime) )
		{
			context.goal = Field.Instance.goal.transform;
			context.distToGoal = Vector3.Distance(context.goal.position, actor.transform.position);
			context.ball = Field.Instance.ball.transform;
			context.distToBall = Vector3.Distance(context.ball.position, actor.transform.position);
			context.isDefense = actor.team.isDefense;
			context.isOffense = actor.team.isOffense;
			context.hasBall = actor.ownedBall != null;
			for(int i=0; i<strategyList.Count; ++i)
			{
				strategyList[i].context = context;
			}
		}
	}
	void Decide(float deltaTime, bool forced)
	{
		if( forced || decideTimer.Tick(deltaTime) )
		{
			strategy.Decide();
		}
	}

	void Reevaluate(float deltaTime)
	{

		if( reevaluateTimer.Tick(deltaTime) )
		{
			strategy = null;
		}
		if( strategy == null )
		{
			int bestGoodness = -100;
			Scan(0, true);
			for(int i=0; i<strategyList.Count; ++i)
			{
				int goodness = strategyList[i].GetGoodness();
				if( goodness > bestGoodness )
				{
					strategy = strategyList[i];
					bestGoodness = goodness;
				}
			}
			Decide(0,true);

		}
	}
}