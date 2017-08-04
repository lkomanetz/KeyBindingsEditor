using KeyPad.ViewModels;
using KeyPad.DataManager;
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

	public class ServiceSettingsViewModel : IViewModel, IObservableViewModel, IForm {
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
		public IList<ServiceSettingViewModel> Settings => _serviceSettings;
		public bool IsDirty => _serviceSettings.Any(x => x.IsDirty);
		public ICommand SaveCommand { get; private set; }

		private void LoadSettings() {
			if (_serviceSettings.Count > 0)
				_serviceSettings.Clear();

			var settings = (IList<ServiceSetting>)_dataManager.Read();
			_serviceSettings = settings.Select(x => {
				var vm = new ServiceSettingViewModel(x);
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