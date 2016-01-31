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

	float energyLevel;
	float maxEnergyLevel;
	Timer recoverEnergyTimer;
	Timer alphaTimer;
	Timer bravoTimer;

	public override void OnSpawn()
	{
		base.OnSpawn();
		AddStrategy(gameObject.AddComponent<StrategySpace>());
		AddStrategy(gameObject.AddComponent<StrategyBallHawk>());
        AddStrategy(gameObject.AddComponent<StrategyOffenseScore>());
        energyLevel = 12f;
        maxEnergyLevel = 12f;
        recoverEnergyTimer = new Timer(1f, true);
        alphaTimer = new Timer(1f, true);
        bravoTimer = new Timer(1f, true);
    }

	void AddStrategy(AgentStrategy strategy)
	{
		strategyList.Add(strategy);
		strategy.actor = actor;
		strategy.source = actor.transform;
	}

	public void Update()
	{
		float deltaTime = Time.deltaTime;
		InputClear();

		if( !Boss.Instance.IsInGame )
		{
			return;
		}
		Reevaluate(deltaTime);
		Scan(deltaTime, false);
		Decide(deltaTime, false);

		Vector2 moveDir = Vector2.zero;
		float energyConsumed = 0f;
		if( energyLevel > 0f )
		{
			strategy.GetMove(ref moveDir, ref energyConsumed);
			int alphaDice = 0;
			if( alphaTimer.Tick(deltaTime) ) 
			{
				alphaTimer.SetDuration(UnityEngine.Random.Range(1f,3f));
				alphaDice = UnityEngine.Random.Range(1,6);
			} 
			int bravoDice = 0;
			if( bravoTimer.Tick(deltaTime) ) 
			{
				bravoTimer.SetDuration(UnityEngine.Random.Range(1f,3f));
				bravoDice = UnityEngine.Random.Range(1,6);
			}
			
			InputMove(moveDir.x,moveDir.y);	
        
			InputAlpha = strategy.GetAlpha(alphaDice, ref energyConsumed);

			InputBravo = strategy.GetBravo(bravoDice, ref energyConsumed);
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