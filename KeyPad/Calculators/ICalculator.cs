using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.Calculators {

	public interface ICalculator<TResult, TInput> {

		TResult Calculate(TInput input);

	}

}
