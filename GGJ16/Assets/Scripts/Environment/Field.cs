using UnityEngine;
using GGJ16.Pooling;

public class Field : GGJ16.Singleton<Field>
{


	public Renderer[] currentRenderers;

	public Goal goal;
	public Ball ball;

	public void BeginRound()
	{
		GameObject goalObject = GameObjectFactory.Instance.Spawn("p-Goal", null, Vector3.right*7f, Quaternion.identity) ;
		goalObject.name = "Goal";
		goal = goalObject.GetComponent<Goal>();

		GameObject ballObject = GameObjectFactory.Instance.Spawn("p-Ball", null, Vector3.up*3f, Quaternion.identity) ;
		ballObject.name = "Ball";
		ball = ballObject.GetComponent<Ball>();
	}

}