using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBehaviour : MonoBehaviour
{
	protected virtual string MenuName { get { return ""; } }

	public Transform[] Cursors;
	private List<float> lastTime = new List<float>();

	protected void InitializeTimes()
	{
		for(int i = 0; i< Cursors.Length; ++i)
		{
			lastTime.Add(0.0f);
		}
	}

	public void ChangeScreenEvent(string screen)
    {
        if (screen == this.MenuName)
        {
			Debug.Log(this.MenuName + " active");
			GetComponent<Canvas>().gameObject.SetActive(true);
        }
		else
		{
			Debug.Log(this.MenuName + " inactive");
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
