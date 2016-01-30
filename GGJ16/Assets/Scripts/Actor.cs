using UnityEngine;

public class Actor : MonoBehaviour
{
	public ActorController controller;
	public Engine engine;
	public Body body;
	public MecanimAnimator animator;

	public void Update()
	{

	}

	public void DoActionAlpha()
	{
		ActionSequencer sequencer = GetComponent<ActionSequencer>();
		sequencer.RunSequence(sequencer.sequences[0]);
	}

	public void DoActionBravo()
	{
		Debug.Log("Bravo!");
		body.SetColor(Color.green, 1f);
	}
}