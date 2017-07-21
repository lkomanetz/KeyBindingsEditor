using KeyPad.KeyBindingsEditor.ViewModels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyPad.KeyBindingsEditor.Models;

namespace KeyPad.KeyBindingsEditor {

	public class BindingFileManager {

		private string _fileLocation;

		public BindingFileManager(string fileLocation) => _fileLocation = fileLocation;

		public void Save(IEnumerable<KeyBindingViewModel> keyBindings) {
			using (StreamWriter sw = new StreamWriter(_fileLocation, false)) {
				foreach (var binding in keyBindings) {
					string keyCode = (binding.KeyCode == -1) ? "NULL" : Convert.ToString(binding.KeyCode, 16);
					sw.WriteLine($"{(int)binding.GamepadCode}={keyCode}");
				}
			}
		}

		public KeyBindingViewModel[] Read() {
			string[] fileContents = File.ReadAllLines(_fileLocation);
			KeyBindingViewModel[] bindings = new KeyBindingViewModel[fileContents.Length];

			for (int i = 0;i < fileContents.Length; ++i) {
				string[] items = fileContents[i].Split('=');

				int keyCode = -1;
				if (items[1] != "NULL") {
					keyCode = Convert.ToInt32(items[1], 16);
				}

				GamepadButton btn = (GamepadButton)Int32.Parse(items[0]);
				bindings[i] = new KeyBindingViewModel(new KeyBinding(btn, keyCode));
			}

			return bindings;
		}

	}

}
