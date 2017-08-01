using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.DataManager {

	public interface IDataManager {

		bool Save<T>(T items) where T : class;
		object Read();

	}

}
