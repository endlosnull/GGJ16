using UnityEngine;

public class Ball : MonoBehaviour
{
    public PhysicsObj physics = new PhysicsObj();
    public Actor owner;

    public Renderer[] currentRenderers;

    public void FixedUpdate()
    {
        if (owner == null)
        {
            this.physics.FixedUpdate(Vector3.zero);
            this.transform.position = this.physics.position;
        }
        else
        {
            this.transform.position = this.owner.transform.position + this.owner.Forward * 0.1f;
        }
    }
}