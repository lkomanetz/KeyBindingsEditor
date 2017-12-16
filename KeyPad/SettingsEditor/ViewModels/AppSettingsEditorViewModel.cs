using KeyPad.ViewModels;
using KeyPad.DataManager;
using KeyPad.SettingsEditor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using KeyPad.Calculators;

namespace KeyPad.SettingsEditor.ViewModels {

	internal class AppSettingsEditorViewModel :
		IObservableViewModel,
		IViewModel,
		IForm {

		private IValidator _validator;
		private IDataManager _settingsManager;
		private IList<ApplicationSettingViewModel> _settingModels;
		private bool _initialCloseValue;
		private bool _initialStartupValue;
		private string _initialLocationValue;
		private string _initialProcessNameValue;

		public AppSettingsEditorViewModel(IDataManager settingsDataManager) {
			var settings = (IList<ApplicationSetting>)settingsDataManager.Read();
			_settingsManager = settingsDataManager;
			_settingModels = settings
				.Select(x => new ApplicationSettingViewModel(x, new Md5Calculator()))
				.ToList();
			
			this.SaveCommand = new DelegateCommand<object>((param) => SaveSettings());

			_validator = new AppSettingsValidator(_settings);
			Initialize();
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };
		public event EventHandler<EventArgs> SaveCompleted = delegate { };

		public string Title => "Keypad Settings";
		public bool IsDirty => _settingModels.Any(x => x.IsDirty);
		public ICommand SaveCommand { get; private set; }

		public bool ShouldStartOnStartup {
			get => (bool)GetSetting("service_startup").Value;
			set {
				var setting = GetSetting("service_startup");
				if ((bool)setting.Value != value) {
					setting.Value = value;
					PropertyChanged(this, new PropertyChangedEventArgs(nameof(ShouldStartOnStartup)));
					PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsDirty)));
				}
			}
		}

		public bool ShouldStopOnClose {
			get => (bool)GetSetting("service_stop_on_close").Value;
			set {
				var setting = GetSetting("service_stop_on_close");
				if ((bool)setting.Value == value) return;
				setting.Value = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(ShouldStopOnClose)));
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsDirty)));
			}
		}

		public string ServiceLocation {
			get => GetSetting("service_location").Value.ToString();
			set {
				var setting = GetSetting("service_location");
				if (setting.Value.ToString() != value) {
					setting.Value = value;
					GetSetting("process_name").Value = GetProcessName(value);
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
			SaveCompleted(this, EventArgs.Empty);
			Initialize();
		}

		private void Initialize() {
			PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsDirty)));
			PropertyChanged(this, new PropertyChangedEventArgs(nameof(ServiceLocation)));
			PropertyChanged(this, new PropertyChangedEventArgs(nameof(ShouldStartOnStartup)));
		}

		private string GetProcessName(string fileLocation) {
			Match match = Regex.Match(fileLocation, @"[^\\]\w+(?=\.[a-zA-Z]{3}\z)");
			if (match.Success)
				return match.Value;
			else
				return String.Empty;
		}

		private ApplicationSettingViewModel GetSetting(string name) =>
			_settingModels.Where(x => x.Name == name).Single();

	}

}