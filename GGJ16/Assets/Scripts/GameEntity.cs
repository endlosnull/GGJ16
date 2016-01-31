using UnityEngine;
using System.Collections.Generic;

public class GameEntity : MonoBehaviour
{
    public PhysicsObj physics = new PhysicsObj();
    public Vector2 inputForce = Vector2.zero;
    public Vector2 inputAdj = Vector2.zero;
    private Vector3 directionVector = Vector3.forward;

    public List<GameAction> statusEffects = new List<GameAction>();

    public Collider gameCollider;
    public Rigidbody gameRigidbody;

	void Reset()
	{
		gameCollider = GetComponent<Collider>();
		gameRigidbody = GetComponent<Rigidbody>();
	}

    public virtual void OnSpawn()
    {
        SyncPhysics();
    }

    public void SyncPhysics()
    {
        this.physics.position = this.transform.position;
        this.directionVector = this.transform.forward;
    }


    
    public void Update()
	{
        if (physics.enabled)
        {
            this.transform.rotation = Quaternion.LookRotation(directionVector, Vector3.up);
        }

		float deltaTime = Time.deltaTime;
		for (int i = statusEffects.Count - 1; i >= 0; --i)
		{
			if (statusEffects[i].OnTick(deltaTime))
			{
				statusEffects.RemoveAt(i);
			}
		}
 	}


    public Vector3 Forward
    {
        get { return this.directionVector; }
    }

    public Vector3 Backward
    {
        get { return -this.directionVector; }
    }

    public Vector3 Left
    {
        get { return Vector3.Cross(this.directionVector, Vector3.up); }
    }

    public Vector3 Right
    {
        get { return -this.Left; }
    }

    public void Turn(float angleDegrees)
    {
        this.directionVector = Quaternion.AngleAxis(angleDegrees, Vector3.up) * this.directionVector;
    }

    public virtual void FixedUpdate()
    {
		if (!physics.enabled)
		{
			return;
		}

        this.inputAdj = this.inputForce;

        Vector2 inputAdj = inputForce;
        if (inputAdj.sqrMagnitude < 0.2f * 0.2f)
            inputAdj = Vector2.zero; // deadzone HACK, sorry


        if (inputAdj.sqrMagnitude > 0)
        {
            this.directionVector.x = inputForce.normalized.x;
            this.directionVector.y = 0;
            this.directionVector.z = inputForce.normalized.y;
        }

        if (this.directionVector.sqrMagnitude == 0)
            this.directionVector = Vector3.forward; // hack to avoid 0 direction
        this.directionVector.Normalize();

        TestObjectCollisions();
        Vector3 physicsInput = Vector3.zero;
        if (inputAdj.sqrMagnitude > 0)
            physicsInput = new Vector3(inputAdj.normalized.x, 0, inputAdj.normalized.y);

        this.physics.FixedUpdate(physicsInput);
        this.transform.position = this.physics.position;
    }

    public virtual void TestObjectCollisions()
    {

    }

   

	public void AddStatusEffect(GameAction effect)
	{
		effect.Invoke();
		statusEffects.Add(effect);
	}

	public virtual void SetUnityPhysics(bool value)
	{
		if (value)
		{
            if( gameCollider )
            {
                gameCollider.enabled = true;
            }
            if( gameRigidbody )
            {
    			gameRigidbody.useGravity = true;
    			gameRigidbody.isKinematic = false;
            }
			physics.enabled = false;
		}
		else
		{
			if( gameCollider )
            {
                gameCollider.enabled = false;
            }
			if( gameRigidbody )
            {
                gameRigidbody.useGravity = false;
                gameRigidbody.isKinematic = true;
            }
			physics.enabled = true;
		}
	}

	public void AddUnityExplosionForce(float force, Vector3 position, float radius)
	{
        if( gameRigidbody )
        {
            gameRigidbody.AddExplosionForce(force, position, radius);
        }
	}
}