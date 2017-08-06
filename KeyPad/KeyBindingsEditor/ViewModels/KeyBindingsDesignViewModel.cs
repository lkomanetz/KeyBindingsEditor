using KeyPad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.KeyBindingsEditor.ViewModels {

	public class KeyBindingsDesignViewModel {

		public KeyBindingViewModel[] Bindings => new KeyBindingViewModel[] {
			new KeyBindingViewModel(new KeyBinding(GamepadButton.A, 97)),
			new KeyBindingViewModel(new KeyBinding(GamepadButton.B, 98)),
			new KeyBindingViewModel(new KeyBinding(GamepadButton.LeftShoulder, 113)),
			new KeyBindingViewModel(new KeyBinding(GamepadButton.RightShoulder, 114)),
			new KeyBindingViewModel(new KeyBinding(GamepadButton.Menu, -1)),
			new KeyBindingViewModel(new KeyBinding(GamepadButton.DpadLeft, 37))
		};

	}

}
