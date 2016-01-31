public class Flag
{
	int value;

	public int Value { get { return value; } }

	public Flag() : this(0)
	{
	}

	public Flag(int mask)
	{
		value = mask;
	}

	public int On(int flag)
	{
		return value |= (1 << flag);
	}

	public int Off(int flag)
	{
		return value &= ~(1 << flag);
	}

	public int Toggle(int flag)
	{
		return value ^= (1 << flag);
	}

	public bool Check(int flag)
	{
		return (value & (1 << flag)) != 0;
	}
}
