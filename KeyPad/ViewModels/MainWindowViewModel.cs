using KeyPad.KeyBindingsEditor.ViewModels;
using KeyPad.ProcessWatcher.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KeyPad.ViewModels {
	
	internal class MainWindowViewModel : INotifyPropertyChanged {

		public MainWindowViewModel() {
			this.ExitCommand = new DelegateCommand<object>((param) => Shutdown());
			this.OpenFileCommand = new DelegateCommand<object>((param) => OpenKeybindingsFile());

			this.NewFileCommand = new DelegateCommand<object>((param) => {
				this.PresenterViewModel = new KeyBindingsEditorViewModel();
			});

			this.ProcessWatcherViewModel = new ProcessWatcherViewModel("keypadservice");
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public ICommand ExitCommand { get; private set; }
		public ICommand OpenFileCommand { get; private set; }
		public ICommand NewFileCommand { get; private set; }

		private object _presenterViewModel;
		public object PresenterViewModel {
			get => _presenterViewModel;
			private set {
				_presenterViewModel = value;
				PropertyChanged(this, new PropertyChangedEventArgs("PresenterViewModel"));
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

	}

}
