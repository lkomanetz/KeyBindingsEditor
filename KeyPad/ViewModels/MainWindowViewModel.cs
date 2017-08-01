using KeyPad.DataManager;
using KeyPad.KeyBindingsEditor.ViewModels;
using KeyPad.ProcessWatcher;
using KeyPad.ProcessWatcher.ViewModels;
using KeyPad.Settings.Models;
using KeyPad.Serializer;
using KeyPad.Settings.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;

namespace KeyPad.ViewModels {
	
	internal class MainWindowViewModel : INotifyPropertyChanged {

		private const string SETTINGS_FILE_LOCATION = "settings.json";
		private IList<ApplicationSetting> _appSettings;
		private IProcessManager _processManager;
		private IDataManager _settingsManager;

		public MainWindowViewModel() {
			_settingsManager = new AppSettingsManager(new SettingsJsonSerializer());
			_appSettings = (IList<ApplicationSetting>)_settingsManager.Read();
			this.TopMenu = CreateMenu();

			ApplicationSetting processName = _appSettings
				.Where(x => x.Name.Equals("process_name"))
				.Single();
			ApplicationSetting exeLocation = _appSettings
				.Where(x => x.Name.Equals("service_location"))
				.Single();

			_processManager = new WindowsProcessManager(processName.Value.ToString(), exeLocation.Value.ToString());
			this.ProcessWatcherViewModel = new ProcessWatcherViewModel(_processManager);

			bool startService = (bool)_appSettings
				.Where(x => x.Name.Equals("service_startup"))
				.Single()
				.Value;

			if (startService) {
				_processManager.Start();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public IList<IMenuItem> TopMenu { get; private set; }

		private object _presenterViewModel;
		public object PresenterViewModel {
			get => _presenterViewModel;
			private set {
				_presenterViewModel = value;
				PropertyChanged(this, new PropertyChangedEventArgs("PresenterViewModel"));
				PropertyChanged(this, new PropertyChangedEventArgs("HeaderVisibility"));
				PropertyChanged(this, new PropertyChangedEventArgs("Title"));
			}
		}

		private object _processWatcherViewModel;
		public object ProcessWatcherViewModel {
			get => _processWatcherViewModel;
			private set {
				_processWatcherViewModel = value;
				PropertyChanged(this, new PropertyChangedEventArgs("ProcessWatcherViewModel"));
			}
		}

		public Visibility HeaderVisibility => (this.PresenterViewModel != null) ? Visibility.Visible : Visibility.Collapsed;
		private void Shutdown() => Application.Current.Shutdown();

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
				new BindingFileManager(dlg.FileName)
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
									Action = new DelegateCommand<object>((param) => PresenterViewModel = new ServiceSettingsViewModel("settings.txt")),
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
