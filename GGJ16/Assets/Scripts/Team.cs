using UnityEngine;

using System.Collections.Generic;

public class Team : MonoBehaviour
{
	public List<Actor> actors = new List<Actor>();
	public int teamIndex;
	public int score;

	public Vector3 GetHomePos(int slot)
	{
		if( teamIndex == 0 )
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector3(-10,0,0);
				case 1: return new Vector3(-5,0,-3);
				case 2: return new Vector3(-5,0,3);
				case 3: return new Vector3(-2,0,1);
			}
		}
		else
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector3(10,0,0);
				case 1: return new Vector3(5,0,-3);
				case 2: return new Vector3(5,0,3);
				case 3: return new Vector3(2,0,-1);
			}
		}
	}

	public Vector3 GetAttackPos(int slot)
	{
		if( teamIndex == 0 )
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector3(-2,0,0);
				case 1: return new Vector3(3,0,-3);
				case 2: return new Vector3(3,0,3);
				case 3: return new Vector3(5,0,1);
			}
		}
		else
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector3(2,0,0);
				case 1: return new Vector3(-3,0,-3);
				case 2: return new Vector3(-3,0,3);
				case 3: return new Vector3(-5,0,-1);
			}
		}
	}

	public Vector3 GetDefendPos(int slot)
	{
		if( teamIndex == 0 )
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector3(-10,0,0);
				case 1: return new Vector3(-5,0,-3);
				case 2: return new Vector3(-5,0,3);
				case 3: return new Vector3(-2,0,1);
			}
		}
		else
		{
			switch(slot) 
			{
				default:
				case 0: return new Vector3(10,0,0);
				case 1: return new Vector3(5,0,-3);
				case 2: return new Vector3(5,0,3);
				case 3: return new Vector3(2,0,-1);
			}
		}
	}
	
}