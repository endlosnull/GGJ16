using UnityEngine;

public class Actor : MonoBehaviour
{
	public ActorController controller;
	public ActorBody body;
    public Ball ownedBall;
	public MecanimAnimator animator;

    public PhysicsObj physics = new PhysicsObj();
    public Vector2 inputForce = Vector2.zero;
    private Vector3 directionVector = Vector3.forward;

    private const float possessionDelayTime = 0.5f;
    private float possessionDelay = 0;
    
    public void Update()
	{
		this.transform.rotation = Quaternion.LookRotation(directionVector, Vector3.up);
 	}

	public void DoActionAlpha()
	{
		ActionSequencer sequencer = GetComponent<ActionSequencer>();
		sequencer.RunSequence(sequencer.sequences[0]);

		body.SetShadowColor(Color.blue, 1f);
	}

	public void DoActionBravo()
	{
		Debug.Log("Bravo!");
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
    }

    public void BallHandling(Ball ball)
    {
        if (ball.owner == null && this.possessionDelay < 0)
        {
            float distance = (ball.transform.position - this.transform.position).magnitude;
            if (distance < this.physics.HalfSize + ball.physics.HalfSize)
            {
                TakePossession(ball);
            }
        }
    }

    public void TakePossession(Ball ball)
    {
        if (this.ownedBall != null)
            LosePossession();

        ball.owner = this;
        this.ownedBall = ball;
    }

    public void LosePossession()
    {
        if (this.ownedBall == null)
            return;

        this.ownedBall.owner = null;
        this.ownedBall = null;
        this.possessionDelay = Actor.possessionDelayTime;
    }


}