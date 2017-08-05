using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.DataManager.EventArguments {

	public class SaveCompleteEventArgs : EventArgs {

		private object _itemsSaved;

		public SaveCompleteEventArgs(object itemsSaved) => _itemsSaved = itemsSaved;

		public object Items => _itemsSaved;
		
	}

}
