using KeyPad.Settings.Models;
using KeyPad.Settings.Serializer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KeyPad.Settings.ViewModels {

	public class AppSettingsEditorViewModel : IViewModel, INotifyPropertyChanged {

		private IValidator _validator;
		private IList<ApplicationSetting> _settings;
		private ApplicationSetting _startupSetting;
		private ApplicationSetting _locationSetting;
		private ApplicationSetting _processNameSetting;
		private ISerializer _settingsSerializer;
		private bool _initialStartupValue;
		private string _initialLocationValue;
		private string _initialProcessNameValue;

		public AppSettingsEditorViewModel(IList<ApplicationSetting> settings) {
			_settingsSerializer = new SettingsJsonSerializer();
			_settings = settings;

			_startupSetting = settings.Where(x => x.Name.Equals("service_startup")).Single();
			_locationSetting = settings.Where(x => x.Name.Equals("service_location")).Single();
			_processNameSetting = settings.Where(x => x.Name.Equals("process_name")).Single();
			
			_initialStartupValue = (bool)_startupSetting.Value;
			_initialLocationValue = _locationSetting.Value.ToString();
			_initialProcessNameValue = _processNameSetting.Value.ToString();

			this.SaveCommand = new DelegateCommand<object>((param) => SaveSettings());
			this.FindServiceCommand = new DelegateCommand<object>((param) => GetServiceLocation());

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
		public ICommand FindServiceCommand { get; private set; }

		public bool ShouldStartOnStartup {
			get => (bool)_startupSetting.Value;
			set {
				if ((bool)_startupSetting.Value != value) {
					_startupSetting.Value = value;
					PropertyChanged(this, new PropertyChangedEventArgs("ShouldStartOnStartup"));
					PropertyChanged(this, new PropertyChangedEventArgs("IsDirty"));
				}
			}
		}

		public string ServiceLocation {
			get => _locationSetting.Value.ToString();
			set {
				if (_locationSetting.Value.ToString() != value) {
					_locationSetting.Value = value;
					PropertyChanged(this, new PropertyChangedEventArgs("ServiceLocation"));
					PropertyChanged(this, new PropertyChangedEventArgs("IsDirty"));
				}
			}
		}

		public string ProcessName {
			get => _processNameSetting.Value.ToString();
			set {
				if (_processNameSetting.Value.ToString() != value) {
					_processNameSetting.Value = value;
					PropertyChanged(this, new PropertyChangedEventArgs("ServiceLocation"));
					PropertyChanged(this, new PropertyChangedEventArgs("IsDirty"));
				}
			}
		}

		private void SaveSettings() {
			var results = _validator.Validate();
			if (results.Any(x => !x.IsSuccess)) {
				string msg = BuildValidationMessage(results);
				MessageBox.Show(
					msg,
					this.Title,
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
				return;
			}

			string settingsJson = _settingsSerializer.Serialize(_settings);
			System.IO.File.WriteAllText("settings.json", settingsJson);
			Initialize();
		}

		private string BuildValidationMessage(IList<ValidatorResult> results) {
			String result = String.Empty;
			if (results.All(x => x.IsSuccess))
				return result;

			var failedResults = results.Where(x => !x.IsSuccess).ToList();
			foreach (var failedResult in failedResults) {
				result += $"{failedResult.Message}\n";
			}

			return result;
		}

		private void GetServiceLocation() {
			OpenFileDialog dlg = new OpenFileDialog() {
				Filter = "Executable files (*.exe) | *.exe"
			};

			bool? result = dlg.ShowDialog();
			if (result == false)
				return;

			this.ServiceLocation = dlg.FileName;
		}

		private void Initialize() {
			_initialStartupValue = (bool)_startupSetting.Value;
			_initialProcessNameValue = _processNameSetting.Value.ToString();
			_initialLocationValue = _locationSetting.Value.ToString();

			PropertyChanged(this, new PropertyChangedEventArgs("IsDirty"));
		}

	}

}