using UnityEngine;

public class ActorController : MonoBehaviour
{

	Vector2 inputMove = Vector2.zero;
	bool inputLastAlpha = false;
	bool inputAlpha = false;
	public bool InputAlpha { 
		set { inputAlpha = value; }
	}
	bool inputLastBravo = false;
	bool inputBravo = false;
	public bool InputBravo { 
		set { inputBravo = value; }
	}

	public Actor actor;
	public Engine engine;

	public void OnSpawn()
	{
		actor = gameObject.GetComponent<Actor>();	
		engine = gameObject.GetComponent<Engine>();	
	}

	public void Update()
	{

	}

	public void InputClear()
	{
		engine.inputForce.x = 0f;
		engine.inputForce.y = 0f;
		inputLastAlpha = inputAlpha;
		inputLastBravo = inputBravo;
	}

	public void InputMove(float x, float y)
	{
		engine.inputForce.x += x;
		engine.inputForce.y += y;
	}

	public void InputTick(float deltaTime)
	{
		engine.Tick(deltaTime);
	}


}