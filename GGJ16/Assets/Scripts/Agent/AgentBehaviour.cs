using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct BehaviourContext
{
	public Transform goal;
	public float distToGoal;
	public Transform ball;
	public float distToBall;
	public bool isDefense;
	public bool isOffense;
	public bool hasBall;
}

public class AgentBehaviour : MonoBehaviour
{
	public Actor actor;
	public Transform source;
	protected Vector2 sourcePos = Vector2.zero;
	public Transform target;
	protected Vector2 targetPos = Vector2.zero;
	protected Vector2 targetOffset = Vector2.zero;
	public BehaviourContext context;

	public void Dispose()
	{

	}

	public virtual void Decide()
	{
		
	}

	public Vector2 GetMove()
	{
		if( target != null )
		{
			targetPos = new Vector2(target.position.x, target.position.z);
		}
		sourcePos = new Vector2(source.position.x, source.position.z);
		Vector2 diff = (targetPos + targetOffset-sourcePos);
		if( diff.magnitude < 0.5f )
		{
			return Vector2.zero;
		}
		return diff.normalized;

	}
	public virtual bool GetAlpha()
	{
		return false;
	}
	public virtual bool GetBravo()
	{
		return false;
	}

	public virtual int GetGoodness()
	{
		return 0;
	}
}

public class BehavSpace : AgentBehaviour
{
	public override void Decide()
	{
		target = null;
		targetPos = actor.team.GetLeashPos(actor.positionIndex);
		
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

public class BehavBallHawk : AgentBehaviour
{
	public override void Decide()
	{
		target = context.ball;
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


public class BehavOffenseScore : AgentBehaviour
{
	public override void Decide()
	{
		target = context.goal;
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


public class BehavDefendActor : AgentBehaviour
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


public class BehavOffenseHelp : AgentBehaviour
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

public class BehavOffenseAdvance : AgentBehaviour
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