using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionBricks
{
    static public void QueuePop(ActionSequence sequence, int max)
    {
        while(sequence.actions.Count > max)
        {
            sequence.actions.RemoveAt(0);
        }
    }

    static public void AddForwardDash(GameObject gameObject, ActionSequence sequence)
    {
        ForwardDash action = new ForwardDash();
        action.source = gameObject;
        action.force = 6f;
        action.duration = 0.4f;
        QueuePop(sequence,3);
        sequence.actions.Add(action);
    }

    static public void AddBackDash(GameObject gameObject, ActionSequence sequence)
    {
        BackDash action = new BackDash();
        action.source = gameObject;
        action.force = 6f;
        action.duration = 0.4f;
        QueuePop(sequence,3);
        sequence.actions.Add(action);
    }

    static public void AddRightDash(GameObject gameObject, ActionSequence sequence)
    {
        SideDash action = new SideDash();
		action.name = "left dash";
        action.source = gameObject;
        action.force = 3f;
        action.left = false;
        action.duration = 0.4f;
        QueuePop(sequence,3);
        sequence.actions.Add(action);
    }

    static public void AddLeftDash(GameObject gameObject, ActionSequence sequence)
    {
        SideDash action = new SideDash();
		action.name = "right dash";
		action.source = gameObject;
        action.force = 3f;
        action.left = true;
        action.duration = 0.4f;
        QueuePop(sequence,3);
        sequence.actions.Add(action);
    }

    static public void AddSwat(GameObject gameObject, ActionSequence sequence)
    {
        Swat action = new Swat();
        action.source = gameObject;
        action.range = 2.0f;
        action.yRange = 1.0f;
        action.duration = 0.2f;
        QueuePop(sequence,3);
        sequence.actions.Add(action);
    }

    static public void AddGrab(GameObject gameObject, ActionSequence sequence)
    {
        Grab action = new Grab();
        action.source = gameObject;
        action.range = 2.0f;
        action.duration = 0.2f;
        QueuePop(sequence,3);
        sequence.actions.Add(action);
    }

    static public void AddJump(GameObject gameObject, ActionSequence sequence)
    {
        Jump action = new Jump();
        action.source = gameObject;
        action.force = 5f;
        action.duration = 0.5f;
        QueuePop(sequence,3);
        sequence.actions.Add(action);
    }

    static public void AddThrow(GameObject gameObject, ActionSequence sequence)
    {
        Throw action = new Throw();
        action.source = gameObject;
        action.forceForward = 3f;
        action.forceUp = 3f;
        action.duration = 0.2f;
        action.actionTime = 0.1f;
        QueuePop(sequence,3);
        sequence.actions.Add(action);
    }

    static public void AddTurnAround(GameObject gameObject, ActionSequence sequence)
    {
        Turn action = new Turn();
        action.name = "turn around";
        action.source = gameObject;
        action.angleDegrees = 180;
        action.actionTime = 0.1f;
        QueuePop(sequence,3);
        sequence.actions.Add(action);
    }

    static public void AddTurnRight(GameObject gameObject, ActionSequence sequence)
    {
        Turn action = new Turn();
        action.name = "turn right";
        action.source = gameObject;
        action.angleDegrees = 90;
        action.actionTime = 0.1f;
        QueuePop(sequence,3);
        sequence.actions.Add(action);
    }

    static public void AddTurnLeft(GameObject gameObject, ActionSequence sequence)
    {
        Turn action = new Turn();
        action.name = "turn left";
        action.source = gameObject;
        action.angleDegrees = 90;
        action.actionTime = 0.1f;
        QueuePop(sequence,3);
        sequence.actions.Add(action);
    }
}
