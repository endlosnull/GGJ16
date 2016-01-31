using UnityEngine;

public class Ball : MonoBehaviour
{
    public PhysicsObj physics = new PhysicsObj();
    //public Engine

    public Renderer[] currentRenderers;

    public void FixedUpdate()
    {
        this.physics.FixedUpdate(Vector3.zero);
        this.transform.position = this.physics.position;
    }
}