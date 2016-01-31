using UnityEngine;
using UnityEngine.UI;

public class MenuBehaviour : MonoBehaviour
{
	protected virtual string MenuName { get { return ""; } }

	public Transform[] Cursors;

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
}
