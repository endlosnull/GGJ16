using UnityEngine;
using GGJ16;

public class Body : MonoBehaviour
{

	public Body body;
	public Renderer torsoRenderer;
	public Color defaultColor = Color.white;
	Timer colorTimer;
	
	public void OnSpawn()
	{
		torsoRenderer = gameObject.GetComponent<Renderer>();
		colorTimer = new Timer();
	}

	public void Update()
	{
		float deltaTime = Time.deltaTime;
		if( colorTimer.Tick(deltaTime) )
		{
			torsoRenderer.material.color = defaultColor; 
		}
	}

	public void SetColor(Color color, float duration)
	{
		torsoRenderer.material.color = color; 
		colorTimer.SetDuration(duration);
	}
}