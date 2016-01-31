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

	void OnSpawn()
	{
		for(int i=0;i<2;++i)
		{
			sequences.Add(new ActionSequence());
		}
        // TEST SEQUENCE
        int seqInd = 0;
		sequences[seqInd].actions.Clear();
        //ActionBricks.AddSwat(gameObject, sequences[seqInd]);
        //ActionBricks.AddGrab(gameObject, sequences[seqInd]);
        //ActionBricks.AddBackDash(gameObject, sequences[seqInd]);
        //ActionBricks.AddJump(gameObject, sequences[seqInd]);
        //ActionBricks.AddTurnRight(gameObject, sequences[seqInd]);
        //ActionBricks.AddThrow(gameObject, sequences[seqInd]);
        ActionBricks.AddForwardDash(gameObject, sequences[seqInd]);
        ActionBricks.AddGrab(gameObject, sequences[seqInd]);

        seqInd++;
        sequences[1].actions.Clear();
        ActionBricks.AddSwat(gameObject, sequences[seqInd]);
        //ActionBricks.AddTurnRight(gameObject, sequences[seqInd]);
        ActionBricks.AddJump(gameObject, sequences[seqInd]);
        ActionBricks.AddThrow(gameObject, sequences[seqInd]);
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
