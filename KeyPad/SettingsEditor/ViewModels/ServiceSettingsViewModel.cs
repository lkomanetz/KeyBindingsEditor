using KeyPad.ViewModels;
using KeyPad.DataManager;
using KeyPad.SettingsEditor.Models;
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
using KeyPad.Calculators;

namespace KeyPad.SettingsEditor.ViewModels {

	internal class ServiceSettingsViewModel : IFormViewModel, IObservableViewModel, IForm {
		private IDataManager _dataManager;
		private IList<ServiceSettingViewModel> _serviceSettings;

		public ServiceSettingsViewModel(IDataManager dataManager) {
			_dataManager = dataManager;
			_serviceSettings = new List<ServiceSettingViewModel>();
			LoadSettings();

			this.SaveCommand = new DelegateCommand<object>((param) => SaveSettings());
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };
		public event EventHandler<EventArgs> SaveCompleted = delegate { };

		public string Title => "KeyPad Service Settings";

		public IList<ServiceSettingViewModel> Settings => _serviceSettings
			.Where(x => !x.Name.Equals(ServiceSettingNames.KEYBINDINGS_LOCATION_SETTING))
			.ToList();

		public bool IsDirty => _serviceSettings.Any(x => x.IsDirty);
		public ICommand SaveCommand { get; private set; }

		private void LoadSettings() {
			if (_serviceSettings.Count > 0) _serviceSettings.Clear();
			var settings = (IList<ServiceSetting>)_dataManager.Read();

			/*
			 * The ServiceSettingViewModel has a UIElement property that is creating at runtime.
			 * The UIElement binds to the ServiceSettingViewModel's Value property.  When the value
			 * property's content changes it'll call ServiceSettingViewModel.PropertyChanged.  I'm
			 * subscribing to that event and calling this class's PropertyChanged so I can enable
			 * or disable the Save button in the ServiceSettings.xaml page.
			 */
			_serviceSettings = settings.Select(x => {
				var vm = new ServiceSettingViewModel(x, new Md5Calculator());
				vm.PropertyChanged += (sender, e) => PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsDirty)));
				return vm;
			})
			.ToList();

			PropertyChanged(this, new PropertyChangedEventArgs(nameof(Settings)));
		}

		private void SaveSettings() {
			var settings = _serviceSettings
				.Select(x => new ServiceSetting(x.Name, x.Value))
				.ToList();

			_dataManager.Save(settings);
			LoadSettings();
			PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsDirty)));
			SaveCompleted(this, EventArgs.Empty);
		}

	}

}