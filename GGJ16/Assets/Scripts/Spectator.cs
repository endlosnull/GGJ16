using UnityEngine;
using System.Collections;

public class Spectator : MonoBehaviour
{
	public Animator animator;
	public Collider gameCollider;
	public Rigidbody gameRigidbody;

	void Start()
	{
		animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, Random.value);
	}

	public void SetUnityPhysics(bool value)
	{
		if (value)
		{
			if( gameCollider )
			{
				gameCollider.enabled = true;
			}
			if( gameRigidbody )
			{
				gameRigidbody.useGravity = true;
				gameRigidbody.isKinematic = false;
			}
		}
		else
		{
			if( gameCollider )
			{
				gameCollider.enabled = false;
			}
			if( gameRigidbody )
			{
				gameRigidbody.useGravity = false;
				gameRigidbody.isKinematic = true;
			}
		}
	}

	public void AddUnityExplosionForce(float force, Vector3 position, float radius)
	{
		if( gameRigidbody )
		{
			gameRigidbody.AddExplosionForce(force, position, radius);
		}
	}
}
