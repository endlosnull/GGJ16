using UnityEngine;

public class Ball : MonoBehaviour
{
    public PhysicsObj physics = new PhysicsObj();
    public Actor owner;
    public Field field;

    public Renderer[] currentRenderers;

    void Awake()
    {
        this.physics.SetSize(1);
        this.physics.fullStop = 1.0f;
        this.physics.bounce = 0.5f;
    }

    public virtual void OnSpawn()
    {
        this.physics.position = this.transform.position;
    }

    public void FixedUpdate()
    {
        if (this.field == null)
            return;

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
            this.physics.position += normal * penetration;
            Bounce(normal);
        }
    }

    private void Bounce(Vector3 normal)
    {
        float angle = Mathf.Atan2(normal.x, normal.z) / Mathf.PI * 180;
        this.physics.velocity = Quaternion.AngleAxis(-angle, Vector3.up) * this.physics.velocity;
        this.physics.velocity.z *= -1;
        this.physics.velocity = Quaternion.AngleAxis(angle, Vector3.up) * this.physics.velocity;
    }
}