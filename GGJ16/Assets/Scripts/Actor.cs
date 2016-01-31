using UnityEngine;
using System.Collections.Generic;

public class Actor : MonoBehaviour
{
    public ActionSequencer sequencer;
	public ActorController controller;
	public ActorBody body;
    public Ball ownedBall;

    public Team team;
    public int positionIndex; // position 0 is forward, 3 is defense, or whatever
    
    public PhysicsObj physics = new PhysicsObj();
    public Vector2 inputForce = Vector2.zero;
    private Vector3 directionVector = Vector3.forward;

    private const float possessionDelayTime = 0.5f;
    private float possessionDelay = 0;

	public List<GameAction> statusEffects = new List<GameAction>();

    public virtual void OnSpawn()
    {
        physics.position = this.transform.position;
    }

    
    public void Update()
	{
		this.transform.rotation = Quaternion.LookRotation(directionVector, Vector3.up);

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
		
		sequencer.RunSequence(sequencer.sequences[0]);
		LockInput effect = new LockInput();
		effect.duration = sequencer.sequences[0].TotalDuration;
		effect.target = gameObject;
		AddStatusEffect(effect);

		body.SetShadowColor(Color.blue, 1f);
	}

	public void DoActionBravo()
	{
		//Debug.Log("Bravo!");
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
        if (inputForce.sqrMagnitude > 0)
        {
            this.directionVector.x = inputForce.normalized.x;
            this.directionVector.y = 0;
            this.directionVector.z = inputForce.normalized.y;
        }

        if (this.directionVector.sqrMagnitude == 0)
            this.directionVector = Vector3.forward; // hack to avoid 0 direction
        this.directionVector.Normalize();

        this.physics.FixedUpdate(new Vector3(inputForce.normalized.x, 0, inputForce.normalized.y));
        this.transform.position = this.physics.position;

        this.possessionDelay -= Time.deltaTime;

		Vector3 moveDelta = ((new Vector3(inputForce.normalized.x, 0, inputForce.normalized.y) * this.physics.inputPower) + this.physics.velocity).normalized;
		float forward = Vector3.Dot(this.transform.forward, moveDelta);
		float strafe = Vector3.Dot(this.transform.right, moveDelta);
		body.SetAnimatorMoveSpeed(Mathf.Max(forward, strafe));
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

        Vector3 diff = ball.transform.position - this.transform.position;
        diff.y = 0;
        Vector3 normal = diff.normalized;
        if (diff.sqrMagnitude == 0)
            return;

        float distance = diff.magnitude;
        float penetration = this.physics.HalfSize + ball.physics.HalfSize - distance;
        if (penetration > 0)
        {
            ball.physics.position += normal * penetration;
            //ball.physics.velocity += normal * penetration;
        }
        //else
        //{
        //    float penetration2 = penetration + 5;
        //    if (penetration2 > 0)
        //    {
        //        //ball.physics.position += normal * penetration2;
        //        ball.physics.velocity += this.physics.velocity * penetration2 * 20.5f;
        //    }
        //}
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
}