using KeyPad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.KeyBindingsEditor.ViewModels {

	internal class KeyBindingsDesignViewModel {

		internal KeyBindingViewModel[] Bindings => new KeyBindingViewModel[] {
			new KeyBindingViewModel(new KeyBinding(GamepadButton.A, 97), null),
			new KeyBindingViewModel(new KeyBinding(GamepadButton.B, 98), null),
			new KeyBindingViewModel(new KeyBinding(GamepadButton.LeftShoulder, 113), null),
			new KeyBindingViewModel(new KeyBinding(GamepadButton.RightShoulder, 114), null),
			new KeyBindingViewModel(new KeyBinding(GamepadButton.Menu, -1), null),
			new KeyBindingViewModel(new KeyBinding(GamepadButton.DpadLeft, 37), null)
		};

	}

}
