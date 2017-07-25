using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyPad.KeyBindingsEditor.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace KeyPad.KeyBindingsEditor.ViewModels {

	internal class KeyBindingsEditorViewModel : IViewModel, INotifyPropertyChanged {

		private KeyBindingViewModel[] _bindings;
		private KeyBindingViewModel _selectedBinding;
		private BindingFileManager _fileManager;

		public KeyBindingsEditorViewModel() {
			_bindings = new KeyBindingViewModel[15];
			for (int i = 0; i < _bindings.Length; ++i) {
				Models.KeyBinding binding = new Models.KeyBinding((GamepadButton)i);
				_bindings[i] = new KeyBindingViewModel(binding);
			}
			this.OnKeyUp = new DelegateCommand<KeyEventArgs>((keyEvent) => SetBinding(keyEvent));
			this.SaveCommand = new DelegateCommand<object>((param) => SaveBindings());
		}

		public KeyBindingsEditorViewModel(string fileLocation) {
			_fileManager = _fileManager ?? new BindingFileManager(fileLocation);
			_fileManager = new BindingFileManager(fileLocation);
			_bindings = _fileManager.Read();

			this.OnKeyUp = new DelegateCommand<KeyEventArgs>((keyEvent) => SetBinding(keyEvent));
			this.SaveCommand = new DelegateCommand<object>((param) => SaveBindings());
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public string Title => "Key Bindings Editor";
		public KeyBindingViewModel[] Bindings {
			get { return _bindings; }
			set {
				_bindings = value;
				PropertyChanged(this, new PropertyChangedEventArgs("Bindings"));
			}
		}

		public ICommand OnKeyUp { get; private set; }
		public ICommand SaveCommand { get; private set; }
		public bool SaveEnabled => Bindings.Any(x => x.IsDirty);

		public KeyBindingViewModel SelectedBinding {
			get => _selectedBinding;
			set {
				_selectedBinding = value;
				PropertyChanged(this, new PropertyChangedEventArgs("SelectedBinding"));
			}
		}

		private void SetBinding(KeyEventArgs eventArgs) {
			if (this.SelectedBinding == null)
				return;

			int keyCode = KeyInterop.VirtualKeyFromKey(eventArgs.Key);

			var binding = this.Bindings
				.Where(x => x.Id == this.SelectedBinding.Id)
				.Single();

			binding.KeyCode = keyCode;
			this.SelectedBinding = null;
			PropertyChanged(this, new PropertyChangedEventArgs("SaveEnabled"));
		}

		private void SaveBindings() {
			IValidator validator = new KeyBindingValidator(this.Bindings);
			var result = validator.Validate();

			if (!result.IsSuccess) {
				MessageBox.Show(
					result.Message,
					"Validation Result",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
				return;
			}

			if (_fileManager == null) {
				string fileLocation = GetSaveLocation();
				_fileManager = new BindingFileManager(fileLocation);
			}

			_fileManager.Save(this.Bindings);
			this.Bindings = _fileManager.Read();
			PropertyChanged(this, new PropertyChangedEventArgs("SaveEnabled"));
		}

		private string GetSaveLocation() {
			SaveFileDialog dlg = new SaveFileDialog() {
				DefaultExt = "*.txt",
				Filter = "Text files (*.txt)|*.txt"
			};

			bool? result = dlg.ShowDialog();
			if (result == false) {
				return String.Empty;
			}

			return dlg.FileName;
		}

	}

}
