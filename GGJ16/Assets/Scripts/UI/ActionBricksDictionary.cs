using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



class ActionBrickVisuals
{
	public string name;
	public string image;
	public string method;
}

static class ActionBricksDictionary
{
	public static Dictionary<string, ActionBrickVisuals> Dictionary = new Dictionary<string, ActionBrickVisuals>
	{
		{
			"forward dash",
			new ActionBrickVisuals
			{
				name = "Forward dash",
				image = "Textures/cards/arrow_up",
				method = "AddForwardDash"
			}
		},
		{
			"right dash",
			new ActionBrickVisuals
			{
				name = "Right dash",
				image = "Textures/cards/arrow_right",
				method = "AddRightDash"
			}
		},
		{
			"left dash",
			new ActionBrickVisuals
			{
				name = "Left dash",
				image = "Textures/cards/arrow_left",
				method = "AddLeftDash"
			}
		},
		{
			"jump",
			new ActionBrickVisuals
			{
				name = "Jump",
				image = "Textures/cards/foot",
				method = "AddJump"
			}
		},
		{
			"grab",
			new ActionBrickVisuals
			{
				name = "Grab",
				image = "Textures/cards/hand",
				method = "AddGrab"
			}
		},
		{
			"throw",
			new ActionBrickVisuals
			{
				name = "Throw",
				image = "Textures/cards/throw",
				method = "AddThrow"
			}
		},
		{
			"swat",
			new ActionBrickVisuals
			{
				name = "Swat",
				image = "Textures/cards/hand",
				method = "AddSwat"
			}
		}
	};
}