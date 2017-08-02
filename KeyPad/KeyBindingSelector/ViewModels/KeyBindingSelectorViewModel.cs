using KeyPad.DataManager;
using KeyPad.KeyBindingSelector.Models;
using KeyPad.Settings.Models;
using KeyPad.Settings.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.KeyBindingSelector.ViewModels {

	public class KeyBindingSelectorViewModel : IViewModel {

		private const string PROPERTY_NAME = "keybindings_location";
		private const string DIRECTORY_LOCATION = @"C:\Users\logan.komanetz\Desktop\KeyPad\Bindings";
		private IDataManager _serviceSettingManager;
		private IList<ServiceSetting> _serviceSettings;
		private KeyBindingFile _selectedFile;

		public KeyBindingSelectorViewModel(IDataManager dataManager) {
			_serviceSettingManager = dataManager;
			_serviceSettings = (List<ServiceSetting>)_serviceSettingManager.Read();

			this.Files = LoadFiles();

			string selectedFileName = _serviceSettings.Where(x => x.Name.Equals(PROPERTY_NAME)).Single().Value;
			this.SelectedFile = this.Files.Where(x => x.FileName.Equals(selectedFileName)).Single(); 

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
			if (!System.IO.Directory.Exists(DIRECTORY_LOCATION))
				return null;

			return System.IO.Directory.GetFiles(DIRECTORY_LOCATION)
				.Select(x => new KeyBindingFile(x))
				.ToArray();
		}

		private void UpdateServiceSettings() {
			var setting = _serviceSettings
				.Where(x => x.Name.Equals(PROPERTY_NAME))
				.Single();

			setting = new ServiceSetting(PROPERTY_NAME, SelectedFile.FileLocation);
			_serviceSettingManager.Save(_serviceSettings);
		}

	}

}
