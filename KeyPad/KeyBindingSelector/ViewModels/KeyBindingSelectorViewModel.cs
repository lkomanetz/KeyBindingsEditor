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

namespace KeyPad.KeyBindingSelector.ViewModels {

	public class KeyBindingSelectorViewModel : IViewModel, IObservableViewModel {

		private const string PROPERTY_NAME = "keybindings_location";

		private readonly string _directoryLocation;
		private IDataManager _serviceSettingManager;
		private IList<ServiceSetting> _serviceSettings;
		private KeyBindingFile _selectedFile;

		public KeyBindingSelectorViewModel(IDataManager dataManager) {
			_serviceSettingManager = dataManager;
			_serviceSettings = (List<ServiceSetting>)_serviceSettingManager.Read();
			_directoryLocation = $@"{Environment.CurrentDirectory}\Bindings";

			this.Files = LoadFiles();

			string selectedFileLocation = _serviceSettings
				.Where(x => x.Name.Equals(PROPERTY_NAME))
				.Single()
				.Value;

			var kbf = new KeyBindingFile(selectedFileLocation);
			this.SelectedFile = this.Files.Where(x => x.FileName.Equals(kbf.FileName)).Single(); 

			PropertyChanged(this, new PropertyChangedEventArgs(nameof(Files)));
			PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedFile)));
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public string Title => String.Empty;
		public bool IsDirty => throw new NotImplementedException();
		public IList<KeyBindingFile> Files { get; set; }

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
			for (int i = 0; i < _serviceSettings.Count; ++i) {
				if (_serviceSettings[i].Name == PROPERTY_NAME)
					_serviceSettings[i] = new ServiceSetting(PROPERTY_NAME, SelectedFile.FileLocation);
			}
			_serviceSettingManager.Save(_serviceSettings);
		}

	}

}
