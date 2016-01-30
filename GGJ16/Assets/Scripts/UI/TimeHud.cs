using UnityEngine;
using UnityEngine.UI;

public class TimeHud : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void UpdateTime(float time)
    {
        GetComponent<Text>().text = Mathf.Floor(time) + "";
    }
}
