using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBehaviour : MonoBehaviour
{
	protected virtual string MenuName { get { return ""; } }

	public List<Transform> Cursors = new List<Transform>();
	public float[] lastTime;

	public virtual void Start()
	{
		Cursors.Clear();
		foreach(Transform t in this.transform)
		{
			if(t.name.StartsWith("Cursor"))
			{
				Cursors.Add(t);
			}
		}
 		Cursors.Sort(delegate(Transform t1, Transform t2)
        {
        	return -t1.position.y.CompareTo(t2.position.y);
    	});
	}


	public void ChangeScreenEvent(string screen)
    {
        if (screen == this.MenuName)
        {
			//Debug.Log(this.MenuName + " active");
			GetComponent<Canvas>().gameObject.SetActive(true);
        }
		else
		{
			//Debug.Log(this.MenuName + " inactive");
			GetComponent<Canvas>().gameObject.SetActive(false);
		}
    }

	protected bool CanMoveCursor(int idx)
	{
		if (Time.realtimeSinceStartup - lastTime[idx] < 0.25)
		{
			return false;
		}
		lastTime[idx] = Time.realtimeSinceStartup;
		return true;
	}
}
