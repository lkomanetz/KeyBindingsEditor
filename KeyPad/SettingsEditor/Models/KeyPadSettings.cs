using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.SettingsEditor.Models {

	[DataContract]
	public class KeyPadSettings {

		[DataMember] public IList<ApplicationSetting> Settings { get; set; }

	}

}
