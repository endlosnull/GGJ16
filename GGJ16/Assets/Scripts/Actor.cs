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
		Debug.Log("Alpha!");
		body.SetColor(Color.blue, 1f);
	}

	public void DoActionBravo()
	{
		Debug.Log("Bravo!");
		body.SetColor(Color.green, 1f);
	}
}