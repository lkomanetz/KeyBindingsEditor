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
		private static object lockObj = new Object();
		private IList<ApplicationSetting> _settings;
		private string _processName;
		private string _exeLocation;
		private Thread _watcherThread;
		private bool _isThreadRunning;
		private bool _wasProcessRunning;
		private bool _closeProcessOnExit;

		public WindowsProcessManager(string processName, string exeLocation, IList<ApplicationSetting> settings) {
			_processName = processName;
			_exeLocation = exeLocation;
			_isThreadRunning = true;
			_closeProcessOnExit = settings
				.Where(x => x.Name == "service_stop_on_close")
				.Select(x => (bool)x.Value)
				.Single();
		}

		public bool IsRunning { get; private set; }

		public event EventHandler<EventArgs> ProcessStarted;
		public event EventHandler<EventArgs> ProcessStopped;

		public void Start() {
			_watcherThread = new Thread(PollProcessState) {
				IsBackground = false
			};

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

			Process process = new Process() {
				StartInfo = info,
				EnableRaisingEvents = true
			};

			bool processStarted = process.Start();
			if (processStarted) {
				this.IsRunning = true;
				ProcessStarted(this, EventArgs.Empty);
			}

			_isThreadRunning = true;
			_watcherThread.Start();
		}

		public void Stop() {
			Process[] processes = Process.GetProcessesByName(_processName);
			foreach (Process process in processes) {
				process.Kill();
			}

			this.IsRunning = false;
			lock (lockObj) {
				_isThreadRunning = false;
			}
		}

		private bool IsProcessRunning() =>
			Process.GetProcessesByName(_processName).Length > 0;

		private void PollProcessState() {
			while (true) {
				long elapsedTime = Time(() => CheckProcessState());
				long delta = THREAD_INTERVAL_MILLIS - elapsedTime;
				if (!_isThreadRunning) break;
				if (delta > 0L) Thread.Sleep((int)delta);
			}

			long Time(Action action) {
				Stopwatch sw = new Stopwatch();
				sw.Start();
				action.Invoke();
				sw.Stop();

				return sw.ElapsedMilliseconds;
			}
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
			lock (lockObj) {
				_isThreadRunning = false;
			}

			if (this.IsRunning && _closeProcessOnExit) Stop();
		}

	}

}
