using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionBricks
{
    static public void AddForwardDash(GameObject gameObject, ActionSequence sequence)
    {
        ForwardDash action = new ForwardDash();
        action.source = gameObject;
        action.force = 6f;
        action.duration = 0.4f;
        sequence.actions.Add(action);
    }

    static public void AddRightDash(GameObject gameObject, ActionSequence sequence)
    {
        SideDash action = new SideDash();
        action.source = gameObject;
        action.force = 3f;
        action.left = false;
        action.duration = 0.4f;
        sequence.actions.Add(action);
    }

    static public void AddLeftDash(GameObject gameObject, ActionSequence sequence)
    {
        SideDash action = new SideDash();
        action.source = gameObject;
        action.force = 3f;
        action.left = true;
        action.duration = 0.4f;
        sequence.actions.Add(action);
    }

    static public void AddGrab(GameObject gameObject, ActionSequence sequence)
    {
        Grab action = new Grab();
        action.source = gameObject;
        action.range = 2.0f;
        action.duration = 0.2f;
        sequence.actions.Add(action);
    }

    static public void AddJump(GameObject gameObject, ActionSequence sequence)
    {
        Jump action = new Jump();
        action.source = gameObject;
        action.force = 5f;
        action.duration = 0.5f;
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
        sequence.actions.Add(action);
    }
}
