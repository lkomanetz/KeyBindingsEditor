using KeyPad.Models;
using KeyPad.KeyBindingsEditor.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using KeyPad.ViewModels;
using System.Runtime.Serialization;
using KeyPad.Calculators;

namespace KeyPad.KeyBindingsEditor.ViewModels {

	[Serializable]
	internal class KeyBindingViewModel :
		IObservableViewModel,
		ISerializable,
		IDataViewModel<KeyBinding> {

		private const int ESCAPE_KEY_CODE = 27;
		private ICalculator<string, object> _hashCalculator;
		private KeyBinding _binding;
		private Guid _id;
		private int _startingValue;
		private string _initialHash;

		public KeyBindingViewModel(KeyBinding binding, ICalculator<string, object> hashCalculator) {
			_hashCalculator = hashCalculator;
			_id = Guid.NewGuid();
			_startingValue = binding.KeyboardButton;
			_binding = binding;
			_initialHash = _hashCalculator.Calculate(this);
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
					_binding = new KeyBinding(GamepadCode, pressedKeyCode);
					PropertyChanged(this, new PropertyChangedEventArgs(nameof(KeyboardButton)));
				}
			}
		}

		public bool IsSelected { get; set; } = false;
		public bool IsDirty => _initialHash != _hashCalculator.Calculate(this);
		public string GamepadButton => GamepadButtonToStringConverter.Convert(_binding.GamepadButton);
		public string KeyboardButton => KeyboardButtonToStringConverter.Convert(_binding.KeyboardButton);
		public KeyBinding ToDataModel() => _binding;

		public void GetObjectData(SerializationInfo info, StreamingContext context) {
			info.AddValue(nameof(GamepadButton), this.GamepadButton);
			info.AddValue(nameof(KeyboardButton), this.KeyboardButton);
			info.AddValue(nameof(Id), this.Id);
		}
	}

}