using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.SettingsEditor.Models {

	[Serializable]
	[DataContract]
	public class ApplicationSetting {

		[DataMember] public string Name { get; set; }
		[DataMember] public string Display { get; set; }
		[DataMember] public object Value { get; set; }

	}

}