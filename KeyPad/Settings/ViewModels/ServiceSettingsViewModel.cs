using KeyPad.Settings.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KeyPad.Settings.ViewModels {

	public class ServiceSettingsViewModel : INotifyPropertyChanged{
		private string _fileLocation;
		private ObservableCollection<KeyPadSettingViewModel> _serviceSettings;

		public ServiceSettingsViewModel(string fileLocation) {
			_fileLocation = fileLocation;
			_serviceSettings = new ObservableCollection<KeyPadSettingViewModel>(); 
			LoadSettings(fileLocation);
			this.SaveCommand = new DelegateCommand<object>((param) => SaveSettings());
			this.UpdateButtonCommand = new DelegateCommand<object>((param) => PropertyChanged(this, new PropertyChangedEventArgs("ButtonEnabled")));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<KeyPadSettingViewModel> Settings => _serviceSettings;
		public bool ButtonEnabled => _serviceSettings.Any(x => x.IsDirty);
		public ICommand SaveCommand { get; private set; }
		public ICommand UpdateButtonCommand { get; private set; }

		private void LoadSettings(string fileLocation) {
			if (_serviceSettings.Count > 0)
				_serviceSettings.Clear();

			string[] fileContents = File.ReadAllLines(fileLocation);	
			foreach (string line in fileContents) {
				string[] items = line.Split('=');
				string name = items[0];
				string value = (items[1].Equals("NULL")) ? String.Empty : items[1];
				var setting = new KeyPadSetting(name, value);

				_serviceSettings.Add(new KeyPadSettingViewModel(setting));
			}
		}

		private void SaveSettings() {
			string msg = String.Empty;
			string newContent = String.Empty;
			for (int i = 0; i < _serviceSettings.Count; ++i) {
				string newVal = (String.IsNullOrEmpty(_serviceSettings[i].Value)) ? "NULL" : _serviceSettings[i].Value;
				newContent += $"{_serviceSettings[i].Name}={newVal}";

				if (i < _serviceSettings.Count)
					newContent += Environment.NewLine;
			}

			using (StreamWriter sw = new StreamWriter(_fileLocation)) {
				sw.Write(newContent);
			}

			LoadSettings(_fileLocation);
			PropertyChanged(this, new PropertyChangedEventArgs("ButtonEnabled"));
		}

	}

}