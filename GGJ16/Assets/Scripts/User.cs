using UnityEngine;
using System.Collections.Generic;

public class User : MonoBehaviour
{
	public List<ActionSequence> sequences = new List<ActionSequence>();

	public Actor controlledActor;

	public string inputPrefix = "Key";
	public bool isLocalHuman = true;

	void Awake()
	{
		for(int i=0;i<2;++i)
		{
			sequences.Add(new ActionSequence());
		}
		// TEST SEQUENCE
        int seqInd = 0;
		sequences[seqInd].actions.Clear();
        //ActionBricks.AddSwat(null, sequences[seqInd]);
        //ActionBricks.AddGrab(null, sequences[seqInd]);
        //ActionBricks.AddBackDash(null, sequences[seqInd]);
        //ActionBricks.AddJump(null, sequences[seqInd]);
        //ActionBricks.AddTurnRight(null, sequences[seqInd]);
        //ActionBricks.AddThrow(null, sequences[seqInd]);
        ActionBricks.AddForwardDash(null, sequences[seqInd]);
        ActionBricks.AddGrab(null, sequences[seqInd]);

        seqInd++;
        sequences[seqInd].actions.Clear();
        ActionBricks.AddSwat(null, sequences[seqInd]);
        //ActionBricks.AddTurnRight(null, sequences[seqInd]);
        ActionBricks.AddJump(null, sequences[seqInd]);
        ActionBricks.AddThrow(null, sequences[seqInd]);
	}

	public void Update()
	{

	}
}