using KeyPad.KeyBindingsEditor.ViewModels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyPad.KeyBindingsEditor.Models;
using KeyPad.KeyBindingSelector.Models;

namespace KeyPad.DataManager {

	public class KeyBindingFileManager : IDataManager {

		private const string DIRECTORY_NAME = "Bindings";
		private string _fileLocation;

		public KeyBindingFileManager(string fileName) =>
			_fileLocation = $@"{Environment.CurrentDirectory}\{DIRECTORY_NAME}\{fileName}";

		public bool Save<T>(T keyBindings) where T : class {
			var bindings = keyBindings as IEnumerable<KeyBindingViewModel>;
			if (bindings == null)
				throw new ArgumentException("keyBindings parameter is not an IEnumerable");

			try {
				using (StreamWriter sw = new StreamWriter(_fileLocation, false)) {
					foreach (var binding in bindings) {
						string keyCode = (binding.KeyCode == -1) ? "NULL" : Convert.ToString(binding.KeyCode, 16);
						sw.WriteLine($"{(int)binding.GamepadCode}={keyCode}");
					}
				}

				return true;
			}
			catch (Exception) {
				return false;
			}

		}

		public KeyBindingFile[] GetFiles() {
			return System.IO.Directory.GetFiles($@"{Environment.CurrentDirectory}\{DIRECTORY_NAME}")
				.Select(x => new KeyBindingFile(x))
				.ToArray();
		}

		public object Read() {
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
