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

		private System.Timers.Timer _executeThreadTimer;
		private Thread _watcherThread;
		private Process _keypadProcess;
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

			this.StartProcessCommand = new DelegateCommand<object>((param) => StartProcess());
		} 

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public string LabelContent => "KeyPad Service Status:";
		public string StatusColor => (_isProcessRunning) ? Colors.Green.ToString() : Colors.DarkRed.ToString();
		public Visibility ButtonVisibility => (_isProcessRunning) ? Visibility.Collapsed : Visibility.Visible;
		public ICommand StartProcessCommand { get; private set; }

		private void WatchProcess(string processName) {
			_keypadProcess = Process.GetProcessesByName(processName).SingleOrDefault();
			bool isRunning = _keypadProcess != null;
			if (_isProcessRunning != isRunning) {
				_isProcessRunning = isRunning;
				PropertyChanged(this, new PropertyChangedEventArgs("StatusColor"));
				PropertyChanged(this, new PropertyChangedEventArgs("ButtonVisibility"));
			}
		}

		private void StartProcess() {
			ProcessStartInfo info = new ProcessStartInfo() {
				FileName = @"C:\users\logan\Desktop\XKey\keypadservice.exe",
				UseShellExecute = false,
				RedirectStandardOutput = true,
			};
			if (_keypadProcess == null)
			{
				Process process = new Process();
				process.StartInfo = info;
				process.Start();
			}
		}

	}

}
