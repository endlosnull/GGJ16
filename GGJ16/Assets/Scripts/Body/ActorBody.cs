using UnityEngine;
using System.Collections.Generic;

public class ActorBody : Body
{
	public Renderer vfxRenderer;
	Color defaultColor =  new Color(0f,0f,0f,0.3f);
	Timer colorTimer = new Timer();	
	float maxAlpha = 0.3f;
	

	void Awake()
	{
		ResetShadow();
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
}