using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace KeyPad.ProcessWatcher.ViewModels {

	internal class ProcessWatcherViewModel : INotifyPropertyChanged {

		private const double TIMER_INTERVAL = 500D;
		private ICommand _stopProcessCommand;
		private ICommand _startProcessCommand;
		private System.Timers.Timer _executeThreadTimer;
		private Thread _watcherThread;
		private IProcessManager _processManager;
		private bool _isProcessRunning;

		public ProcessWatcherViewModel(IProcessManager processManager) {
			_processManager = processManager;
			_processManager.ProcessStarted += (sender, args) => InvokePropertyChangeEvents();
			_processManager.ProcessStopped += (sender, args) => InvokePropertyChangeEvents();

			_watcherThread = new Thread(() => WatchProcess());
			_executeThreadTimer = new System.Timers.Timer() {
				Interval = TIMER_INTERVAL,
				Enabled = true
			};
			_executeThreadTimer.Elapsed += (sender, args) => {
				if (_watcherThread.ThreadState != ThreadState.Running)
					_watcherThread = new Thread(() => WatchProcess());

				_watcherThread.Start();
			};

			_startProcessCommand = new DelegateCommand<object>((param) => _processManager.Start());
			_stopProcessCommand = new DelegateCommand<object>((param) => _processManager.Stop());
		} 

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public string LabelContent => (_isProcessRunning) ? "Running..." : "Stopped...";
		public string ButtonLabelContent => (_isProcessRunning) ? "Stop" : "Start";
		public ICommand ButtonCommand => (_isProcessRunning) ? _stopProcessCommand : _startProcessCommand;
		public Brush StatusColor => (_isProcessRunning) ? Brushes.Green : Brushes.Red;

		private void WatchProcess() {
			_isProcessRunning = _processManager.IsRunning;
			InvokePropertyChangeEvents();
		}

		private void InvokePropertyChangeEvents() {
			PropertyChanged(this, new PropertyChangedEventArgs("StatusColor"));
			PropertyChanged(this, new PropertyChangedEventArgs("ButtonLabelContent"));
			PropertyChanged(this, new PropertyChangedEventArgs("ButtonCommand"));
			PropertyChanged(this, new PropertyChangedEventArgs("LabelContent"));
		}

	}

}
