using UnityEngine;
using System.Collections.Generic;

public class Actor : MonoBehaviour
{
    public ActionSequencer sequencer;
	public ActorController controller;
	public ActorBody body;
    public Ball ownedBall;
    public Boss boss;

    public Team team;
    public int positionIndex; // position 0 is forward, 3 is defense, or whatever
    public bool isHuman;
    
    public PhysicsObj physics = new PhysicsObj();
    public Vector2 inputForce = Vector2.zero;
    private Vector3 directionVector = Vector3.forward;

    private const float possessionDelayTime = 0.5f;
    private float possessionDelay = 0;

	public List<GameAction> statusEffects = new List<GameAction>();

    public Collider actorCollider;
    public Rigidbody actorRigidbody;

	void Reset()
	{
		actorCollider = GetComponent<Collider>();
		actorRigidbody = GetComponent<Rigidbody>();
	}

    public virtual void OnSpawn()
    {
        this.physics.position = this.transform.position;
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

	public void DoActionAlpha()
	{
		// Test explosion
		
		/*for (int i = 0; i < Field.Instance.allActors.Count; ++i)
		{
			Actor actor = Field.Instance.allActors[i];
			actor.SetUnityPhysics(true);
			actor.AddUnityExplosionForce(500f, Vector3.down * 10f, 500f);
		}*/
		

		sequencer.RunSequence(sequencer.sequences[0]);
		LockInput effect = new LockInput();
		effect.duration = sequencer.sequences[0].TotalDuration;
		effect.target = gameObject;
		AddStatusEffect(effect);

		body.SetShadowColor(Color.blue, 1f);
	}

	public void DoActionBravo()
	{
		sequencer.RunSequence(sequencer.sequences[1]);
        LockInput effect = new LockInput();
        effect.duration = sequencer.sequences[1].TotalDuration;
        effect.target = gameObject;
        AddStatusEffect(effect);

        body.SetShadowColor(Color.red, 1f);
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

    public void FixedUpdate()
    {
		if (!physics.enabled)
		{
			return;
		}

        if (inputForce.sqrMagnitude > 0)
        {
            this.directionVector.x = inputForce.normalized.x;
            this.directionVector.y = 0;
            this.directionVector.z = inputForce.normalized.y;
        }

        if (this.directionVector.sqrMagnitude == 0)
            this.directionVector = Vector3.forward; // hack to avoid 0 direction
        this.directionVector.Normalize();

        TestObjectCollisions();

        this.physics.FixedUpdate(new Vector3(inputForce.normalized.x, 0, inputForce.normalized.y));
        this.transform.position = this.physics.position;

        this.possessionDelay -= Time.deltaTime;

		Vector3 moveDelta = ((new Vector3(inputForce.normalized.x, 0, inputForce.normalized.y) * this.physics.inputPower) + this.physics.velocity).normalized;
		float forward = Vector3.Dot(this.transform.forward, moveDelta);
		float strafe = Vector3.Dot(this.transform.right, moveDelta);
		body.SetAnimatorMoveSpeed(Mathf.Max(forward, strafe));
    }

    public void TestObjectCollisions()
    {
        Field field = this.boss.field;
        if (field == null)
            return;

        Goal goal = field.goal;

        Vector3 normal;
        float penetration;
        if (PhysicsObj.TestCollision(this.physics, goal.physics, out normal, out penetration))
            this.physics.position += normal * penetration;
    }

    public void BallHandling(Ball ball)
    {
        TryKickBall(ball);
    }

    public void TryTakePossession(Ball ball, float range)
    {
        if (ball.owner != null && this.possessionDelay > 0)
            return;

        float distance = (ball.transform.position - this.transform.position).magnitude;
        if (distance < range)
        {
            TakePossession(ball);
        }
    }

    public void TryKickBall(Ball ball)
    {
        if (ball.owner != null)
            return;

        Vector3 normal;
        float penetration;
        if (PhysicsObj.TestCollision(ball.physics, this.physics, out normal, out penetration))
        {
            ball.physics.position += normal * penetration;

            ball.Bounce(normal);
        }
    }

    public void TakePossession(Ball ball)
    {
        if (this.ownedBall != null)
            LosePossession();

        ball.owner = this;
        this.ownedBall = ball;
        Boss.Instance.SetOffenseTeam(this.team);
		ball.transform.parent = transform.FindTransformInChildren("Hand_Target");
		ball.transform.localPosition = Vector3.zero;
		body.SetAnimatorHold(true);
    }

    public void LosePossession()
    {
        if (this.ownedBall == null)
            return;

        this.ownedBall.owner = null;
        Boss.Instance.SetOffenseTeam(null);
		this.ownedBall.transform.parent = null;
        this.ownedBall = null;
        this.possessionDelay = Actor.possessionDelayTime;
		body.SetAnimatorHold(false);
    }

	public void AddStatusEffect(GameAction effect)
	{
		effect.Invoke();
		statusEffects.Add(effect);
	}

	public void SetUnityPhysics(bool value)
	{
		if (value)
		{
			actorCollider.enabled = true;
			actorRigidbody.useGravity = true;
			actorRigidbody.isKinematic = false;

            body.transform.position = Vector3.zero;
			physics.enabled = false;
		}
		else
		{
			actorCollider.enabled = false;
			actorRigidbody.useGravity = false;
			actorRigidbody.isKinematic = true;
			physics.enabled = true;
		}
	}

	public void AddUnityExplosionForce(float force, Vector3 position, float radius)
	{
		GetComponent<Rigidbody>().AddExplosionForce(force, position, radius);
	}
}