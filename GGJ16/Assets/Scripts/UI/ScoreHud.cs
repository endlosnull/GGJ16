using UnityEngine;
using UnityEngine.UI;

public class ScoreHud : MonoBehaviour
{
    public string TeamName = "";
        
    // Use this for initialization
    void Start () {
    }
        
    // Update is called once per frame
    void Update () {
    }

    public void UpdateScore(string teamName, string score)
    {
        if(teamName == TeamName)
        {
            GetComponent<Text>().text = score;
        }
    }
}
