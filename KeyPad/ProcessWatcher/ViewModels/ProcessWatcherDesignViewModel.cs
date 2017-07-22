using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KeyPad.ProcessWatcher.ViewModels {

	public class ProcessWatcherDesignViewModel {

		public string LabelContent => "Running...";
		public string StatusColor => Colors.Green.ToString();
		public string ButtonLabelContent => "Start Service";

	}

}
