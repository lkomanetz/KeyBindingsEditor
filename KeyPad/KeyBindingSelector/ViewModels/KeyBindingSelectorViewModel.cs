using KeyPad.DataManager;
using KeyPad.Models;
using KeyPad.SettingsEditor.Models;
using KeyPad.SettingsEditor.ViewModels;
using KeyPad.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KeyPad.KeyBindingSelector.ViewModels {

	internal class KeyBindingSelectorViewModel :
		IViewModel,
		IObservableViewModel {

		private const string PROPERTY_NAME = "keybindings_location";

		private IDataManager _serviceSettingManager;
		private IDataManager _keyBindingFileManager;
		private IList<ServiceSetting> _serviceSettings;
		private Visibility _visibility;
		private KeyBindingFile _selectedFile;

		public KeyBindingSelectorViewModel(
			IDataManager serviceSettingManager,
			IDataManager keyBindingFileManager
		) {
			_serviceSettingManager = serviceSettingManager;
			_keyBindingFileManager = keyBindingFileManager;
			_serviceSettings = (List<ServiceSetting>)_serviceSettingManager.Read();

			LoadFiles();

			string selectedFileLocation = _serviceSettings
				.Where(x => x.Name.Equals(PROPERTY_NAME))
				.Single()
				.Value;

			this.SelectedFile = this.Files.FirstOrDefault(x => x.FileLocation.Equals(selectedFileLocation)); 
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public string Title => String.Empty;

		private IList<KeyBindingFile> _files;
		public IList<KeyBindingFile> Files {
			get => _files;
			set {
				_files = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(Files)));
			}
		}

		public Visibility Visibility {
			get => _visibility;
			set {
				if (_visibility == value)
					return;
				_visibility = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(Visibility)));
			}
		}

		public KeyBindingFile SelectedFile {
			get => _selectedFile;
			set {
				_selectedFile = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedFile)));
				UpdateServiceSettings();
			}
		}

		public void UpdateFile(KeyBindingFile file) {
			var currentItem = this.Files.First(x => x.FileName.Equals(file.FileName));
			int index = this.Files.IndexOf(currentItem);
			this.Files[index] = file;
		}

		public void LoadFiles() {
			var previouslySelectedFile = this.SelectedFile;

			this.Files = (KeyBindingFile[])_keyBindingFileManager.Read();

			if (previouslySelectedFile == null)
				return;

			this.SelectedFile = this.Files.FirstOrDefault(
				x => x.FileName.Equals(previouslySelectedFile.FileName)
			);
		}

		public void DeleteSelectedKeyBinding() {
			string msg = String.Empty;
			if (this.SelectedFile == null)
				msg = "No key binding file to delete.";
			else
				msg = $"Delete file '{SelectedFile.FileName}'?";

			MessageBoxResult result = MessageBox.Show(
				msg,
				"Delete",
				MessageBoxButton.YesNo,
				MessageBoxImage.Question
			);

			if (result != MessageBoxResult.Yes)
				return;

			_keyBindingFileManager.Delete(this.SelectedFile);

			this.Files = this.Files
				.Where(x => !x.FileName.Equals(this.SelectedFile.FileName))
				.ToList();

			this.SelectedFile = (this.Files.Count > 0) ? this.Files[0] : null;
		}

		private void UpdateServiceSettings() {
			var selectedFile = SelectedFile ?? new KeyBindingFile();
			for (int i = 0; i < _serviceSettings.Count; ++i) {
				if (_serviceSettings[i].Name == PROPERTY_NAME)
					_serviceSettings[i] = new ServiceSetting(PROPERTY_NAME, selectedFile.FileLocation);
			}
			_serviceSettingManager.Save(_serviceSettings);
		}

	}

}
