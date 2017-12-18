using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.ViewModels {

	public interface IObservableViewModel :
		IViewModel,
		INotifyPropertyChanged { }

}
