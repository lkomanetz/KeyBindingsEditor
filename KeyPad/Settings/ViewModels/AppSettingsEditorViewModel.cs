using KeyPad.DataManager;
using KeyPad.Settings.Models;
using KeyPad.Serializer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KeyPad.Settings.ViewModels {

	public class AppSettingsEditorViewModel : IViewModel, INotifyPropertyChanged {

		private IValidator _validator;
		private IDataManager _settingsManager;
		private IList<ApplicationSetting> _settings;
		private ApplicationSetting _startupSetting;
		private ApplicationSetting _locationSetting;
		private ApplicationSetting _processNameSetting;
		private bool _initialStartupValue;
		private string _initialLocationValue;
		private string _initialProcessNameValue;

		public AppSettingsEditorViewModel(IDataManager settingsDataManager) {
			_settings = (IList<ApplicationSetting>)settingsDataManager.Read();
			_settingsManager = settingsDataManager;
			_startupSetting = _settings.Where(x => x.Name.Equals("service_startup")).Single();
			_locationSetting = _settings.Where(x => x.Name.Equals("service_location")).Single();
			_processNameSetting = _settings.Where(x => x.Name.Equals("process_name")).Single();
			
			this.SaveCommand = new DelegateCommand<object>((param) => SaveSettings());

			_validator = new AppSettingsValidator(_settings);
			Initialize();
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public string Title => "Keypad Settings";
		public bool IsDirty =>
			(_initialStartupValue != (bool)_startupSetting.Value) ||
			(_initialLocationValue != _locationSetting.Value.ToString()) ||
			(_initialProcessNameValue != _processNameSetting.Value.ToString());

		public ICommand SaveCommand { get; private set; }

		public bool ShouldStartOnStartup {
			get => (bool)_startupSetting.Value;
			set {
				if ((bool)_startupSetting.Value != value) {
					_startupSetting.Value = value;
					PropertyChanged(this, new PropertyChangedEventArgs(nameof(ShouldStartOnStartup)));
					PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsDirty)));
				}
			}
		}

		public string ServiceLocation {
			get => _locationSetting.Value.ToString();
			set {
				if (_locationSetting.Value.ToString() != value) {
					_locationSetting.Value = value;
					_processNameSetting.Value = GetProcessName(value);
					PropertyChanged(this, new PropertyChangedEventArgs(nameof(ServiceLocation)));
					PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsDirty)));
				}
			}
		}

		private void SaveSettings() {
			var results = _validator.Validate();
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

			_settingsManager.Save(_settings);
			Initialize();
		}

		private void Initialize() {
			_initialStartupValue = (bool)_startupSetting.Value;
			_initialProcessNameValue = _processNameSetting.Value.ToString();
			_initialLocationValue = _locationSetting.Value.ToString();

			PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsDirty)));
		}

		private string GetProcessName(string fileLocation) {
			Match match = Regex.Match(fileLocation, @"[^\\]\w+(?=\.[a-zA-Z]{3}\z)");
			if (match.Success)
				return match.Value;
			else
				return String.Empty;
		}

	}

}