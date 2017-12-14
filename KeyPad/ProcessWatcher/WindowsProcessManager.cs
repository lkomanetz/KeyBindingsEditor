using KeyPad.SettingsEditor.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KeyPad.ProcessWatcher {

	public class WindowsProcessManager : IProcessManager {

		private const long THREAD_INTERVAL_MILLIS = 500L;
		private static object key = new Object();
		private string _processName;
		private string _exeLocation;
		private Thread _watcherThread;
		private bool _isThreadRunning;
		private bool _wasProcessRunning;

		public WindowsProcessManager(string processName, string exeLocation) {
			_processName = processName;
			_exeLocation = exeLocation;
			_isThreadRunning = true;
		}


		public bool IsRunning { get; private set; }
		public string ProcessName => _processName;
		public string ExeLocation => _exeLocation;

		public event EventHandler<EventArgs> ProcessStarted;
		public event EventHandler<EventArgs> ProcessStopped;

		public void Start() {
			_watcherThread = new Thread(() => {
				while (_isThreadRunning) {
					long elapsedTime = Do(() => CheckProcessState());
					long delta = THREAD_INTERVAL_MILLIS - elapsedTime;
					if (delta > 0L) Thread.Sleep((int)delta);
				}
			});
			_watcherThread.IsBackground = true;

			if (String.IsNullOrEmpty(_exeLocation)) return;
			if (!File.Exists(_exeLocation)) return;

			if (!IsProcessRunning()) {
				this.IsRunning = true;
				ProcessStarted(this, EventArgs.Empty);
			}

			ProcessStartInfo info = new ProcessStartInfo() {
				FileName = _exeLocation,
				UseShellExecute = true,
				WindowStyle = ProcessWindowStyle.Minimized
			};

			Process process = new Process() { StartInfo = info, EnableRaisingEvents = true };

			bool processStarted = process.Start();
			if (processStarted) {
				this.IsRunning = true;
				ProcessStarted(this, EventArgs.Empty);
			}
			_watcherThread.Start();
		}

		public void Stop() {
			Process[] processes = Process.GetProcessesByName(_processName);
			foreach (Process process in processes) {
				process.Kill();
			}
			this.IsRunning = false;
			ProcessStopped(this, EventArgs.Empty);

			lock (key) {
				_isThreadRunning = false;
			}
		}

		private bool IsProcessRunning() {
			return Process.GetProcessesByName(_processName).Length > 0;
		}

		private long Do(Action action) {
			Stopwatch sw = new Stopwatch();
			sw.Start();
			action.Invoke();
			sw.Stop();

			return sw.ElapsedMilliseconds;
		}

		private void CheckProcessState() {
			bool processIsRunning = IsProcessRunning();
			if (_wasProcessRunning && !processIsRunning) {
				this.IsRunning = false;
				ProcessStopped(this, EventArgs.Empty);
			}
			else if (!_wasProcessRunning && processIsRunning) {
				this.IsRunning = true;
				ProcessStarted(this, EventArgs.Empty);
			}
			_wasProcessRunning = processIsRunning;
		}

		public void Dispose() {
			lock (key) {
				_isThreadRunning = false;
			}
		}

	}

}
