using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace KeyPad.ProcessWatcher.ViewModels {

	public class ProcessWatcherViewModel : INotifyPropertyChanged {

		private readonly object _threadKey = new object();

		private ICommand _stopProcessCommand;
		private ICommand _startProcessCommand;
		private System.Timers.Timer _executeThreadTimer;
		private Thread _watcherThread;
		private string _processName;
		private bool _isProcessRunning;

		public ProcessWatcherViewModel(string processName) {
			_processName = processName;
			_watcherThread = new Thread(() => WatchProcess(processName));
			_executeThreadTimer = new System.Timers.Timer() {
				Interval = 500.0D,
				Enabled = true
			};
			_executeThreadTimer.Elapsed += (sender, args) => {
				if (_watcherThread.ThreadState != System.Threading.ThreadState.Running)
					_watcherThread = new Thread(() => WatchProcess(_processName));

				_watcherThread.Start();
			};

			_startProcessCommand = new DelegateCommand<object>((param) => StartProcess());
			_stopProcessCommand = new DelegateCommand<object>((param) => StopProcess());
		} 

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public string LabelContent => (_isProcessRunning) ? "Running..." : "Stopped...";
		public string ButtonLabelContent => (_isProcessRunning) ? "Stop" : "Start";
		public ICommand ButtonCommand => (_isProcessRunning) ? _stopProcessCommand : _startProcessCommand;
		public Brush StatusColor => (_isProcessRunning) ? Brushes.Green : Brushes.Red;

		private void WatchProcess(string processName) {
			Process keypadProcess = Process.GetProcessesByName(processName).SingleOrDefault();

			_isProcessRunning = keypadProcess != null;
			InvokePropertyChangeEvents();
		}

		private void StartProcess() {
			ProcessStartInfo info = new ProcessStartInfo() {
				FileName = @"C:\Users\logan\Desktop\XKey\keypadservice.exe",
				UseShellExecute = true,
				WindowStyle = ProcessWindowStyle.Minimized
			};

			Process keypadProcess = new Process() { StartInfo = info };
			_isProcessRunning = keypadProcess.Start();
		}

		private void StopProcess() {
			Process keypadProcess = Process.GetProcessesByName(_processName).SingleOrDefault();
			if (keypadProcess != null) {
				keypadProcess.Kill();
				InvokePropertyChangeEvents();
			}
		}

		private void InvokePropertyChangeEvents() {
			PropertyChanged(this, new PropertyChangedEventArgs("StatusColor"));
			PropertyChanged(this, new PropertyChangedEventArgs("ButtonLabelContent"));
			PropertyChanged(this, new PropertyChangedEventArgs("ButtonCommand"));
			PropertyChanged(this, new PropertyChangedEventArgs("LabelContent"));
		}

	}

}
