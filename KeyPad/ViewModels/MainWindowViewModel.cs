using KeyPad.DataManager;
using KeyPad.KeyBindingsEditor.ViewModels;
using KeyPad.ProcessWatcher;
using KeyPad.ProcessWatcher.ViewModels;
using KeyPad.Settings.Models;
using KeyPad.Serializer;
using KeyPad.Settings.ViewModels;
using KeyPad.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using KeyPad.KeyBindingSelector.ViewModels;

namespace KeyPad.ViewModels {
	
	//TODO(Logan) -> Look at saving/loading key bindings to internal directory in the app.
	//TODO(Logan) -> Figure out how to edit bindings and settings with new UI.
	internal class MainWindowViewModel : IObservableViewModel {

		private const string APP_SETTINGS_FILE_LOCATION = "settings.json";
		private const string SERVICE_SETTINGS_FILE_LOCATION = "service_settings.txt";
		private IList<ApplicationSetting> _appSettings;
		private IProcessManager _processManager;
		private IDataManager _appSettingsManager;
		private IDataManager _serviceSettingsManager;
		private KeyBindingSelectorViewModel _kbSelectorVm;
		private IViewModel _processWatcherViewModel;

		public MainWindowViewModel() {
			_appSettingsManager = new AppSettingsManager(new SettingsJsonSerializer());
			_serviceSettingsManager = new ServiceSettingsManager(SERVICE_SETTINGS_FILE_LOCATION);
			_appSettings = (IList<ApplicationSetting>)_appSettingsManager.Read();

			_kbSelectorVm = new KeyBindingSelectorViewModel(_serviceSettingsManager);
#if DEBUG
			_processManager = SetupProcessMonitor();
			_processWatcherViewModel = new ProcessWatcherViewModel(_processManager);
#endif

			bool startService = (bool)_appSettings
				.Where(x => x.Name.Equals("service_startup"))
				.Single()
				.Value;

			if (startService) {
				_processManager.Start();
			}

			if (_processManager.IsRunning)
				_kbSelectorVm.Visibility = Visibility.Collapsed;	

			this.Cards = BuildCards();
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public bool IsDirty => false;

		private IList<IViewModel> _cards;
		public IList<IViewModel> Cards {
			get => _cards;
			set {
				_cards = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(Cards)));
			}
		}

		public static string APP_SETTINGS_FILE_LOCATION1 => APP_SETTINGS_FILE_LOCATION;

		private void Shutdown() => Application.Current.Shutdown();

		private IProcessManager SetupProcessMonitor() {
			var processNameSetting = _appSettings.First(x => x.Name.Equals("process_name"));
			var exeLocationSetting = _appSettings.First(x => x.Name.Equals("service_location"));

			var wpm = new WindowsProcessManager(
				processNameSetting.Value.ToString(),
				exeLocationSetting.Value.ToString()
			);
			wpm.ProcessStarted += (sender, args) => _kbSelectorVm.Visibility = Visibility.Collapsed;
			wpm.ProcessStopped += (sender, args) => _kbSelectorVm.Visibility = Visibility.Visible;

			return wpm;
		}

		private void OpenKeybindingsFile() {
			OpenFileDialog dlg = new OpenFileDialog() {
				DefaultExt = ".txt",
				Filter = "Text Files (*.txt) | *.txt"
			};
			bool? result = dlg.ShowDialog();
			if (result == false) {
				return;
			}
		}

		private IList<IViewModel> BuildCards() {
			return new List<IViewModel>() {
				new CardViewModel() {
					Title = "Service",
					TitleActions = new List<TitleAction>() {
						new TitleAction() {
							ActionImage = $@"{Environment.CurrentDirectory}/IconImages/edit_icon.png",
							Action = new DelegateCommand<object>((param) => MessageBox.Show("Hello"))
						}
					},
					CardContent = new List<IViewModel>() {
						_processWatcherViewModel
					}
				},
				new CardViewModel() {
					Title = "Key Bindings",
					TitleActions = new List<TitleAction>() {
						new TitleAction() {
							ActionImage= $@"{Environment.CurrentDirectory}/IconImages/edit_icon.png",
							Action = new DelegateCommand<object>((param) => MessageBox.Show("There"))
						},
						new TitleAction() {
							ActionImage = $@"{Environment.CurrentDirectory}/IconImages/add_icon.png",
							Action = new DelegateCommand<object>((param) => MessageBox.Show("Yay!"))
						}
					},
					CardContent = new List<IViewModel>() {
						_kbSelectorVm
					}
				}
			};
		}

	}

}
