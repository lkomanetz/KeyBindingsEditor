using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KeyPad { 

	public class TopBarMenuItem : IMenuItem {

		public string Title { get; set; }
		public ICommand Action { get; set; }
		public IList<IMenuItem> Children { get; set; }

	}

}
