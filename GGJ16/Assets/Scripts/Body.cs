using UnityEngine;
using System.Collections.Generic;

public class Body : MonoBehaviour
{

	public Renderer torsoRenderer;
	public List<GameObject> attachments = new List<GameObject>();
	public Color defaultColor = Color.white;
	Timer colorTimer;

	Quaternion defaultTorsoRotation = Quaternion.identity;
	float skew = 10f;

	
	public void OnSpawn()
	{
		torsoRenderer = gameObject.GetComponent<Renderer>();
		colorTimer = new Timer();
		defaultTorsoRotation = torsoRenderer.transform.rotation;
	}

	public void Update()
	{
		torsoRenderer.transform.localRotation = defaultTorsoRotation * Quaternion.AngleAxis(skew, Vector3.forward);
		//Debug.Log("Rot"+torsoRenderer.transform.localRotation+" "+skew+" "+this);
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