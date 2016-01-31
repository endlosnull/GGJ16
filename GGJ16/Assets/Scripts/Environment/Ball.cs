using UnityEngine;

public class Ball : MonoBehaviour
{
    public PhysicsObj physics = new PhysicsObj();
    public Actor owner;

    public Renderer[] currentRenderers;

    void Awake()
    {
        this.physics.SetSize(1);
        this.physics.fullStop = 1.0f;
    }

    public void FixedUpdate()
    {
        if (owner == null)
        {
            this.physics.FixedUpdate(Vector3.zero);
        }
        else
        {
            this.physics.position = this.owner.physics.position + this.owner.Forward * 0.2f + Vector3.up * 0.5f;
            this.physics.velocity = this.owner.physics.velocity;
        }

        this.transform.position = this.physics.position;
    }
}