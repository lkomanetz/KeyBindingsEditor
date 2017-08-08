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
using KeyPad.UserControls.ViewModels;
using KeyPad.Models;

namespace KeyPad.ViewModels {
	
	//TODO(Logan) -> Figure out how to handle the Environment.CurrentDirectory/Bindings/<blah> nonsense.
	//TODO(Logan) -> Use the modal dialog concept in an abstract way.
	internal class MainWindowViewModel : IObservableViewModel {

		private const string APP_SETTINGS_FILE_LOCATION = "settings.json";
		private const string SERVICE_SETTINGS_FILE_LOCATION = "service_settings.txt";
		private IList<ApplicationSetting> _appSettings;
		private IProcessManager _processManager;
		private IDataManager _appSettingsManager;
		private IDataManager _serviceSettingsManager;
		private IDataManager _keyBindingDataManager;
		private KeyBindingSelectorViewModel _kbSelectorVm;
		private IViewModel _processWatcherViewModel;
		private Window _windowOwner;

		public MainWindowViewModel(Window owner) {
			_windowOwner = owner;
			_keyBindingDataManager = new KeyBindingFileManager();
			_keyBindingDataManager.SaveComplete += (sender, args) => _kbSelectorVm.LoadFiles();

			_appSettingsManager = new AppSettingsManager(new SettingsJsonSerializer());
			_serviceSettingsManager = new ServiceSettingsManager(SERVICE_SETTINGS_FILE_LOCATION);
			_appSettings = (IList<ApplicationSetting>)_appSettingsManager.Read();

			_kbSelectorVm = new KeyBindingSelectorViewModel(_serviceSettingsManager, _keyBindingDataManager);
#if !DEBUG
			_processManager = SetupProcessMonitor();
			_processWatcherViewModel = new ProcessWatcherViewModel(_processManager);

			bool startService = (bool)_appSettings
				.Where(x => x.Name.Equals("service_startup"))
				.Single()
				.Value;

			if (startService) {
				_processManager.Start();
			}

			if (_processManager.IsRunning)
				_kbSelectorVm.Visibility = Visibility.Collapsed;	
#endif

			this.Cards = BuildCards();
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public bool IsDirty => false;

		private IList<CardViewModel> _cards;
		public IList<CardViewModel> Cards {
			get => _cards;
			set {
				_cards = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(Cards)));
			}
		}

		private IForm _formViewContent;
		public IForm FormViewContent {
			get => _formViewContent;
			set {
				if (value != null) 
					value.SaveCompleted += (sender, args) => this.FormViewContent = null;
				
				_formViewContent = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(FormViewContent)));
			}
		}

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

		private IList<CardViewModel> BuildCards() {
			IList<CardViewModel> cardMenu = new List<CardViewModel>();

			var root = new CardViewModel() {
				Title = "Application",
				TitleActions = new List<TitleAction>() {
					new TitleAction() {
						ActionImage = $"{Environment.CurrentDirectory}/IconImages/edit_icon.png",
						Action = new DelegateCommand<object>((param) =>
							this.FormViewContent = new AppSettingsEditorViewModel(_appSettingsManager)
						)
					}
				}
			};

			var children = new List<IViewModel>() {
				new CardViewModel() {
					Title = "Service",
					TitleActions = new List<TitleAction>() {
						new TitleAction() {
							ActionImage = $@"{Environment.CurrentDirectory}/IconImages/edit_icon.png",
							Action = new DelegateCommand<object>((param) =>
								this.FormViewContent = new ServiceSettingsViewModel(_serviceSettingsManager)
							)
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
							ActionImage = $@"{Environment.CurrentDirectory}/IconImages/delete_icon.png",
							Action = new DelegateCommand<object>((param) => _kbSelectorVm.DeleteSelectedKeyBinding())
						},
						new TitleAction() {
							ActionImage= $@"{Environment.CurrentDirectory}/IconImages/edit_icon.png",
							Action = new DelegateCommand<object>((param) => {
								this.FormViewContent = new KeyBindingsEditorViewModel(
									_keyBindingDataManager,
									_kbSelectorVm.SelectedFile,
									_windowOwner
								);
							})
						},
						new TitleAction() {
							ActionImage = $@"{Environment.CurrentDirectory}/IconImages/add_icon.png",
							Action = new DelegateCommand<object>((param) => {
								this.FormViewContent = new KeyBindingsEditorViewModel(
									_keyBindingDataManager,
									(KeyBindingFile)param,
									_windowOwner
								);
							})
						}
					},
					CardContent = new List<IViewModel>() {
						_kbSelectorVm
					}
				}
			};

			root.CardContent = children;
			cardMenu.Add(root);

			return cardMenu;
		}

	}

}
