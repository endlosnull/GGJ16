using UnityEngine;

public class Ball : GameEntity
{
    public Actor owner;
    public Field field;
	public TrailRenderer trail;
    public AudioSource audioSource;
	public Renderer vfxRenderer;
    private bool passedThroughGoal = false;
    private int scoringTeam = 0;

	Color defaultColor =  new Color(0f,0f,0f,0.3f);

    public override void OnSpawn()
    {
        this.physics.SetSize(1);
        this.physics.fullStop = 1.0f;
        this.physics.bounce = 0.5f;
        this.physics.audioSource = audioSource;
		this.vfxRenderer.material.color = defaultColor;

        base.OnSpawn();
    }

   
    public override void FixedUpdate()
    {
        if (this.field == null)
            return;

        if (!physics.enabled)
        {
            return;
        }

        if (owner == null)
        {
            this.physics.FixedUpdate(Vector3.zero);
			this.transform.position = this.physics.position;

            CheckScore();
        }
        else
        {
            this.physics.position = this.owner.physics.position + this.owner.Forward * 0.2f + Vector3.up * 0.5f;
            this.physics.velocity = this.owner.physics.velocity;
        }
    }

    public void CheckScore()
    {
        Goal goal = this.field.goal;

        Vector3 normal;
        float penetration;
        if (PhysicsObj.TestCollision(this.physics, goal.physics, out normal, out penetration))
        {
            // see if the angle is right for a score
            float normalAngle = Mathf.Atan2(normal.x, normal.z) / Mathf.PI * 180;
            float goalAngle = 90 + Mathf.Atan2(this.transform.forward.x, this.transform.forward.z) / Mathf.PI * 180;

            float scoreArc = 130;
            normalAngle -= goalAngle;
            normalAngle = PutAngleInRange(normalAngle);
            float normalAngleOp = PutAngleInRange(normalAngle + 180);

            bool inRange = false;
            if (Mathf.Abs(normalAngle) < scoreArc / 2)
                inRange = true;
            else if (Mathf.Abs(normalAngleOp) < scoreArc / 2)
                inRange = true;

            if (this.physics.position.y < 1)
                inRange = false;

            if (!inRange)
            {
                this.physics.position += normal * penetration;
                Bounce(normal);
            }
            else
            {
                Vector3 posDiff = this.physics.position - goal.physics.position;
                bool passedGoal = (this.physics.velocity.x > 0) == (posDiff.x > 0);
                if (this.field.RoundActive)
                {
                    int angleScoringTeam = (Mathf.Abs(normalAngle) < 90) ? 0 : 1;
                    int dirScoringTeam = (this.physics.velocity.x > 0) ? 0 : 1;
                    if (passedGoal && !this.passedThroughGoal && dirScoringTeam == angleScoringTeam)
                    {
                        this.passedThroughGoal = true;
                        this.scoringTeam = angleScoringTeam;
                    }
                }
            }
        }
        else
        {
            if (this.passedThroughGoal)
                this.field.OnScore(this.scoringTeam);
            this.passedThroughGoal = false;
        }
    }

    public float PutAngleInRange(float angle)
    {
        while (angle > 180)
            angle -= 360;

        while (angle < -180)
            angle += 360;

        return angle;
    }

    public void Bounce(Vector3 normal)
    {
        float angle = Mathf.Atan2(normal.x, normal.z) / Mathf.PI * 180;
        this.physics.velocity = Quaternion.AngleAxis(-angle, Vector3.up) * this.physics.velocity;
        this.physics.velocity.z *= -1;

        float vol = Mathf.Abs(this.physics.velocity.z) / 3;
        if (vol > 1)
            vol = 1;
        AudioManager.Instance.PlayOneShot(audioSource, AudioManager.Instance.ballBounce, vol);

        this.physics.velocity = Quaternion.AngleAxis(angle, Vector3.up) * this.physics.velocity;
    }

	public override void SetUnityPhysics(bool value)
	{
		base.SetUnityPhysics(value);
		if (value)
		{
			vfxRenderer.enabled = false;
		}
		else
		{
			vfxRenderer.enabled = true;
		}
	}
}