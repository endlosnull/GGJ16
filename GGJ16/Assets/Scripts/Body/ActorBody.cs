using UnityEngine;
using System.Collections.Generic;

public class ActorBody : Body
{
	public Renderer vfxRenderer;
	Color defaultColor =  new Color(0f,0f,0f,0.3f);
	Timer colorTimer = new Timer();	
	float maxAlpha = 0.3f;

	Actor actor;

	// Animation
	public MecanimAnimator animator;
	int ctrlIdMoveSpeed;

	void Reset()
	{
		animator = GetComponentInChildren<MecanimAnimator>();
	}

	void Awake()
	{
		ResetShadow();

		if (animator != null)
		{
			ctrlIdMoveSpeed = animator.GetControlId("MoveSpeed");
		}
	}

	public override void Update()
	{
		base.Update();
		float deltaTime = Time.deltaTime;
		if( colorTimer.Tick(deltaTime) )
		{
			ResetShadow();
		}
	}

	void ResetShadow()
	{
		vfxRenderer.material.color = defaultColor; 
	}

	

	public void SetShadowColor(Color color, float duration)
	{
		Color nextColor = new Color(color.r,color.g,color.b, Mathf.Min(maxAlpha,color.a));
		vfxRenderer.material.color = nextColor; 
		colorTimer.SetDuration(duration);
	}

	public void SetMoveSpeed(float value)
	{
		animator.SetFloat(ctrlIdMoveSpeed, value);
	}
}