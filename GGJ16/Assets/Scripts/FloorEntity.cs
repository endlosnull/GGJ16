using UnityEngine;
using System.Collections.Generic;

public class FloorEntity : MonoBehaviour
{
    public PhysicsObj physics = new PhysicsObj();
    public Renderer mainRenderer;

    public List<GameAction> statusEffects = new List<GameAction>();

    void Reset()
    {
        mainRenderer = GetComponentInChildren<Renderer>();
    }

    public virtual void OnSpawn()
    {
        SyncPhysics();
    }

    public void SyncPhysics()
    {
        this.physics.position = this.transform.position;
    }
    
    public void Update()
	{
    		float deltaTime = Time.deltaTime;
		for (int i = statusEffects.Count - 1; i >= 0; --i)
		{
			if (statusEffects[i].OnTick(deltaTime))
			{
				statusEffects.RemoveAt(i);
			}
		}
 	}

    public virtual void FixedUpdate()
    {
		if (!physics.enabled)
		{
			return;
		}
        
        this.physics.FixedUpdate(Vector3.zero);
        this.transform.position = this.physics.position;
    }

	public void AddStatusEffect(GameAction effect)
	{
		effect.Invoke();
		statusEffects.Add(effect);
	}

    public void SetColor(Color color)
    {
        mainRenderer.material.color = color;
    }


}