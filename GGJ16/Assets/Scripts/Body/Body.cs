using UnityEngine;
using System.Collections.Generic;

public class Body : MonoBehaviour
{

	
	public Transform root;
	public GameObject modelRoot;
	public List<GameObject> attachments = new List<GameObject>();

	Quaternion defaultModelRotation = Quaternion.identity;
	float skew = 40f;

	
	public void OnSpawn()
	{
		root = this.transform.parent;
		defaultModelRotation = modelRoot.transform.rotation;
	}

	public virtual void Update()
	{
		modelRoot.transform.rotation = Quaternion.AngleAxis(skew, Vector3.right)*defaultModelRotation*root.rotation;
		
	}

	public void AttachToBone(GameObject go, string boneName)
	{
		Transform foundBone = transform.Find(boneName);
		if( foundBone != null )
		{
			go.transform.SetParent(foundBone, false);
		}
		else
		{
			Destroy(go);

		}
	}
}