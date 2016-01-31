using System;
using UnityEngine;
using UnityEngine.UI;

public class SequenceHud : MonoBehaviour
{
	public int PlayerIdx;
	public int SequenceIdx;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void OnSequenceUpdated(int playerIdx, int sequenceIdx, int sequence)
	{
		if (playerIdx == PlayerIdx && sequenceIdx == SequenceIdx)
		{
			GetComponent<Text>().text = string.Format("{0}", sequence);
		}
	}
}
