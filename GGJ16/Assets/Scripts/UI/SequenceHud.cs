using System;
using UnityEngine;
using UnityEngine.UI;

public class SequenceHud : MonoBehaviour
{
	public ActionSequencer sequencer;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if(sequencer)
		{
			// Which sequence
			//sequencer.SequenceIndex;
			// Which index
			//sequencer.ActionIndex
			string name = string.Format("Ritual_{0}/ButtonLabel_{1}", 0, 0);
			GameObject.Find(name).GetComponent<Text>().text = "AA";
		}
	}
}
