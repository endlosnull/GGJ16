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
		GameObject score0 = GameObject.Find("Score_0");
		if (score0 != null)
		{
			Text text0 = score0.GetComponent<Text>();
			if (text0 != null)
			{
				text0.text = team0;
			}
		}
		GameObject score1 = GameObject.Find("Score_1");
		if (score1 != null)
		{
			Text text1 = score1.GetComponent<Text>();
			if (text1 != null)
			{
				text1.text = team1;
			}
		}
	}
}
