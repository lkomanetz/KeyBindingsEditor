using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad {

	public class ValidatorResult {

		public ValidatorResult() => this.Message = String.Empty;
		public ValidatorResult(bool isSuccess) : this() => this.IsSuccess = isSuccess;

		public ValidatorResult(bool isSuccess, string message) {
			this.IsSuccess = isSuccess;
			this.Message = message;
		}

		public bool IsSuccess { get; private set; }
		public string Message { get; private set; }

	}

}
