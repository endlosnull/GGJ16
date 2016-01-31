using UnityEngine.Events;

[System.Serializable]
public class ScoreUpdateEvent : UnityEvent<string, string> { }
[System.Serializable]
public class ChangeScreenEvent : UnityEvent<string> { }
[System.Serializable]
public class UpdateTimeEvent : UnityEvent<float> { }
[System.Serializable]
public class MoveCursorEvent : UnityEvent<int, MoveCursorAction> { }
[System.Serializable]
public class SequenceUpdatedEvent : UnityEvent<int, int, int> { }

public enum MoveCursorAction
{
	Left,
	Right,
	Up,
	Down
}
