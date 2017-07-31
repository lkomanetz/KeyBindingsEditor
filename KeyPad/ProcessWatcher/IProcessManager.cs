using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.ProcessWatcher {

	public interface IProcessManager {

		event EventHandler<EventArgs> ProcessStarted;
		event EventHandler<EventArgs> ProcessStopped;
		bool IsRunning { get; }

		bool Start();
		void Stop();

	}

}
