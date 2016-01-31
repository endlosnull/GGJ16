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

	void OnSpawn()
	{
		for(int i=0;i<2;++i)
		{
			sequences.Add(new ActionSequence());
		}
		// TEST SEQUENCE
		sequences[0].actions.Clear();
        ActionBricks.AddForwardDash(gameObject, sequences[0]);
        ActionBricks.AddGrab(gameObject, sequences[0]);

        sequences[1].actions.Clear();
        ActionBricks.AddRightDash(gameObject, sequences[1]);
        ActionBricks.AddJump(gameObject, sequences[1]);
        ActionBricks.AddThrow(gameObject, sequences[1]);
    }

    public void AddForwardDash(GameObject gameObject, ActionSequence sequence)
    {
        ForwardDash action = new ForwardDash();
        action.source = gameObject;
        action.force = 3f;
        action.duration = 0.4f;
        sequence.actions.Add(action);
    }

    public void AddRightDash(GameObject gameObject, ActionSequence sequence)
    {
        SideDash action = new SideDash();
        action.source = gameObject;
        action.force = 3f;
        action.duration = 0.4f;
        sequence.actions.Add(action);
    }

    public void AddGrab(GameObject gameObject, ActionSequence sequence)
    {
        Grab action = new Grab();
        action.source = gameObject;
        action.range = 2.0f;
        action.duration = 0.2f;
        sequence.actions.Add(action);
    }

    public void AddJump(GameObject gameObject, ActionSequence sequence)
    {
        Jump action = new Jump();
        action.source = gameObject;
        action.force = 2f;
        action.duration = 0.5f;
        sequence.actions.Add(action);
    }

    public void AddThrow(GameObject gameObject, ActionSequence sequence)
    {
        Throw action = new Throw();
        action.source = gameObject;
        action.forceForward = 3f;
        action.forceUp = 3f;
        action.duration = 0.2f;
        action.actionTime = 0.1f;
        sequence.actions.Add(action);
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
