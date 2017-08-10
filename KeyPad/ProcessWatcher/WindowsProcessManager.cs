using KeyPad.SettingsEditor.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.ProcessWatcher {

	public class WindowsProcessManager : IProcessManager {

		private string _processName;
		private string _exeLocation;

		public WindowsProcessManager(string processName, string exeLocation) {
			_processName = processName;
			_exeLocation = exeLocation;
		}

		public bool IsRunning { get; private set; }
		public string ProcessName => _processName;
		public string ExeLocation => _exeLocation;

		public event EventHandler<EventArgs> ProcessStarted;
		public event EventHandler<EventArgs> ProcessStopped;

		public void Start() {
			if (String.IsNullOrEmpty(_exeLocation))
				return;

			if (IsProcessRunning()) {
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
		}

		public void Stop() {
			Process[] processes = Process.GetProcessesByName(_processName);
			foreach (Process process in processes) {
				process.Kill();
			}
			ProcessStopped(this, EventArgs.Empty);
		}

		private bool IsProcessRunning() {
			return Process.GetProcessesByName(_processName).Length > 0;
		}

	}

}
