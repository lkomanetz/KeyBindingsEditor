using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad {

	public static class ValidatorMessageBuilder {

		public static string Build(IList<ValidatorResult> results) {
			String result = String.Empty;
			if (results.All(x => x.IsSuccess))
				return result;

			var failedResults = results.Where(x => !x.IsSuccess).ToList();
			foreach (var failedResult in failedResults) {
				result += $"{failedResult.Message}\n";
			}

			return result;
		}

	}

}
