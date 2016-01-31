using System;
using UnityEngine;
using UnityEngine.UI;

public class SequenceHud : MonoBehaviour
{
	public ActionSequencer sequencer;
	public int PlayerIndex;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if(sequencer)
		{
			if (sequencer.CurrentAction != null)
			{
				GameObject.Find("Ritual_" + PlayerIndex).SetActive(true);

				string name = string.Format("Ritual_{0}/ButtonLabel_{1}", 0, sequencer.SequenceIndex);
				GameObject.Find(name).GetComponent<Text>().text = "seq " + sequencer.CurrentAction.name;
			}
		}
		else
		{
			GameObject.Find("Ritual_" + PlayerIndex).SetActive(false);
		}
	}
}
