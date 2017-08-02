using KeyPad.KeyBindingsEditor.Models;
using KeyPad.KeyBindingsEditor.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace KeyPad.KeyBindingsEditor.ViewModels {

	public class KeyBindingViewModel : INotifyPropertyChanged {
		private const int ESCAPE_KEY_CODE = 27;
		private Models.KeyBinding _binding;
		private Guid _id;
		private int _startingValue;

		public KeyBindingViewModel(Models.KeyBinding binding) {
			_id = Guid.NewGuid();
			_startingValue = binding.KeyboardButton;
			_binding = binding;
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public Guid Id => _id;
		public GamepadButton GamepadCode => _binding.GamepadButton;
		public int KeyCode {
			get => _binding.KeyboardButton;
			set {
				int pressedKeyCode = value;
				if (_binding.KeyboardButton != pressedKeyCode) {
					pressedKeyCode = (pressedKeyCode == ESCAPE_KEY_CODE) ? -1 : pressedKeyCode;
					_binding = new Models.KeyBinding(GamepadCode, pressedKeyCode);
					PropertyChanged(this, new PropertyChangedEventArgs(nameof(KeyboardButton)));
				}
			}
		}

		public bool IsSelected { get; set; } = false;
		public bool IsDirty => _binding.KeyboardButton != _startingValue;
		public string GamepadButton => GamepadButtonToStringConverter.Convert(_binding.GamepadButton);
		public string KeyboardButton => KeyboardButtonToStringConverter.Convert(_binding.KeyboardButton);

	}

}