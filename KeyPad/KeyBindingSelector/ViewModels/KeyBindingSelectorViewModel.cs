using KeyPad.DataManager;
using KeyPad.Models;
using KeyPad.Settings.Models;
using KeyPad.Settings.ViewModels;
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

		private readonly string _directoryLocation;
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
			_directoryLocation = $@"{Environment.CurrentDirectory}\Bindings";

			//TODO(Logan) -> Look a little more into where the below conditional statment is.
			/*
			 * I'm not really sure about where this code is right now.  Does it make sense
			 * for it to live in the KeyBindingSelectorViewModel? Is there a better place
			 * for it to live?  Right now I'm not sure.
			 */
			if (!System.IO.Directory.Exists(_directoryLocation))
				System.IO.Directory.CreateDirectory(_directoryLocation);

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

			if (!System.IO.Directory.Exists(_directoryLocation))
				this.Files = new List<KeyBindingFile>();

			this.Files = (KeyBindingFile[])_keyBindingFileManager.Read();

			if (previouslySelectedFile == null)
				return;

			this.SelectedFile = this.Files.FirstOrDefault(
				x => x.FileName.Equals(previouslySelectedFile.FileName)
			);
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
