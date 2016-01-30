using UnityEngine;
using UnityEngine.Events;

public class Engine : MonoBehaviour
{
    public PhysicsObj physics = new PhysicsObj();
	public Vector2 inputForce = Vector2.zero;

	public void Tick(float deltaTime)
	{
	}

    public void FixedUpdate()
    {
        this.physics.FixedUpdate(new Vector3(inputForce.normalized.x, 0, inputForce.normalized.y));
        this.transform.position = this.physics.position;
    }
}