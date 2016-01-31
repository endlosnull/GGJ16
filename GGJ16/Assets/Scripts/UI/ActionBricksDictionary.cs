using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



class ActionBrickVisuals
{
	public string name;
	public string image;
}

static class ActionBricksDictionary
{
	public static Dictionary<string, ActionBrickVisuals> Dictionary = new Dictionary<string, ActionBrickVisuals>
	{
		{ "forward dash", new ActionBrickVisuals { name = "Forward dash", image = "Textures/cards/arrow_up" } },
		{ "right dash", new ActionBrickVisuals { name = "Right dash", image = "Textures/cards/arrow_right" } },
		{ "left dash", new ActionBrickVisuals { name = "Left dash", image = "Textures/cards/arrow_left" } },
		{ "jump", new ActionBrickVisuals { name = "Jump", image = "Textures/cards/foot" } },
		{ "throw", new ActionBrickVisuals { name = "Throw", image = "Textures/cards/throw" } },
		{ "swat", new ActionBrickVisuals { name = "Swat", image = "Textures/cards/hand" } }
	};
}