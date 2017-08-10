using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.SettingsEditor.Models {

	public class ServiceSetting {

		public ServiceSetting(string name, string value) {
			this.Value = value;
			this.Name = name;
		}

		public string Name { get; }

		public string Value { get; }

	}

}