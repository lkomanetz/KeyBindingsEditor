using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KeyPad.ProcessWatcher.ViewModels {

	public class ProcessWatcherDesignViewModel {

		public string LabelContent => "Running...";
		public Brush StatusBrush => Brushes.Green;
		public string ButtonLabelContent => "Start Service";

	}

}
