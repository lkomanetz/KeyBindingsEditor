using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KeyPad.Models {

	public class KeyBindingFile {

		private const string EXTENSION = ".txt";
		private string _fileLocation;
		private string _fileName;
		private IList<KeyBinding> _bindings;

		public KeyBindingFile() {
			_fileLocation = null;
			_fileName = null;
		}

		public KeyBindingFile(string fileLocation, IList<KeyBinding> bindings) {
			_fileLocation = fileLocation;
			_fileName = GetFileName(fileLocation);
			_bindings = bindings;
		}

		public string FileLocation => _fileLocation;
		public string FileName => _fileName;
		public IList<KeyBinding> Bindings => _bindings;

		public override string ToString() => _fileName;

		private string GetFileName(string fileLocation) {
			Match match = Regex.Match(fileLocation, @"[^\\]\w+(?=\.[a-zA-Z]{3}\z)");
			if (match.Success)
				return match.Value;
			else
				return String.Empty;
		}

	}

}
