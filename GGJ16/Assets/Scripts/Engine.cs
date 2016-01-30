using UnityEngine;

public class Engine : MonoBehaviour
{
	

	Vector2 acceleration = Vector2.zero;

	public float inputSpeed = 1f;
	public Vector2 inputForce = Vector2.zero;
	public Vector2 indirectForce = Vector2.zero;

	public void Tick(float deltaTime)
	{

		acceleration += ((inputForce.normalized*inputSpeed) + indirectForce)*deltaTime;
		Vector3 deltaPosition = new Vector3(acceleration.x, 0, acceleration.y);
		this.transform.position = this.transform.position + deltaPosition;

		indirectForce = Vector2.zero;
		acceleration = Vector2.zero;
	}
}