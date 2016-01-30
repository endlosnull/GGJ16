using UnityEngine;
using UnityEngine.Events;

public class Engine : MonoBehaviour
{
	public UnityEvent ScoreUpdate = new UnityEvent();
	public int[] Scores = new int[]{0, 0};

	Vector2 acceleration = Vector2.zero;

	public float inputSpeed = 1f;
	public Vector2 inputForce = Vector2.zero;
	
	public void Tick(float deltaTime)
	{

		acceleration += inputForce.normalized*inputSpeed*deltaTime;
		Vector3 deltaPosition = new Vector3(acceleration.x, 0, acceleration.y);
		this.transform.position = this.transform.position + deltaPosition;

		this.Scores[0]++;
		this.Scores[1]++;
		ScoreUpdate.Invoke();
		
		acceleration = Vector2.zero;
	}
}