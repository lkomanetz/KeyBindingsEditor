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
	//TODO(Logan) -> Fix bug with saving new keybindings without choosing a file.
	internal class MainWindowViewModel : IObservableViewModel {

		private const string APP_SETTINGS_FILE_LOCATION = "settings.json";
		private const string SERVICE_SETTINGS_FILE_LOCATION = "service_settings.txt";
		private IList<ApplicationSetting> _appSettings;
		private IProcessManager _processManager;
		private IDataManager _appSettingsManager;
		private IDataManager _serviceSettingsManager;

		public MainWindowViewModel() {
			_appSettingsManager = new AppSettingsManager(new SettingsJsonSerializer());
			_serviceSettingsManager = new ServiceSettingsManager(SERVICE_SETTINGS_FILE_LOCATION);
			_appSettings = (IList<ApplicationSetting>)_appSettingsManager.Read();
			this.TopMenu = CreateMenu();

			ApplicationSetting processName = _appSettings
				.Where(x => x.Name.Equals("process_name"))
				.Single();
			ApplicationSetting exeLocation = _appSettings
				.Where(x => x.Name.Equals("service_location"))
				.Single();

#if !DEBUG
			_processManager = SetupProcessMonitor(processName.Value.ToString(), exeLocation.Value.ToString());
			this.ProcessWatcherViewModel = new ProcessWatcherViewModel(_processManager);
#endif

			bool startService = (bool)_appSettings
				.Where(x => x.Name.Equals("service_startup"))
				.Single()
				.Value;

			if (startService) 
				_processManager.Start();

			this.KeyBindingSelectorViewModel = new KeyBindingSelectorViewModel(_serviceSettingsManager);
			this.KeyBindingSelectorVisibility = (_processManager.IsRunning) ? Visibility.Collapsed : Visibility.Visible;
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public IList<IMenuItem> TopMenu { get; private set; }
		public bool IsDirty => false;

		private Visibility _kbSelectorVisibility;
		public Visibility KeyBindingSelectorVisibility
		{
			get => _kbSelectorVisibility;
			set {
				if (_kbSelectorVisibility == value)
					return;

				_kbSelectorVisibility = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(KeyBindingSelectorVisibility)));
			}
		}

		private IViewModel _presenterViewModel;
		public IViewModel PresenterViewModel {
			get => _presenterViewModel;
			private set {
				_presenterViewModel = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(PresenterViewModel)));
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(HeaderVisibility)));
			}
		}

		private IViewModel _processWatcherViewModel;
		public IViewModel ProcessWatcherViewModel {
			get => _processWatcherViewModel;
			private set {
				_processWatcherViewModel = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(ProcessWatcherViewModel)));
			}
		}

		private IViewModel _kbSelectorVm;
		public IViewModel KeyBindingSelectorViewModel {
			get => _kbSelectorVm;
			private set {
				_kbSelectorVm = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(KeyBindingSelectorViewModel)));
			}
		}

		public Visibility HeaderVisibility => (this.PresenterViewModel != null) ? Visibility.Visible : Visibility.Collapsed;
		public static string APP_SETTINGS_FILE_LOCATION1 => APP_SETTINGS_FILE_LOCATION;

		private void Shutdown() => Application.Current.Shutdown();

		private IProcessManager SetupProcessMonitor(string processName, string exeLocation) {
			var wpm = new WindowsProcessManager(processName, exeLocation);
			wpm.ProcessStarted += (sender, args) => this.KeyBindingSelectorVisibility = Visibility.Collapsed;
			wpm.ProcessStopped += (sender, args) => this.KeyBindingSelectorVisibility = Visibility.Visible;

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

			this.PresenterViewModel = new KeyBindingsEditorViewModel(
				new KeyBindingFileManager(dlg.FileName)
			);
		}

		private IList<IMenuItem> CreateMenu() {
			return new List<IMenuItem>() {
				new TopBarMenuItem() {
					Title = "File",
					Children = new List<IMenuItem>() {
						new TopBarMenuItem() {
							Title = "New",
							Action = new DelegateCommand<object>((param) => PresenterViewModel = new KeyBindingsEditorViewModel())
						},
						new TopBarMenuItem() {
							Title = "Open",
							Action = new DelegateCommand<object>((param) => OpenKeybindingsFile())
						},
						new TopBarMenuItem() {
							Title = "Settings",
							Children = new List<IMenuItem>() {
								new TopBarMenuItem() {
									Title = "Service settings",
									Action = new DelegateCommand<object>((param) => PresenterViewModel = new ServiceSettingsViewModel(_serviceSettingsManager)),
								},
								new TopBarMenuItem() {
									Title = "KeyPad settings",
									Action = new DelegateCommand<object>((param) => PresenterViewModel = new AppSettingsEditorViewModel(new AppSettingsManager(new SettingsJsonSerializer())))
								}
							}
						},
						new TopBarMenuItem() {
							Title = "Exit",
							Action = new DelegateCommand<object>((param) => Shutdown())
						}
					}
				}
			};
		}

	}

}
