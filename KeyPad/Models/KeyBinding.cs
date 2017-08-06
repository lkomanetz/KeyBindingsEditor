using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.Models {

	public class KeyBinding {

		private GamepadButton _gamepadButton;
		private int _keyboardButton;

		public KeyBinding(GamepadButton btn) {
			_gamepadButton = btn;
			_keyboardButton = -1;
		}

		public KeyBinding(GamepadButton btn, int keyCode) {
			_gamepadButton = btn;
			_keyboardButton = keyCode;
		}

		public GamepadButton GamepadButton => _gamepadButton;
		public int KeyboardButton => _keyboardButton;

	}

}
