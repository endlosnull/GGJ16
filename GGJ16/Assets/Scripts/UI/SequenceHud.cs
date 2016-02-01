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
				string textName = string.Format("Ritual_{0}/ButtonLabel_{1}", PlayerIndex, sequencer.SequenceIndex);
				GameObject.Find(textName).GetComponent<Text>().text = sequencer.CurrentAction.name;
				string imageName = string.Format("Ritual_{0}/Image_{1}", PlayerIndex, sequencer.SequenceIndex);

				if (ActionBricksDictionary.Dictionary.ContainsKey(sequencer.CurrentAction.name)) {
					string imagePath = ActionBricksDictionary.Dictionary[sequencer.CurrentAction.name].image;
					GameObject.Find(imageName).GetComponent<Image>().sprite = Resources.Load<Sprite>(imagePath);
				}
			}
			GameObject.Find("Ritual_" + PlayerIndex).SetActive(true);

			string playerName = string.Format("Ritual_{0}/PlayerName", PlayerIndex);
			GameObject.Find(playerName).GetComponent<Text>().text = "P" + PlayerIndex;
		}
		else
		{
			GameObject.Find("Ritual_" + PlayerIndex).SetActive(false);
		}
	}
}
