using UnityEngine;

public class Actor : MonoBehaviour
{

	public ActorController controller;
	public Engine engine;
	public Body body;

	public void Update()
	{

	}

	public void DoActionAlpha()
	{
		GGJ16.ActionSequencer sequencer = GetComponent<GGJ16.ActionSequencer>();
		sequencer.RunSequence(sequencer.sequences[0]);
	}

	public void DoActionBravo()
	{
		Debug.Log("Bravo!");
		body.SetColor(Color.green, 1f);
	}
}