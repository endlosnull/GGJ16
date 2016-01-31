using UnityEngine;

public class ActorController : MonoBehaviour
{
	public bool locked = false;
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

	public virtual void OnSpawn()
	{
		actor = gameObject.GetComponent<Actor>();	
	}

	public void InputClear()
	{
		actor.inputForce.x = 0f;
        actor.inputForce.y = 0f;
		inputLastAlpha = inputAlpha;
		inputLastBravo = inputBravo;
	}

	public void InputMove(float x, float y)
	{
		if (locked)
		{
			return;
		}
        actor.inputForce.x += x;
        actor.inputForce.y += y;
	}

	public void InputTick(float deltaTime)
	{
		if (locked)
		{
			return;
		}
		if( inputAlpha && !inputLastAlpha )
		{
			actor.DoActionAlpha();
		}
		if( inputBravo && !inputLastBravo )
		{
			actor.DoActionBravo();
		}
	}
}