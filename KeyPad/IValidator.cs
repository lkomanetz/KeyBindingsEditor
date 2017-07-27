using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad {

	public interface IValidator {

		IList<ValidatorResult> Validate();

	}

}
