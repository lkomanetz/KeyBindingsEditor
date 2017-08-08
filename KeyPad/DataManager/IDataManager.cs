using KeyPad.DataManager.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.DataManager {

	public interface IDataManager {

		bool Save<T>(T items) where T : class;
		object Read();
		bool Delete<T>(T item) where T : class;

		event EventHandler<SaveCompleteEventArgs> SaveComplete;

	}

}
