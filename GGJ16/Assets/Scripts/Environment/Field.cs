using UnityEngine;
using System.Collections.Generic;
using Pooling;

public class Field : Singleton<Field>
{


	public Renderer[] currentRenderers;

	public Goal goal;
	public Ball ball;
	public List<Actor> allActors;

	public void BeginRound()
	{
			
		GameObject goalObject = GameObjectFactory.Instance.Spawn("p-Goal", null, Vector3.zero, Quaternion.identity) ;
		goalObject.transform.localRotation = Quaternion.AngleAxis(-90f,Vector3.up);
		goalObject.name = "Goal";
		goal = goalObject.GetComponent<Goal>();
		goalObject.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);

		GameObject ballObject = GameObjectFactory.Instance.Spawn("p-Ball", null, Vector3.forward*3f, Quaternion.identity) ;
		ballObject.name = "Ball";
		ball = ballObject.GetComponent<Ball>();
        ball.field = this;
	}

}