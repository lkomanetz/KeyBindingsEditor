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

		private IList<IViewModel> _settings;
		private IValidator _validator;

		public AppSettingsEditorViewModel(IList<ApplicationSetting> settings) {
			_settings = new List<IViewModel>();
			foreach (var setting in settings) {
				var vm = ToViewModel(setting);
				vm.PropertyChanged += (sender, args) => OnSettingChanged();
				_settings.Add(vm);
			}

			this.SaveCommand = new DelegateCommand<object>((param) => SaveSettings());
			_validator = new AppSettingsValidator(settings); //TODO(Logan) -> FIX THIS!
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public IList<IViewModel> Settings => _settings;
		public string Title => "Keypad Settings";
		public bool IsDirty => _settings.Any(x => x.IsDirty);
		public ICommand SaveCommand { get; private set; }

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

		private void OnSettingChanged() => PropertyChanged(this, new PropertyChangedEventArgs("IsDirty"));

		private IViewModel ToViewModel(ApplicationSetting setting) {
			Type classType = typeof(ApplicationSettingViewModel<>);
			Type constructedType = classType.MakeGenericType(Type.GetType(setting.ValueType));
			return (IViewModel)Activator.CreateInstance(constructedType, new object[] { setting });
		}

	}

}