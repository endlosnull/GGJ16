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

        public void DefaultAISequences()
        {
                int seqInd = 0;
                sequences[seqInd].actions.Clear();
                ActionBricks.AddForwardDash(null, sequences[seqInd]);
                ActionBricks.AddGrab(null, sequences[seqInd]);

                seqInd++;
                sequences[seqInd].actions.Clear();
                ActionBricks.AddSwat(null, sequences[seqInd]);
                ActionBricks.AddJump(null, sequences[seqInd]);
                ActionBricks.AddThrow(null, sequences[seqInd]);
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