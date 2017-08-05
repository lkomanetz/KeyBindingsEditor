using KeyPad.DataManager;
using KeyPad.KeyBindingSelector.Models;
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

	public class KeyBindingSelectorViewModel :
		IViewModel,
		IObservableViewModel {

		private const string PROPERTY_NAME = "keybindings_location";

		private readonly string _directoryLocation;
		private IDataManager _serviceSettingManager;
		private IList<ServiceSetting> _serviceSettings;
		private Visibility _visibility;
		private KeyBindingFile _selectedFile;

		public KeyBindingSelectorViewModel(IDataManager dataManager) {
			_serviceSettingManager = dataManager;
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

			this.Files = LoadFiles();

			string selectedFileLocation = _serviceSettings
				.Where(x => x.Name.Equals(PROPERTY_NAME))
				.Single()
				.Value;

			var kbf = new KeyBindingFile(selectedFileLocation);
			this.SelectedFile = this.Files.FirstOrDefault(x => x.FileName.Equals(kbf.FileName)); 

			PropertyChanged(this, new PropertyChangedEventArgs(nameof(Files)));
			PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedFile)));
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public string Title => String.Empty;
		public IList<KeyBindingFile> Files { get; set; }

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
				if (_selectedFile != null && _selectedFile.FileLocation.Equals(value.FileLocation))
					return;

				_selectedFile = value;
				UpdateServiceSettings();
			}
		}

		private KeyBindingFile[] LoadFiles() {
			if (!System.IO.Directory.Exists(_directoryLocation))
				return null;

			return System.IO.Directory.GetFiles(_directoryLocation)
				.Select(x => new KeyBindingFile(x))
				.ToArray();
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
