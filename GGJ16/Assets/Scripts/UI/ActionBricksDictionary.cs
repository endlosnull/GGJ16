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
			"back dash",
			new ActionBrickVisuals
			{
				name = "Back dash",
				image = "Textures/cards/arrow_down",
				method = "AddBackDash"
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
			"turn left",
			new ActionBrickVisuals
			{
				name = "Left Turn",
				image = "Textures/cards/arrow_turn_left",
				method = "AddTurnLeft"
			}
		},
		{
			"turn right",
			new ActionBrickVisuals
			{
				name = "Right Turn",
				image = "Textures/cards/arrow_turn_right",
				method = "AddTurnRight"
			}
		},
		{
			"turn around",
			new ActionBrickVisuals
			{
				name = "TurnAround",
				image = "Textures/cards/arrow_turn_down",
				method = "AddTurnAround"
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