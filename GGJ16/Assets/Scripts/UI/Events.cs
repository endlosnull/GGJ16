using UnityEngine.Events;

[System.Serializable]
public class ScoreUpdateEvent : UnityEvent<string, string> { }
[System.Serializable]
public class ChangeScreenEvent : UnityEvent<string> { }
[System.Serializable]
public class UpdateTimeEvent : UnityEvent<float> { }
[System.Serializable]
public class SetPlayerStateEvent : UnityEvent<string, string, SetPlayerStateAction> { }

public enum SetPlayerStateAction
{
	Add,
	Remove,
	ChangeTeam
}
