using UnityEngine;
using System.Collections.Generic;

public class Actor : GameEntity
{
    public ActionSequencer sequencer;
	public ActorController controller;
	public ActorBody body;
    public Ball ownedBall;
    public Boss boss;
    public AudioSource audioSource;

    public Team team;
    public int positionIndex; // position 0 is forward, 3 is defense, or whatever
    public bool isHuman;

    private const float possessionDelayTime = 0.5f;
    private float possessionDelay = 0;
    private float ballCheckCooldown = 0;

    void Reset()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void DoActionAlpha()
	{
		sequencer.RunSequence(0);
		LockInput effect = new LockInput();
		effect.duration = sequencer.sequences[0].TotalDuration;
		effect.target = gameObject;
		AddStatusEffect(effect);

		body.SetShadowColor(Color.blue, 1f);
	}

	public void DoActionBravo()
	{
		sequencer.RunSequence(1);
        LockInput effect = new LockInput();
        effect.duration = sequencer.sequences[1].TotalDuration;
        effect.target = gameObject;
        AddStatusEffect(effect);

        body.SetShadowColor(Color.red, 1f);
	}

    public override void FixedUpdate()
    {
		base.FixedUpdate();

        this.possessionDelay -= Time.deltaTime;

		Vector3 moveDelta = ((new Vector3(inputForce.normalized.x, 0, inputForce.normalized.y) * this.physics.inputPower) + this.physics.velocity).normalized;
		float forward = Vector3.Dot(this.transform.forward, moveDelta);
		float strafe = Vector3.Dot(this.transform.right, moveDelta);
		body.SetAnimatorMoveSpeed(Mathf.Max(forward, strafe));
    }

    public override void TestObjectCollisions()
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
        if (ball.owner != null || this.possessionDelay > 0)
            return;

        float distance = (ball.transform.position - this.transform.position).magnitude;
        if (distance < range)
        {
            TakePossession(ball);
        }
    }

    public void TrySwatBall(Ball ball, float range)
    {
        if (ball.owner == this)
            return; // don't swat self!

        float distance = (ball.transform.position - this.transform.position).magnitude;
        if (distance < range)
        {
            SwatBall(ball);
        }
    }

    public void OnThrowBall()
    {
        this.ballCheckCooldown = 0.2f;
    }

    public void TryKickBall(Ball ball)
    {
        if (ball.owner != null || this.ballCheckCooldown > 0)
            return;

        Vector3 normal;
        float penetration;
        if (PhysicsObj.TestCollision(ball.physics, this.physics, out normal, out penetration))
        {
            ball.physics.position += normal * penetration;

            ball.Bounce(normal);
        }
    }

    public void BeSwatted()
    {
        Ball ball = this.ownedBall;
        LosePossession();

        ball.physics.velocity += Vector3.down * 2;
    }

    public void SwatBall(Ball ball)
    {
        if (ball.owner == null)
            return;

        ball.owner.BeSwatted();
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

	public override void SetUnityPhysics(bool value)
	{
        base.SetUnityPhysics(value);
		if (value)
		{
			body.vfxRenderer.enabled = false;
		}
		else
		{
			body.vfxRenderer.enabled = true;
		}
	}
}