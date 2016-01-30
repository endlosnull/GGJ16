namespace GGJ16
{
	[System.Serializable]
	public class Timer
	{
		float timeLeft;
		float timeMax;
		bool isPaused;
		bool resetOnComplete;

		public Timer()
		{
			timeLeft = -1;
			timeMax = -1;
			isPaused = true;
			resetOnComplete = false;
		}

		public Timer(float duration, bool resetOnComplete)
		{
			timeLeft = duration;
			timeMax = duration;
			isPaused = false;
			this.resetOnComplete = resetOnComplete;
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
				if( resetOnComplete )
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
}
