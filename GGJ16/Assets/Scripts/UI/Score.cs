using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Score : MonoBehaviour
{

	// Use this for initialization
	void Start () {
		var evt = GetComponent<Engine>().ScoreUpdate;
		evt.AddListener(Ping);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void Ping()
	{
		int[] scores = GetComponent<Engine>().Scores;
		Debug.Log(scores[0]);
	}
}
