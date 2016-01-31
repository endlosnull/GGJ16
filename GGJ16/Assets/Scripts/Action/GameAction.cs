using UnityEngine;
using System.Collections;

public class GameAction : System.ICloneable
{
	public GameObject source;
	public GameObject target;
	public float duration;
	public float actionTime;

	public string name = "";

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

		active = true;
		time = 0f;
		OnInvokeStart();
		return true;
	}

	protected virtual void OnInvokeStart()
	{
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
		if (!active)
		{
			return false;
		}

		if (time >= actionTime && !usedAction)
		{
			OnActionTime();
		}

		if (time >= duration)
		{
			OnInvokeEnd();
			return true;
		}

		time += deltaTime;
		return false;
	}

	public object Clone()
	{
		return this.MemberwiseClone();
	}
}
