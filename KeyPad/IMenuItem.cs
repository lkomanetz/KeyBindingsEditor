using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KeyPad {

	public interface IMenuItem {

		string Title { get; }
		ICommand Action { get; }

	}

}
