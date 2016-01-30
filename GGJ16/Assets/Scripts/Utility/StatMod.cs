using UnityEngine;
using System.Collections;

public class StatMod
{
	public enum Operation
	{
		None,
		Add,
		Subtract,
		Multiply,
		Divide,
		Power,
		Modulo,
	}

	public float value;
	public Operation operation;
}
