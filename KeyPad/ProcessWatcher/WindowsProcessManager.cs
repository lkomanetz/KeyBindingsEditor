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

		public WindowsProcessManager(string processName, string exeLocation) {
			_processName = processName;
			_exeLocation = exeLocation;
		}

		public bool IsRunning => CheckProcessIsRunning();
		public string ProcessName => _processName;
		public string ExeLocation => _exeLocation;

		public event EventHandler<EventArgs> ProcessStarted;
		public event EventHandler<EventArgs> ProcessStopped;

		public bool Start() {
			ProcessStartInfo info = new ProcessStartInfo() {
				FileName = _exeLocation,
				UseShellExecute = true,
				WindowStyle = ProcessWindowStyle.Minimized
			};

			var keypadProcess = new Process() { StartInfo = info };

			bool processStarted = keypadProcess.Start();
			if (processStarted)
				ProcessStarted(this, EventArgs.Empty);

			return processStarted;
		}

		public void Stop() {
			Process keypadProcess = Process.GetProcessesByName(_processName).SingleOrDefault();
			if (keypadProcess == null)
				return;

			keypadProcess.Kill();
			ProcessStopped(this, EventArgs.Empty);
		}

		private bool CheckProcessIsRunning() {
			Process keypadProcess = Process.GetProcessesByName(_processName).SingleOrDefault();
			return keypadProcess != null;
		}

	}

}
