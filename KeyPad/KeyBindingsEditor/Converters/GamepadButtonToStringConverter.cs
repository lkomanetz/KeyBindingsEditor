using KeyPad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.KeyBindingsEditor.Converters {

	public class GamepadButtonToStringConverter {

		public static string Convert(GamepadButton btn) {
			switch (btn) {
				case GamepadButton.A: return "A Button";
				case GamepadButton.B: return "B Button";
				case GamepadButton.X: return "X Button";
				case GamepadButton.Y: return "Y Button";
				case GamepadButton.RightShoulder: return "Right Shoulder Button";
				case GamepadButton.LeftShoulder: return "Left Shoulder Button";
				case GamepadButton.DpadUp: return "DPAD Up";
				case GamepadButton.DpadDown: return "DPAD Down";
				case GamepadButton.DpadRight: return "DPAD Right";
				case GamepadButton.DpadLeft: return "DPAD Left";
				case GamepadButton.Home: return "Home Button";
				case GamepadButton.Back: return "Back Button";
				case GamepadButton.Menu: return "Menu Button";
				case GamepadButton.LeftStick: return "Left Stick";
				case GamepadButton.RightStick: return "Right Stick";
				default: return String.Empty;
			}
		}

	}

}
