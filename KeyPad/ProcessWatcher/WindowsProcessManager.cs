using KeyPad.Settings.Models;
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
		private Process _process;

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

			ProcessStartInfo info = new ProcessStartInfo() {
				FileName = _exeLocation,
				UseShellExecute = true,
				WindowStyle = ProcessWindowStyle.Minimized
			};

			_process = new Process() { StartInfo = info, EnableRaisingEvents = true };
			_process.Exited += (sender, args) => {
				this.IsRunning = false;
				ProcessStopped(this, EventArgs.Empty);
			};

			bool processStarted = _process.Start();
			if (processStarted) {
				this.IsRunning = true;
				ProcessStarted(this, EventArgs.Empty);
			}
		}

		public void Stop() {
			_process.Kill();
			_process.Dispose();
			_process = null;
		}

	}

}
