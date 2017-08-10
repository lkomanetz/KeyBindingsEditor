using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KeyPad.Models {

	public class KeyBindingFile {

		private const string EXTENSION = ".txt";
		private string _fileName;
		private IList<KeyBinding> _bindings;

		public KeyBindingFile() {
			_fileName = null;
		}

		public KeyBindingFile(string fileName, IList<KeyBinding> bindings) {
			_fileName = fileName;
			_bindings = bindings;
		}

		public string FileLocation => $@"{Environment.CurrentDirectory}\Bindings\{FileName}{EXTENSION}";
		public string FileName => _fileName;
		public IList<KeyBinding> Bindings => _bindings;

		public override string ToString() => _fileName;

	}

}
