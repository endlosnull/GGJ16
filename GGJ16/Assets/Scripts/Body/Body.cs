using UnityEngine;
using System.Collections.Generic;

public class Body : MonoBehaviour
{

	
	public Transform root;
	public Renderer primaryRenderer;
	public List<GameObject> attachments = new List<GameObject>();

	Quaternion defaultRendererRotation = Quaternion.identity;
	float skew = 40f;

	
	public void OnSpawn()
	{
		root = this.transform.parent;
		defaultRendererRotation = primaryRenderer.transform.rotation;
	}

	public virtual void Update()
	{
		primaryRenderer.transform.rotation = Quaternion.AngleAxis(skew, Vector3.right)*defaultRendererRotation*root.rotation;
		
	}

	public void AttachToBone(GameObject go, string boneName)
	{
		Transform foundBone = transform.Find(boneName);
		if( foundBone != null )
		{
			go.transform.SetParent(foundBone);
		}
		else
		{
			Destroy(go);

		}
	}
}