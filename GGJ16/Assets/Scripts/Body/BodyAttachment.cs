using UnityEngine;
using System.Collections.Generic;

public class BodyAttachment : MonoBehaviour
{
	public bool pinToGround;

	public void Update()
	{
		if(pinToGround)
		{
			Vector3 pos = this.transform.position;
			pos.y = 0.01f;
			this.transform.position = pos;
		}
	}
}