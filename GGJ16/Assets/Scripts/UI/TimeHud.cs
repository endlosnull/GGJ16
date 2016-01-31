using System;
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
		TimeSpan t = TimeSpan.FromSeconds(time);
		GetComponent<Text>().text = string.Format("{0:D2}:{1:D2}.{2}", t.Minutes, t.Seconds, (int)Mathf.Round(t.Milliseconds / 100));
    }
}
