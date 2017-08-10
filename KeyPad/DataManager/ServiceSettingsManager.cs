using KeyPad.DataManager.EventArguments;
using KeyPad.SettingsEditor.Models;
using KeyPad.SettingsEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.DataManager {

	public class ServiceSettingsManager : IDataManager {

		private string _fileLocation;

		public ServiceSettingsManager(string fileLocation) => _fileLocation = fileLocation;

		public event EventHandler<SaveCompleteEventArgs> SaveComplete;

		public object Read() {
			string[] fileContents = System.IO.File.ReadAllLines(_fileLocation);
			IList<ServiceSetting> settings = new List<ServiceSetting>();

			foreach (string line in fileContents) {
				string[] items = line.Split('=');
				string value = (items[1].Equals("NULL")) ? String.Empty : items[1];
				settings.Add(new ServiceSetting(items[0], value));
			}

			return settings;
		}

		public bool Save<T>(T items) where T : class {
			var serviceSettings = items as IList<ServiceSetting>;
			if (serviceSettings == null)
				throw new ArgumentException("items is not of type IList<KeyPadSettingViewModel>");

			try {
				string newContent = String.Empty;
				for (int i = 0; i < serviceSettings.Count; ++i) {
					string newVal = (String.IsNullOrEmpty(serviceSettings[i].Value)) ? "NULL" : serviceSettings[i].Value;
					newContent += $"{serviceSettings[i].Name}={newVal}";

					if (i < serviceSettings.Count)
						newContent += Environment.NewLine;
				}

				using (StreamWriter sw = new StreamWriter(_fileLocation)) {
					sw.Write(newContent);
				}

				SaveComplete?.Invoke(this, new SaveCompleteEventArgs(serviceSettings));
				return true;
			}
			catch {
				return false;
			}
		}

		public bool Delete<T>(T item) where T : class => throw new NotImplementedException();

	}

}
