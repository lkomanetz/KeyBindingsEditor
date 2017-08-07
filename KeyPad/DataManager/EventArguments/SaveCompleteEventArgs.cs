using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.DataManager.EventArguments {

	public class SaveCompleteEventArgs : EventArgs {

		private object _itemSaved;

		public SaveCompleteEventArgs(object itemSaved) => _itemSaved = itemSaved;

		public object Item => _itemSaved;
		
	}

}
