using KeyPad.KeyBindingsEditor.Models;
using KeyPad.KeyBindingsEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.KeyBindingsEditor {

	public class KeyBindingValidator : IValidator {
		private IEnumerable<KeyBindingViewModel> _bindings;

		public KeyBindingValidator(IEnumerable<KeyBindingViewModel> bindings) => _bindings = bindings;

		public IList<ValidatorResult> Validate() {
			var duplicates = _bindings.GroupBy(x => new { KeyboardButton = x.KeyboardButton, KeyCode = x.KeyCode })
				.Where(x => x.Key.KeyCode != -1 && x.Count() > 1)
				.Select(x => new { Element = x.Key.KeyboardButton, Count = x.Count() })
				.ToList();

			if (duplicates.Count() > 0) {
				string msg = "The following keyboard buttons are mapped multiple times:\n";
				foreach (var duplicate in duplicates)
					msg += $"{duplicate.Element} -> {duplicate.Count} times\n";

				return new List<ValidatorResult>() { new ValidatorResult(false, msg) };
			}

			return new List<ValidatorResult>() { new ValidatorResult(true) };
		}

	}

}