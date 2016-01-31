using UnityEngine;
using UnityEngine.UI;

public class ScoreHud : MonoBehaviour
{
    // Use this for initialization
    void Start () {
    }
        
    // Update is called once per frame
    void Update () {
    }

    public void UpdateScore(string team0, string team1)
    {
		GameObject.Find("Score_0").GetComponent<Text>().text = team0;
		GameObject.Find("Score_1").GetComponent<Text>().text = team1;
	}
}
