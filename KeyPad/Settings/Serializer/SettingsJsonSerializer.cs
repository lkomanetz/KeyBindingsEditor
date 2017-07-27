using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace KeyPad.Settings.Serializer {

	public class SettingsJsonSerializer : ISerializer {
		private readonly static Encoding _encoding = Encoding.UTF8;

		public T Deserialize<T>(string serializedData) {
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
			using (var ms = new MemoryStream(_encoding.GetBytes(serializedData))) {
				return (T)serializer.ReadObject(ms);
			}
		}

		public string Serialize<T>(T obj) {
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
			byte[] json = null;

			using (var ms = new MemoryStream()) { 
				serializer.WriteObject(ms, obj);
				json = ms.ToArray();
			}

			return _encoding.GetString(json);
		}

	}

}