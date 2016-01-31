using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct StrategyContext
{
	public Transform goal;
	public float distToGoal;
	public Transform ball;
	public float distToBall;
	public bool isDefense;
	public bool isOffense;
	public bool hasBall;
}

public class AgentStrategy : MonoBehaviour
{
	public Actor actor;
	public Transform source;
	protected Vector2 sourcePos = Vector2.zero;
	public Transform target;
	protected Vector2 targetPos = Vector2.zero;
	protected Vector2 targetOffset = Vector2.zero;
	public StrategyContext context;

	public void Dispose()
	{

	}

	public virtual void Decide()
	{
		
	}

	public void GetMove(ref Vector2 direction, ref float energyDrain)
	{
		if( target != null )
		{
			targetPos = new Vector2(target.position.x, target.position.z);
		}
		sourcePos = new Vector2(source.position.x, source.position.z);
		Vector2 diff = (targetPos + targetOffset-sourcePos);
		if( diff.magnitude < 0.5f )
		{
			direction = Vector2.zero;
			energyDrain += 0f;
		}
		else
		{
			direction = diff.normalized;
			energyDrain += 1f;
		} 

	}
	public virtual bool GetAlpha(int rand, ref float energyDrain)
	{
		energyDrain += 0f;
		return false;
	}
	public virtual bool GetBravo(int rand, ref float energyDrain)
	{
		energyDrain += 0f;
		return false;
	}

	public virtual int GetGoodness()
	{
		return 0;
	}
}

public class StrategySpace : AgentStrategy
{
	public override void Decide()
	{
		target = null;
		targetPos = actor.team.GetLeashPos(actor.positionIndex);
		
	}

	public override bool GetAlpha(int rand, ref float energyDrain)
	{
		if( rand == 1 || rand == 2 )
		{
			energyDrain += 2f;
			return true;
		}
		else
		{
			energyDrain += 0f;
			return false;
		}
	}
	public override bool GetBravo(int rand, ref float energyDrain)
	{
		energyDrain += 0f;
		return false;
	}
	public override int GetGoodness()
	{
		if( context.distToBall > 4 )
		{
			return 20;
		}
		else
		{
			return 1;
		}
	}
}

public class StrategyBallHawk : AgentStrategy
{
	public override void Decide()
	{
		target = context.ball;
	}

	public override bool GetAlpha(int rand, ref float energyDrain)
	{
		if( rand == 1 || rand == 2 )
		{
			energyDrain += 2f;
			return true;
		}
		else
		{
			energyDrain += 0f;
			return false;
		}
	}
	public override bool GetBravo(int rand, ref float energyDrain)
	{
		if( rand == 1 || rand == 2 )
		{
			energyDrain += 2f;
			return true;
		}
		else
		{
			energyDrain += 0f;
			return false;
		}
	}
	public override int GetGoodness()
	{
		if( actor.team.bestBallHawk == actor )
		{
			return 100;
		}
		else
		{
			return -100;
		}
	}
}


public class StrategyOffenseScore : AgentStrategy
{
	public override void Decide()
	{
		target = context.goal;
	}

	public override bool GetAlpha(int rand, ref float energyDrain)
	{
		if( rand == 1  )
		{
			energyDrain += 2f;
			return true;
		}
		else
		{
			energyDrain += 0f;
			return false;
		}
	}
	public override bool GetBravo(int rand, ref float energyDrain)
	{
		if( rand == 1 || rand == 2 || rand == 3 )
		{
			energyDrain += 2f;
			return true;
		}
		else
		{
			energyDrain += 0f;
			return false;
		}
	}
	public override int GetGoodness()
	{
		if( context.hasBall )
		{
			return 200;
		}
		else
		{
			return -10;
		}
	}
}

/*


public class StrategyDefendActor : AgentStrategy
{
	public override void Decide()
	{
		
	}

	public virtual bool GetAlpha()
	{
		return false;
	}
	public virtual bool GetBravo()
	{
		return false;
	}

	public override int GetGoodness()
	{
		if( context.isOffense || context.hasBall )
		{
			return -30;
		}
		if( context.distToBall < 4 )
		{
			return 20;
		}
		else
		{
			return 0;
		}
	}
}


public class StrategyOffenseHelp : AgentStrategy
{
	Vector2 targetOffset = Vector2.zero;


	public override void Decide()
	{
		target = Field.Instance.ball.transform;
		targetPos = new Vector2(target.position.x, target.position.z);
		if( sourcePos.y < targetPos.y )
		{
			targetOffset = new Vector2(0,-20f);
		}
		else
		{
			targetOffset = new Vector2(0,20f);
		}
	}

	public virtual bool GetAlpha()
	{
		return false;
	}
	public virtual bool GetBravo()
	{
		return false;
	}
	public override int GetGoodness()
	{
		if( context.isDefense )
		{
			return -10;
		}
		if( !context.hasBall && context.distToBall < 20 )
		{
			return 20;
		}
		else
		{
			return -10;
		}
	}
}

public class StrategyOffenseAdvance : AgentStrategy
{


	public override void Decide()
	{
		Transform goalTransform = Field.Instance.goal.transform;
		Vector2 goalPos = new Vector2(goalTransform.position.x, goalTransform.position.z);
		targetPos = sourcePos;
		if( sourcePos.x < goalPos.x )
		{
			targetPos.x = Mathf.Min(sourcePos.x + 20, goalPos.x);
		}
		else
		{
			targetPos.x = Mathf.Max(sourcePos.x - 20, goalPos.x);
		}
	}


	public virtual bool GetAlpha()
	{
		return false;
	}
	public virtual bool GetBravo()
	{
		return false;
	}
	public override int GetGoodness()
	{
		if( context.isDefense )
		{
			return -10;
		}
		if( context.hasBall && context.distToGoal > 20 )
		{
			return 20;
		}
		else
		{
			return -10;
		}
	}
}

*/