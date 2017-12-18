using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyPad.Models;
using KeyPad.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using KeyPad.DataManager;
using KeyPad.KeyBindingsEditor.Controls;

namespace KeyPad.KeyBindingsEditor.ViewModels {

	internal class KeyBindingsEditorViewModel :
		IFormViewModel,
		IObservableViewModel,
		IForm {

		private KeyBindingViewModel[] _bindings;
		private KeyBindingViewModel _selectedBinding;
		private KeyBindingFileManager _fileManager;
		private Window _owner;
		private KeyBindingFile _file;

		public KeyBindingsEditorViewModel(Window owner) {
			_owner = owner;
			InitKeyBindings();
			this.OnKeyUp = new DelegateCommand<KeyEventArgs>((keyEvent) => SetBinding(keyEvent));
			this.SaveCommand = new DelegateCommand<object>((param) => SaveBindings());
		}


		public KeyBindingsEditorViewModel(
			IDataManager dataManager,
			KeyBindingFile file,
			Window owner
		) {
			_owner = owner;
			_fileManager = (KeyBindingFileManager)dataManager;
			_file = file;

			if (file != null) {
				_bindings = file.Bindings
					.Select(x => new KeyBindingViewModel(x))
					.ToArray();
			}
			else
				InitKeyBindings();

			this.OnKeyUp = new DelegateCommand<KeyEventArgs>((keyEvent) => SetBinding(keyEvent));
			this.SaveCommand = new DelegateCommand<object>((param) => SaveBindings());
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };
		public event EventHandler<EventArgs> SaveCompleted;

		public string Title => "Key Bindings Editor";
		public KeyBindingViewModel[] Bindings {
			get { return _bindings; }
			set {
				_bindings = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(Bindings)));
			}
		}

		public bool IsDirty => this.Bindings.Any(x => x.IsDirty);

		public ICommand OnKeyUp { get; private set; }
		public ICommand SaveCommand { get; private set; }

		public KeyBindingViewModel SelectedBinding {
			get => _selectedBinding;
			set {
				if (_selectedBinding != null)
					_selectedBinding.IsSelected = !_selectedBinding.IsSelected;

				_selectedBinding = value;

				if (_selectedBinding != null)
					_selectedBinding.IsSelected = !_selectedBinding.IsSelected;

				PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedBinding)));
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
			PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsDirty)));
		}

		// TODO(Logan) -> There is a bug with saving a keybinding file.  Not sure why but I need to figure it out.
		private void SaveBindings() {
			IValidator validator = new KeyBindingValidator(this.Bindings);
			var results = validator.Validate();

			if (results.Any(x => !x.IsSuccess)) {
				string msg = ValidatorMessageBuilder.Build(results);
				MessageBox.Show(
					msg,
					this.Title,
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
				return;
			}

			var bindings = this.Bindings
				.Select(x => x.ToDataModel())
				.ToList();

			string fileName = (_file == null) ? GetSaveLocation() : _file.FileName;
			if (String.IsNullOrEmpty(fileName))
				return;

			_file = new KeyBindingFile(
				fileName,
				bindings
			);

			_fileManager.Save(_file);
			SaveCompleted(this, EventArgs.Empty);
			PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsDirty)));
		}

		private string GetSaveLocation() {
			SaveDialog dlg = new SaveDialog(_owner);

			bool? result = dlg.ShowDialog();
			if (result == false) {
				return String.Empty;
			}

			return dlg.FileName;
		}

		private void InitKeyBindings() {
			_bindings = new KeyBindingViewModel[15];
			for (int i = 0; i < _bindings.Length; ++i) {
				Models.KeyBinding binding = new Models.KeyBinding((GamepadButton)i);
				_bindings[i] = new KeyBindingViewModel(binding);
			}
		}

	}

}
