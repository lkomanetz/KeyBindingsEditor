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
        private IViewModel _shouldStartOnStartupVm;
        private IViewModel _serviceLocationVm;
        private IList<ApplicationSetting> _settings;
        private ISerializer _settingsSerializer;

		public AppSettingsEditorViewModel(IList<ApplicationSetting> settings) {
            _settingsSerializer = new SettingsJsonSerializer();
            _settings = settings;

            foreach (var setting in settings) {
                var vm = ToViewModel(setting);
                vm.PropertyChanged += (sender, args) => OnSettingChanged();
                if (setting.Name.Equals("service_startup")) {
                    _shouldStartOnStartupVm = vm;
                }
                else if (setting.Name.Equals("service_location")) {
                    _serviceLocationVm = vm;
                }
            }

			this.SaveCommand = new DelegateCommand<object>((param) => SaveSettings());
            this.FindServiceCommand = new DelegateCommand<object>((param) => GetServiceLocation());

            _validator = new AppSettingsValidator(_settings); //TODO(Logan) -> FIX THIS!
        }

		public event PropertyChangedEventHandler PropertyChanged;

		public string Title => "Keypad Settings";
		public bool IsDirty => _serviceLocationVm.IsDirty || _shouldStartOnStartupVm.IsDirty;
		public ICommand SaveCommand { get; private set; }
        public ICommand FindServiceCommand { get; private set; }

        public bool ShouldStartOnStartup {
            get => (bool)(((ISetting<bool>)_shouldStartOnStartupVm).Value);
            set {
                var setting = (ISetting<bool>)_shouldStartOnStartupVm;
                if (setting.Value != value) {
                    setting.Value = value;
                    UpdateSetting("service_startup", setting.Value);
                    PropertyChanged(this, new PropertyChangedEventArgs("ShouldStartOnStartup"));
                }
            }
        }

        public string ServiceLocation {
            get => ((ISetting<FileLocation>)_serviceLocationVm).Value.ToString();
            set {
                var setting = (ISetting<FileLocation>)_serviceLocationVm;
                if (setting.Value.ToString() != value) {
                    setting.Value = new FileLocation(value);
                    UpdateSetting("service_location", setting.Value);
                    PropertyChanged(this, new PropertyChangedEventArgs("ServiceLocation"));
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

			//TODO(Logan) -> Re-implement saving changes to KeyPad settings
            //TODO(Logan) -> Serializer doesn't know how to handle FileLocation
            string settingsJson = _settingsSerializer.Serialize(_settings);
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

        private void UpdateSetting(string settingName, object newValue) {
            var setting = _settings
                .Where(x => x.Name.Equals(settingName))
                .Single();

            setting.Value = newValue;
        }

		private void OnSettingChanged() => PropertyChanged(this, new PropertyChangedEventArgs("IsDirty"));

        private IViewModel ToViewModel(ApplicationSetting setting) {
            Type classType = typeof(ApplicationSettingViewModel<>);
            Type constructedType = classType.MakeGenericType(Type.GetType(setting.ValueType));
            return (IViewModel)Activator.CreateInstance(constructedType, new object[] { setting });
        }

	}

}