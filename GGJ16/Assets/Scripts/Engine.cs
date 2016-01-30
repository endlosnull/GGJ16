using UnityEngine;

public class Engine : MonoBehaviour
{
	

	Vector2 acceleration = Vector2.zero;

	public float inputSpeed = 1f;
	public Vector2 inputForce = Vector2.zero;

	public void Tick(float deltaTime)
	{

		acceleration += inputForce.normalized*inputSpeed*deltaTime;
		Vector3 deltaPosition = new Vector3(acceleration.x, 0, acceleration.y);
		this.transform.position = this.transform.position + deltaPosition;

		acceleration = Vector2.zero;
	}
}