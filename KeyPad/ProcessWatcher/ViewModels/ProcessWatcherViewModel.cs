using KeyPad.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace KeyPad.ProcessWatcher.ViewModels {

	internal class ProcessWatcherViewModel : IObservableViewModel {

		private ICommand _stopProcessCommand;
		private ICommand _startProcessCommand;
		private IProcessManager _processManager;
		private bool _isProcessRunning;
		private bool _buttonEnabled;

		public ProcessWatcherViewModel(IProcessManager processManager) {
			_buttonEnabled = true;
			_processManager = processManager;
			_processManager.ProcessStarted += (sender, args) => {
				_isProcessRunning = true;
				InvokePropertyChangeEvents();
			};
			_processManager.ProcessStopped += (sender, args) => {
				_isProcessRunning = false;
				InvokePropertyChangeEvents();
			};

			_startProcessCommand = new DelegateCommand<object>((param) => ToggleActionAsync(() => _processManager.Start()));
			_stopProcessCommand = new DelegateCommand<object>((param) => ToggleActionAsync(() => _processManager.Stop()));
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public string LabelContent => (_isProcessRunning) ? "Running..." : "Stopped...";
		public string ButtonLabelContent => (_isProcessRunning) ? "Stop" : "Start";

		public bool ButtonEnabled {
			get => _buttonEnabled;
			private set {
				if (_buttonEnabled == value)
					return;

				_buttonEnabled = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(ButtonEnabled)));
			}
		}

		public ICommand ButtonCommand => (_isProcessRunning) ? _stopProcessCommand : _startProcessCommand;
		public Brush StatusColor => (_isProcessRunning) ? Brushes.Green : Brushes.Red;

		private async void ToggleActionAsync(Action action) {
			this.ButtonEnabled = false;
			await Task.Factory.StartNew(() => action?.Invoke());
			this.ButtonEnabled = true;
		}

		private void InvokePropertyChangeEvents() {
			PropertyChanged(this, new PropertyChangedEventArgs(nameof(ButtonLabelContent)));
			PropertyChanged(this, new PropertyChangedEventArgs(nameof(ButtonCommand)));
			PropertyChanged(this, new PropertyChangedEventArgs(nameof(LabelContent)));
			PropertyChanged(this, new PropertyChangedEventArgs(nameof(StatusColor)));
		}

	}

}
