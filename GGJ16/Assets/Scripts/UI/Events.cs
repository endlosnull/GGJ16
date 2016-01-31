using UnityEngine.Events;

[System.Serializable]
public class ScoreUpdateEvent : UnityEvent<string, string> { }
[System.Serializable]
public class ChangeScreenEvent : UnityEvent<string> { }
[System.Serializable]
public class UpdateTimeEvent : UnityEvent<float> { }
[System.Serializable]
public class MoveCursorEvent : UnityEvent<int, MoveCursorAction> { }

public enum MoveCursorAction
{
	Left,
	Right,
	Up,
	Down
}
