using KeyPad.KeyBindingsEditor.ViewModels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyPad.Models;
using KeyPad.DataManager.EventArguments;
using KeyPad.Serializer;

namespace KeyPad.DataManager {

	public class KeyBindingFileManager : IDataManager {

		private const string DIRECTORY_NAME = "Bindings";
		private readonly string _directoryLocation;
		private string _fileLocation;

		public KeyBindingFileManager() {
			_directoryLocation = $"{Environment.CurrentDirectory}/Bindings";
			if (!Directory.Exists(_directoryLocation))
				Directory.CreateDirectory(_directoryLocation);
		}

		public KeyBindingFileManager(string fileLocation) :
			base() {
			_fileLocation = fileLocation;
		}

		public string FileLocation {
			get => _fileLocation;
			set => _fileLocation = value;
		}

		public event EventHandler<SaveCompleteEventArgs> SaveComplete;

		public bool Save<T>(T file) where T : class {
			var keyBindingFile = file as KeyBindingFile;
			if (keyBindingFile == null)
				throw new ArgumentException("file is not of type KeyBindingFile");

			try {
				using (StreamWriter sw = new StreamWriter(keyBindingFile.FileLocation, false)) {
					foreach (var binding in keyBindingFile.Bindings) {
						string keyCode = (binding.KeyboardButton == -1) ? "NULL" : Convert.ToString(binding.KeyboardButton, 16);
						sw.WriteLine($"{(int)binding.GamepadButton}={keyCode}");
					}
				}

				SaveComplete?.Invoke(this, new SaveCompleteEventArgs(keyBindingFile));
				return true;
			}
			catch (Exception) {
				return false;
			}

		}

		public object Read() {
			return Directory.GetFiles(_directoryLocation)
				.Select(x => new KeyBindingFile(x, GetBindingsFrom(x)))
				.ToArray();
		}

		public bool Delete<T>(T file) where T : class {
			try {
				var keyBindingFile = file as KeyBindingFile;
				if (keyBindingFile == null)
					throw new ArgumentException("file is not of type KeyBindingFile");

				File.Delete(keyBindingFile.FileLocation);
				return true;
			}
			catch (Exception) {
				return false;
			}
		}

		private KeyBinding[] GetBindingsFrom(string fileLocation) {
			string[] fileContents = File.ReadAllLines(fileLocation);
			KeyBinding[] bindings = new KeyBinding[fileContents.Length];

			for (short i = 0; i < fileContents.Length; ++i) {
				string[] items = fileContents[i].Split('=');

				int keyCode = (items[1] != "NULL") ? Convert.ToInt32(items[1], 16) : -1;
				GamepadButton btn = (GamepadButton)Int32.Parse(items[0]);
				bindings[i] = new KeyBinding(btn, keyCode);
			}

			return bindings;
		}

	}

}
