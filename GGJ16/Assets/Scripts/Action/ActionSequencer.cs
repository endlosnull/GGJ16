using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Pooling;

public class ActionSequencer : MonoBehaviour
{
	public List<ActionSequence> sequences = new List<ActionSequence>();

	bool active;
	ActionSequence currentSequence;
	int sequenceIndex;

	void OnSpawn()
	{
		// TEST SEQUENCE
		ActionSequence sequence = new ActionSequence();

        Grab grabAction = new Grab();
        grabAction.source = gameObject;
        grabAction.range = 2.0f;
        grabAction.duration = 0.2f;
        sequence.actions.Add(grabAction);

        ForwardDash forwardDash = new ForwardDash();
        forwardDash.source = gameObject;
        forwardDash.force = 3f;
        forwardDash.duration = 0.4f;
        sequence.actions.Add(forwardDash);

        Jump jump = new Jump();
        jump.source = gameObject;
        jump.force = 2f;
        jump.duration = 0.5f;
        sequence.actions.Add(jump);

        Throw throwAction = new Throw();
        throwAction.source = gameObject;
        throwAction.forceForward = 3f;
        throwAction.forceUp = 3f;
        throwAction.duration = 0.2f;
		throwAction.actionTime = 0.1f;
        sequence.actions.Add(throwAction);

        //SideDash sideDash = new SideDash();
        //sideDash.source = gameObject;
        //sideDash.force = 3f;
        //sideDash.duration = 0.2f;
        //sequence.actions.Add(sideDash);

        //ForwardDash forwardDash2 = new ForwardDash();
        //forwardDash2.source = gameObject;
        //forwardDash2.force = 1.5f;
        //forwardDash2.duration = 0.33f;
        //sequence.actions.Add(forwardDash2);

        AddSequence(sequence);
	}

	public void AddSequence(ActionSequence sequence)
	{
		sequences.Add(sequence);
	}

	public void RemoveSequence(ActionSequence sequence)
	{
		sequences.Remove(sequence);
	}

	public void RunSequence(ActionSequence sequence)
	{
		if (active)
		{
			return;
		}

		OnSequenceStart(sequence);
	}

	void OnSequenceStart(ActionSequence sequence)
	{
		active = true;
		currentSequence = sequence;
		sequenceIndex = 0;
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

		if (sequenceIndex >= currentSequence.actions.Count)
		{
			OnSequenceEnd();
			return;
		}

		float deltaTime = Time.deltaTime;
		GameAction action = currentSequence.actions[sequenceIndex];
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
			++sequenceIndex;
		}
	}
}
