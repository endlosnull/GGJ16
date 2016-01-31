using UnityEngine;
using System.Collections.Generic;

public class ActorBody : Body
{
	public Renderer bodyRenderer;
	public Renderer vfxRenderer;
	Color defaultColor =  new Color(0f,0f,0f,0.3f);
	Timer colorTimer = new Timer();	
	float maxAlpha = 0.3f;

	Actor actor;

	// Animation
	public MecanimAnimator animator;
	int layerArms = 1;
	int ctrlIdMoveSpeed;
	int ctrlIdHold;
	int ctrlIdThrow;
	int tagThrow;

	void Reset()
	{
		animator = GetComponentInChildren<MecanimAnimator>();
		
	}

	void Awake()
	{
		ResetShadow();

		Transform bodyTransform = transform.FindTransformInChildren("Mesh");
		if( bodyTransform )
		{
			bodyRenderer = bodyTransform.GetComponent<Renderer>();
		}

		if (animator != null)
		{
			ctrlIdMoveSpeed = Animator.StringToHash("MoveSpeed");
			ctrlIdHold = Animator.StringToHash("Hold");
			ctrlIdThrow = Animator.StringToHash("Throw");

			tagThrow = Animator.StringToHash("Tag");

			animator.RegisterOnTagEnd(tagThrow, delegate {
				SetAnimatorArmWeight(0f);
			});
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

	public void SetTexture(Texture tex)
	{
		if( bodyRenderer )
		{
			bodyRenderer.material.SetTexture("_MainTex",tex);
		}
	}

	

	public void SetShadowColor(Color color, float duration)
	{
		Color nextColor = new Color(color.r,color.g,color.b, Mathf.Min(maxAlpha,color.a));
		vfxRenderer.material.color = nextColor; 
		colorTimer.SetDuration(duration);
	}

	public void SetAnimatorMoveSpeed(float value)
	{
		animator.SetFloat(ctrlIdMoveSpeed, value);
	}

	public void SetAnimatorHold(bool value)
	{
		animator.SetBool(ctrlIdHold, value);
		SetAnimatorArmWeight((value ? 1f : 0f));
	}

	public void SetAnimatorThrow()
	{
		animator.SetTrigger(ctrlIdThrow);
		SetAnimatorArmWeight(1f);
	}

	public void SetAnimatorArmWeight(float value)
	{
		animator.SetLayerWeight(layerArms, value);
	}
}