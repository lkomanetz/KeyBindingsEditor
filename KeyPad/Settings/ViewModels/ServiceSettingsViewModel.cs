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

	public class ServiceSettingsViewModel : IViewModel {
		private IDataManager _dataManager;
		private IList<KeyPadSettingViewModel> _serviceSettings;

		public ServiceSettingsViewModel(IDataManager dataManager) {
			_dataManager = dataManager;
			_serviceSettings = new List<KeyPadSettingViewModel>();
			LoadSettings();

			this.SaveCommand = new DelegateCommand<object>((param) => SaveSettings());
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public string Title => "KeyPad Service Settings";
		public IList<KeyPadSettingViewModel> Settings => _serviceSettings;
		public bool IsDirty => _serviceSettings.Any(x => x.IsDirty);
		public ICommand SaveCommand { get; private set; }

		private void LoadSettings() {
			if (_serviceSettings.Count > 0)
				_serviceSettings.Clear();

			var settings = (IList<KeyPadServiceSetting>)_dataManager.Read();
			_serviceSettings = settings.Select(x => {
				var vm = new KeyPadSettingViewModel(x);
				vm.PropertyChanged += SettingChanged;
				return vm;
			})
			.ToList();
		}

		private void SaveSettings() {
			_dataManager.Save(_serviceSettings);
			LoadSettings();
		}

		private void SettingChanged(object sender, PropertyChangedEventArgs e) =>
			PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsDirty)));

	}

}