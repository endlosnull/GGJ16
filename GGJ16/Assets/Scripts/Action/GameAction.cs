using UnityEngine;
using System.Collections;

public class GameAction
{
	public GameObject source;
	public GameObject target;
	public float duration;
	public float actionTime;

	protected bool active;
	protected float time;
	protected bool usedAction;

	public bool IsActive
	{
		get
		{
			return active;
		}
	}

	public bool IsDone
	{
		get
		{
			return time >= duration;
		}
	}

	public virtual bool Invoke()
	{
		if (active)
		{
			return false;
		}

		return true;
	}

	protected virtual void OnInvokeStart()
	{
		active = true;
		time = 0f;
	}

	protected virtual void OnInvokeEnd()
	{
		active = false;
		usedAction = false;
	}

	protected virtual void OnActionTime()
	{
		usedAction = true;
	}

	public virtual bool OnTick(float deltaTime)
	{
		if (time > actionTime && !usedAction)
		{
			OnActionTime();
		}

		time += deltaTime;
		return time >= duration;
	}
}
