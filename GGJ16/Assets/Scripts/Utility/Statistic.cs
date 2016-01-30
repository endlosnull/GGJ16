using UnityEngine;
using System.Collections.Generic;

public class Statistic
{
	public float baseValue;
	public List<StatMod> mods = new List<StatMod>();

	public float Value
	{
		get
		{
			float value = baseValue;
			for (int i = 0; i < mods.Count; ++i)
			{
				StatMod mod = mods[i];
				switch (mod.operation)
				{
				case StatMod.Operation.Add:
					value += mod.value;
					break;
				case StatMod.Operation.Subtract:
					value -= mod.value;
					break;
				case StatMod.Operation.Multiply:
					value *= mod.value;
					break;
				case StatMod.Operation.Divide:
					value /= mod.value;
					break;
				case StatMod.Operation.Power:
					value = Mathf.Pow(value, mod.value);
					break;
				case StatMod.Operation.Modulo:
					value = value % mod.value;
					break;
				default:
					break;
				}
			}

			return value;
		}
	}
}
