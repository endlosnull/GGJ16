        using UnityEngine;
        using System.Collections.Generic;

        public class User : MonoBehaviour
        {
        public List<ActionSequence> sequences = new List<ActionSequence>();

        public Actor controlledActor;

        public string inputPrefix = "Key";
        public bool isLocalHuman = true;
        public int teamIndex = 0;

        public List<string> sequenceString = new List<string>(); 
        void Awake()
        {
                for(int i=0;i<2;++i)
                {
                        sequences.Add(new ActionSequence());
                	sequenceString.Add("-");
                }
                sequences[0].actions.Clear();
                sequences[1].actions.Clear();
        }

        public void FillInEmptySequences()
        {
                int seqInd = 0;
                if( sequences[seqInd].actions.Count == 0 )
                {
                        ActionBricks.AddForwardDash(null, sequences[seqInd]);
                        ActionBricks.AddGrab(null, sequences[seqInd]);
                }

                seqInd++;
                if( sequences[seqInd].actions.Count == 0 )
                {
                        ActionBricks.AddSwat(null, sequences[seqInd]);
                        ActionBricks.AddJump(null, sequences[seqInd]);
                        ActionBricks.AddThrow(null, sequences[seqInd]);
                }
        }

        public void UpdateMethods()
        {
                for(int i=0;i<2;++i)
                {
                        sequenceString[i] = "";
                        foreach(GameAction action in sequences[i].actions)
                        {
                                sequenceString[i]+=action.name+",";
                        }
                }

        }
}