using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.Calculators {

	public class Md5Calculator : ICalculator<string, object> {

		public string Calculate(object input) {
			string hash = String.Empty;
			using (var stream = new MemoryStream()) {
				var formatter = new BinaryFormatter();
				formatter.Serialize(stream, input);
				hash = GetMd5Checksum(stream.ToArray());
			}

			return hash;
		}

		private string GetMd5Checksum(byte[] buffer) {
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] hash = md5.ComputeHash(buffer);
			string result = String.Empty;

			foreach (byte singleByte in hash) result += singleByte.ToString("X2");
			return result;
		}

	}

}
