using KeyPad.DataManager.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.DataManager {

	public interface IDataManager {

		bool Save<T>(IList<T> items) where T : class;
		object Read();

		event EventHandler<SaveCompleteEventArgs> SaveComplete;

	}

}
