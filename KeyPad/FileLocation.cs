using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad {

    [DataContract]
	public class FileLocation {

		private const string EXE_IDENTIFIER = "MZ";
		private string _fileLocation;

		public FileLocation(string fileLocation) => _fileLocation = fileLocation;

		[IgnoreDataMember] public bool FileExists => System.IO.File.Exists(_fileLocation);
		[IgnoreDataMember] public bool IsExecutable => IsExeFile();

        [DataMember]
		public string Location {
			get => _fileLocation;
			set => _fileLocation = value;
		}

		public override string ToString() => _fileLocation;
        public override int GetHashCode() => _fileLocation.GetHashCode();

        public override bool Equals(object obj) {
            var item = obj as FileLocation;
            return this == item;
        }

        public static bool operator !=(FileLocation objA, FileLocation objB) => !(objA == objB);

        public static bool operator ==(FileLocation objA, FileLocation objB) {
            if (Object.ReferenceEquals(objA, objB))
                return true;

            if (Object.ReferenceEquals(objA, null))
                return Object.ReferenceEquals(objB, null);

            return objA.Location.Equals(objB.Location);
        }

        private bool IsExeFile() {
			byte[] firstBytes = new byte[2];
			using (System.IO.FileStream fs = System.IO.File.Open(_fileLocation, System.IO.FileMode.Open)) {
				fs.Read(firstBytes, 0, firstBytes.Length);
			}

			return Encoding.UTF8.GetString(firstBytes) == EXE_IDENTIFIER;
		}

	}

}
