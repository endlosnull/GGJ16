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
		for(int i=0;i<2;++i)
		{
			sequences.Add(new ActionSequence());
		}
		// TEST SEQUENCE
		sequences[0].actions.Clear();

        ForwardDash forwardDash = new ForwardDash();
        forwardDash.source = gameObject;
        forwardDash.force = 3f;
        forwardDash.duration = 0.4f;
        sequences[0].actions.Add(forwardDash);

        Grab grabAction = new Grab();
        grabAction.source = gameObject;
        grabAction.range = 2.0f;
        grabAction.duration = 0.2f;
        sequences[0].actions.Add(grabAction);

        sequences[1].actions.Clear();

		SideDash sideDash = new SideDash();
        sideDash.source = gameObject;
        sideDash.force = 3f;
        sideDash.duration = 0.2f;
        sequences[1].actions.Add(sideDash);

        Jump jump = new Jump();
        jump.source = gameObject;
        jump.force = 2f;
        jump.duration = 0.5f;
        sequences[1].actions.Add(jump);

        Throw throwAction = new Throw();
        throwAction.source = gameObject;
        throwAction.forceForward = 3f;
        throwAction.forceUp = 3f;
        throwAction.duration = 0.2f;
		throwAction.actionTime = 0.1f;
        sequences[1].actions.Add(throwAction);

        //ForwardDash forwardDash2 = new ForwardDash();
        //forwardDash2.source = gameObject;
        //forwardDash2.force = 1.5f;
        //forwardDash2.duration = 0.33f;
        //sequence.actions.Add(forwardDash2);

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
