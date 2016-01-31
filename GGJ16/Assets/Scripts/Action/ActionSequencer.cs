using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Pooling;

public class ActionSequencer : MonoBehaviour
{
	public List<ActionSequence> sequences = new List<ActionSequence>();

	bool active;
	ActionSequence currentSequence;
	public int ActionIndex;
	public int SequenceIndex;

	
	public GameAction CurrentAction {
		get
		{
			if (currentSequence != null && ActionIndex < currentSequence.actions.Count)
			{
				return currentSequence.actions[ActionIndex];
			}
			return null;
		}
	}

    public void LoadSequence(List<ActionSequence> other)
    {
    	sequences.Clear();
    	for(int i=0;i<other.Count;++i)
		{
			foreach(GameAction action in other[i].actions)
			{
				Debug.Log("action src"+action);
			}
			ActionSequence seq = other[i].Clone() as ActionSequence;
			foreach(GameAction action in seq.actions)
			{
				action.source = gameObject;
				Debug.Log("action dst"+action);
			}
			sequences.Add(seq);
		}
    }

    public void AddSequence(ActionSequence sequence)
	{
		sequences.Add(sequence);
	}

	public void RemoveSequence(ActionSequence sequence)
	{
		sequences.Remove(sequence);
	}

	public void RunSequence(int sequenceIndex)
	{
		if (active)
		{
			return;
		}

		SequenceIndex = sequenceIndex;
		OnSequenceStart(sequences[SequenceIndex]);
	}

	void OnSequenceStart(ActionSequence sequence)
	{
		active = true;
		currentSequence = sequence;
		ActionIndex = 0;
	}

	void OnSequenceEnd()
	{
		active = false;
	}

	void Update()
	{
		if (!active)
		{
			return;
		}

		if (currentSequence == null)
		{
			return;
		}

		if (ActionIndex >= currentSequence.actions.Count)
		{
			OnSequenceEnd();
			return;
		}

		float deltaTime = Time.deltaTime;
		GameAction action = currentSequence.actions[ActionIndex];
		if (action == null)
		{
			return;
		}
		if (!action.IsActive)
		{
			action.Invoke();
		}
		if (action.OnTick(deltaTime))
		{
			++ActionIndex;
		}
	}
}
