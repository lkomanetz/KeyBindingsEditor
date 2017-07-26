using KeyPad.Settings.Models;
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

		private IList<ApplicationSettingViewModel> _settings;
		private IValidator _validator;

		public AppSettingsEditorViewModel(IList<ApplicationSetting> settings) {
			_settings = new List<ApplicationSettingViewModel>();
			foreach (var setting in settings) {
				var vm = new ApplicationSettingViewModel(setting);
				vm.PropertyChanged += (sender, args) => OnSettingChanged();
				_settings.Add(vm);
			}

			this.SaveCommand = new DelegateCommand<object>((param) => SaveSettings());
			_validator = new AppSettingsValidator(_settings);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public IList<ApplicationSettingViewModel> Settings => _settings;
		public string Title => "Keypad Settings";
		public bool ButtonEnabled => _settings.Any(x => x.IsDirty);
		public ICommand SaveCommand { get; private set; }

		private void SaveSettings() {
			var result = _validator.Validate();
			if (!result.IsSuccess) {
				MessageBox.Show(
					result.Message,
					this.Title,
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
				return;
			}

			foreach (var setting in _settings)
				ConfigurationManager.AppSettings[setting.Name] = setting.Value.ToString();
		}

		private void OnSettingChanged() => PropertyChanged(this, new PropertyChangedEventArgs("ButtonEnabled"));

	}

}