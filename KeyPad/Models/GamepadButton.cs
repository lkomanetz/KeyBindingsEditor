using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.Models {

	/*
	 * I'm explicitly numbering the enum values so that it is
	 * more human readable.
	 */
	public enum GamepadButton : int {
		A = 0,
		B = 1,
		X = 2,
		Y = 3,
		LeftShoulder = 4,
		RightShoulder = 5,
		Back = 6,
		Menu = 7,
		Home = 8,
		LeftStick = 9,
		RightStick = 10,
		DpadRight = 11,
		DpadLeft = 12,
		DpadUp = 13,
		DpadDown = 14
	}

}
