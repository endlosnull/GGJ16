using UnityEngine;
using System.Collections.Generic;

public class ActionSequence : System.ICloneable
{
	public List<GameAction> actions = new List<GameAction>();

	public float TotalDuration
	{
		get
		{
			float duration = 0f;
			for (int i = 0; i < actions.Count; ++i)
			{
				duration += actions[i].duration;
			}
			return duration;
		}
	}

	public object Clone()
	{
		ActionSequence other = new ActionSequence();
		other.actions.Clear();
		foreach(GameAction action in this.actions)
		{
			other.actions.Add(action.Clone() as GameAction);
		}
		return other;
	}
}
