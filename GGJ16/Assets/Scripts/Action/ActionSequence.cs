using UnityEngine;
using System.Collections.Generic;

public class ActionSequence
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
}
