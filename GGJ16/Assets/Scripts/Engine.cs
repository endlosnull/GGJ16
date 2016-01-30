using UnityEngine;
using UnityEngine.Events;

public class Engine : MonoBehaviour
{
    public PhysicsObj physics = new PhysicsObj();
	public Vector2 inputForce = Vector2.zero;
	public Vector2 indirectForce = Vector2.zero;

	public void Tick(float deltaTime)
	{
	}

    public void FixedUpdate()
    {
        this.physics.velocity += new Vector3(this.indirectForce.x, 0, this.indirectForce.y);
        this.indirectForce = Vector2.zero;

        this.physics.FixedUpdate(new Vector3(inputForce.normalized.x, 0, inputForce.normalized.y));
        this.transform.position = this.physics.position;

        //DashCard dc = new DashCard();
        //dc.FixedUpdate(this);
    }
}