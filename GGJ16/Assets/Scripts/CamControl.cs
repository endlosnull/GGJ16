using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class CamControl : Singleton<CamControl>
{
	public Transform target;
	public AudioSource audioSource;
	public AudioSource ambientAudioSource;
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

	float shakeIntensity;
	float shakeFadeMod = 0.5f;

	float zoomValue;
	float zoomSavedValue;
	float zoomTarget;
	float zoomTime;
	float zoomDuration;
	bool zoomReset;

	void Awake()
	{
		currentPosition = this.transform.position;
	}

	public void Update()
	{
		if( target != null )
		{
			float deltaTime = Time.deltaTime;

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

			Vector3 shakePos = Vector3.zero;
			if (shakeIntensity > 0f)
			{
				shakePos = Random.insideUnitSphere * shakeIntensity;
				shakeIntensity = Mathf.Max(0f, shakeIntensity - shakeFadeMod * deltaTime);
			}

			Vector3 zoomPos = Vector3.zero;
			if (zoomDuration > 0f)
			{
				zoomValue = Mathf.Lerp(zoomSavedValue, zoomTarget, Mathf.Clamp01(zoomTime / zoomDuration));

				zoomPos = this.transform.forward * zoomValue;

				zoomTime += deltaTime;

				if (zoomTime >= zoomDuration)
				{
					if (!zoomReset)
					{
						ResetZoom(0.5f);
					}
					else
					{
						ClearZoom();
					}
				}
			}

			this.transform.position = currentPosition + shakePos + zoomPos;
		}
	}

	public void AddShake(float intensity)
	{
		shakeIntensity += intensity;
	}

	public void AddZoom(float target, float duration)
	{
		zoomSavedValue = zoomValue;
		zoomTarget = target;
		zoomTime = 0f;
		zoomDuration = duration;
		zoomReset = false;
	}

	public void ResetZoom(float duration)
	{
		zoomSavedValue = zoomValue;
		zoomTarget = 0f;
		zoomTime = 0f;
		zoomDuration = duration;
		zoomReset = true;
	}

	void ClearZoom()
	{
		zoomValue = 0f;
		zoomSavedValue = 0f;
		zoomTarget = 0f;
		zoomTime = 0f;
		zoomDuration = 0f;
		zoomReset = false;
	}
}
