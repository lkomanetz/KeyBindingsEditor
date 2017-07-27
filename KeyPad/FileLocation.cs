using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad {

	public class FileLocation {

		private const string EXE_IDENTIFIER = "MZ";
		private string _fileLocation;

		public FileLocation(string fileLocation) => _fileLocation = fileLocation;

		public bool FileExists => System.IO.File.Exists(_fileLocation);
		public bool IsExecutable => IsExeFile();
		public override string ToString() => _fileLocation;

		public static explicit operator FileLocation(string value) => new FileLocation(value);

		private bool IsExeFile() {
			byte[] firstBytes = new byte[2];
			using (System.IO.FileStream fs = System.IO.File.Open(_fileLocation, System.IO.FileMode.Open)) {
				fs.Read(firstBytes, 0, firstBytes.Length);
			}

			return Encoding.UTF8.GetString(firstBytes) == EXE_IDENTIFIER;
		}

	}

}
