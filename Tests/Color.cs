using System;
using System.ComponentModel;

namespace Archon.Enums.Tests
{
	enum Color
	{
		[Category("Blues")]
		[Description("Alice's Wonderland Blue")]
		AliceBlue,

		GhostWhite,

		[Category("Blues")]
		Navy
	}
}