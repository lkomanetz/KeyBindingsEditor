using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KeyPad.KeyBindingSelector.Models {

	public class KeyBindingFile {

		private const string EXTENSION = ".txt";
		private string _fileLocation;
		private string _fileName;

		public KeyBindingFile() {
			_fileLocation = null;
			_fileName = null;
		}

		public KeyBindingFile(string fileLocation) {
			_fileLocation = fileLocation;
			_fileName = GetFileName(fileLocation);
		}

		public string FileLocation => _fileLocation;
		public string FileName => _fileName; 
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
