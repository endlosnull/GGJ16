using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class CamControl : Singleton<CamControl>
{
	public Transform target;
	Vector3 targetPosition = Vector3.zero;
	Vector3 currentPosition;
	Vector3 desiredPosition;
	float maxVelocity = 1.2f;
	public float distance = 22;	
	float pitch = 60f*Mathf.PI/180f;
	float minX = -2.5f;
	float maxX = 2.5f;
	float minZ = -1.5f;
	float maxZ = 1.5f;

	void Awake()
	{
	}

	public void Update()
	{
		if( target != null )
		{
			float deltaTime = Time.deltaTime;
			currentPosition = this.transform.position;
			targetPosition = target.position;


			desiredPosition.x = Mathf.Clamp(targetPosition.x,minX,maxX);
			desiredPosition.y = Mathf.Clamp(targetPosition.y,0f,1f);
			desiredPosition.z = Mathf.Clamp(targetPosition.z,minZ,maxZ);
			desiredPosition.y += distance*Mathf.Sin(pitch);
			desiredPosition.z += -distance*Mathf.Cos(pitch);

			Vector3 diff = desiredPosition-currentPosition;
			float maxStep = maxVelocity * deltaTime;
			if( diff.sqrMagnitude > maxStep*maxStep)
			{
				diff = diff.normalized*maxStep;
			}
			currentPosition += diff;
			this.transform.position = currentPosition;
		}
	}
}
