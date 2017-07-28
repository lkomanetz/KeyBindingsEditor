using KeyPad.KeyBindingsEditor.ViewModels;
using KeyPad.ProcessWatcher.ViewModels;
using KeyPad.Settings.Models;
using KeyPad.Settings.UserControls.ViewModels;
using KeyPad.Settings.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KeyPad.ViewModels {
	
	internal class MainWindowViewModel : INotifyPropertyChanged {
		private const string SETTINGS_FILE_LOCATION = "settings.json";
		private IList<ApplicationSetting> _appSettings;

		public MainWindowViewModel() {
			this.TopMenu = CreateMenu();
#if !DEBUG
			this.ProcessWatcherViewModel = new ProcessWatcherViewModel("keypadservice");
#endif
			LoadAppSettings();
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

			this.PresenterViewModel = new KeyBindingsEditorViewModel(dlg.FileName);
		}

		private void LoadAppSettings() {
			string jsonString = System.IO.File.ReadAllText(SETTINGS_FILE_LOCATION);
			_appSettings = new Settings.Serializer.SettingsJsonSerializer().Deserialize<ApplicationSetting[]>(jsonString);

			var serviceLocationSetting = _appSettings.Where(x => x.Name.Equals("service_location")).Single();
			serviceLocationSetting.Value = new FileLocation(serviceLocationSetting.Value.ToString());
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
									Action = new DelegateCommand<object>((param) => PresenterViewModel = new AppSettingsEditorViewModel(_appSettings))
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
