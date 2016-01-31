[System.Serializable]
public class Timer
{
	float timeLeft;
	float timeMax;
	bool isPaused;
	bool isLooping;

	public Timer()
	{
		timeLeft = -1;
		timeMax = -1;
		isPaused = true;
		isLooping = false;
	}

	public Timer(float duration, bool isLooping)
	{
		timeLeft = duration;
		timeMax = duration;
		isPaused = false;
		this.isLooping = isLooping;
	}

	public bool Tick(float deltaTime)
	{
		if( isPaused || timeLeft < 0 )
		{
			return false;
		}
		timeLeft -= deltaTime;
		if( timeLeft < 0f )
		{
			if( isLooping )
			{
				Reset();
			}
			return true;
		}
		return false;
	}

	public void Reset()
	{
		timeLeft = timeMax;
	}

	public void SetDuration(float duration)
	{
		timeLeft = duration;
		timeMax = duration;
		isPaused = false;
	}
}
