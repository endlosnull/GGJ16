using UnityEngine;
using System.Collections.Generic;

public struct BehaviourContext
{
	public Vector2 goalPos;
	public float distToGoal;
	public Vector2 ballPos;
	public float distToBall;
	public bool isDefense;
	public bool hasBall;
}

public class AgentBehaviour : System.IDisposable
{
	public Vector2 homePos;
	public int sourceTeamIndex;
	public Transform source;
	public Vector2 sourcePos;
	public Transform target;
	public Vector2 targetPos;

	public void Dispose()
	{

	}

	public virtual void Scan()
	{
		
	}

	public virtual Vector2 GetMove()
	{
		if( target != null )
		{
			targetPos = new Vector2(target.position.x, target.position.z);
		}
		sourcePos = new Vector2(source.position.x, source.position.z);
		return (targetPos-sourcePos).normalized;
	}
	public virtual bool GetAlpha()
	{
		return false;
	}
	public virtual bool GetBravo()
	{
		return false;
	}

	public virtual int GetGoodness(BehaviourContext context)
	{
		return 0;
	}
}

public class BehavDefendGoal : AgentBehaviour
{
	

	public override void Scan()
	{
		target = Field.Instance.goal.transform;
	}


	public override bool GetAlpha()
	{
		return false;
	}
	public override bool GetBravo()
	{
		return false;
	}
	public override int GetGoodness(BehaviourContext context)
	{
		if( !context.isDefense )
		{
			return -10;
		}
		if( context.distToBall > 20 )
		{
			return 10;
		}
		else
		{
			return -10;
		}
	}
}

public class BehavDefendSpace : AgentBehaviour
{
	public override void Scan()
	{
		target = null;
		targetPos = homePos;
	}

	public virtual bool GetAlpha()
	{
		return false;
	}
	public virtual bool GetBravo()
	{
		return false;
	}
	public override int GetGoodness(BehaviourContext context)
	{
		if( !context.isDefense )
		{
			return -10;
		}
		if( context.distToBall > 20 && context.distToGoal > 20 )
		{
			return 20;
		}
		else
		{
			return -10;
		}
	}
}

public class BehavDefendNearbyActor : AgentBehaviour
{
	public override void Scan()
	{
		List<Actor> allActors = Field.Instance.allActors;
		float bestDiff = 99999f*99999f;
		for(int i=0; i<allActors.Count; ++i)
		{
			if(allActors[i].team.teamIndex != sourceTeamIndex )
			{
				float sqrDiff = (allActors[i].transform.position - source.position).sqrMagnitude;
				if( sqrDiff < bestDiff )
				{
					target = allActors[i].transform;
				}
			}
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

	public override int GetGoodness(BehaviourContext context)
	{
		if( context.isDefense )
		{
			return -10;
		}
		if( context.distToBall > 20 )
		{
			return 20;
		}
		else
		{
			return 2;
		}
	}
}

public class BehavDefendNearbyBall : AgentBehaviour
{
	public override void Scan()
	{
		target = Field.Instance.ball.transform;
	}

	public virtual bool GetAlpha()
	{
		return false;
	}
	public virtual bool GetBravo()
	{
		return false;
	}
	public override int GetGoodness(BehaviourContext context)
	{
		if( context.isDefense )
		{
			return -10;
		}
		if( context.distToBall < 20 )
		{
			return 20;
		}
		else
		{
			return 2;
		}
	}
}

public class BehavOffenseHelp : AgentBehaviour
{
	Vector2 targetOffset = Vector2.zero;

	public override Vector2 GetMove()
	{
		if( target != null )
		{
			targetPos = new Vector2(target.position.x, target.position.z);
		}
		sourcePos = new Vector2(source.position.x, source.position.z);
		return (targetPos + targetOffset-sourcePos).normalized;
	}

	public override void Scan()
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
	public override int GetGoodness(BehaviourContext context)
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


	public override void Scan()
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
	public override int GetGoodness(BehaviourContext context)
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


public class BehavOffenseScore : AgentBehaviour
{
	public override void Scan()
	{
		target = Field.Instance.goal.transform;
	}

	public virtual bool GetAlpha()
	{
		return false;
	}
	public virtual bool GetBravo()
	{
		return false;
	}
	public override int GetGoodness(BehaviourContext context)
	{
		if( context.isDefense )
		{
			return -10;
		}
		if( context.hasBall && context.distToGoal < 20 )
		{
			return 20;
		}
		else
		{
			return -10;
		}
	}
}
