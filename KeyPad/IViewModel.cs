using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad {

	public interface IViewModel : INotifyPropertyChanged {

		string Title { get; }
		bool IsDirty { get; }

	}

}
