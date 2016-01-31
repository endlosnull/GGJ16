using UnityEngine;

using System.Collections.Generic;

public class Team : MonoBehaviour
{
	public List<Actor> actors = new List<Actor>();
	public int teamIndex;
	public int score;
	public bool isDefense;
	public bool isOffense;

	public Vector2 GetHomePos(int slot)
	{
		if( teamIndex == 0 )
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector2(-10,0);
				case 1: return new Vector2(-2,-3);
				case 2: return new Vector2(-2,3);
				case 3: return new Vector2(3,1);
			}
		}
		else
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector2(10,0);
				case 1: return new Vector2(2,-3);
				case 2: return new Vector2(2,3);
				case 3: return new Vector2(-3,-1);
			}
		}
	}

	public Vector2 GetAttackPos(int slot)
	{
		if( teamIndex == 0 )
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector2(-2,0);
				case 1: return new Vector2(3,-3);
				case 2: return new Vector2(3,3);
				case 3: return new Vector2(5,1);
			}
		}
		else
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector2( 2,0);
				case 1: return new Vector2(-3,-3);
				case 2: return new Vector2(-3,3);
				case 3: return new Vector2(-5,-1);
			}
		}
	}

	public Vector2 GetDefendPos(int slot)
	{
		if( teamIndex == 0 )
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector2(-10,0);
				case 1: return new Vector2(-5,-3);
				case 2: return new Vector2(-5,3);
				case 3: return new Vector2(-2,1);
			}
		}
		else
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector2(10,0);
				case 1: return new Vector2(5,-3);
				case 2: return new Vector2(5,3);
				case 3: return new Vector2(2,-1);
			}
		}
	}
	
}