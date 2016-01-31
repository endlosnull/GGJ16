using UnityEngine;
using System.Collections.Generic;

public class BodyAttachment : MonoBehaviour
{
	public bool pinToGround;

	public void FixedUpdate()
	{
		if(pinToGround)
		{
			Vector3 parentPos = this.transform.parent.position;
			Vector3 localPos = new Vector3(0f, 0.01f-parentPos.y, 0f);
			this.transform.localPosition = localPos;
			this.transform.rotation = Quaternion.AngleAxis(90f, Vector3.right);
		}
	}
}