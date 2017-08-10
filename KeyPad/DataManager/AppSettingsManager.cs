using KeyPad.SettingsEditor.Models;
using KeyPad.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyPad.DataManager.EventArguments;

namespace KeyPad.DataManager {

	public class AppSettingsManager : IDataManager {

		private const string FILE_NAME = "settings.json";
		private ISerializer _serializer;

		public AppSettingsManager(ISerializer serializer) => _serializer = serializer;

		public event EventHandler<SaveCompleteEventArgs> SaveComplete;

		public object Read() {
			string jsonString = System.IO.File.ReadAllText(FILE_NAME);
			return _serializer.Deserialize<ApplicationSetting[]>(jsonString);
		}

		public bool Save<T>(T items) where T : class {
			var appSettings = items as IList<ApplicationSetting>;
			if (appSettings == null)
				throw new ArgumentException("items parameter is not a list");

			try
			{
				string settingsJson = _serializer.Serialize(items);
				System.IO.File.WriteAllText(FILE_NAME, settingsJson);
			}
			catch {
				return false;
			}

			SaveComplete?.Invoke(this, new SaveCompleteEventArgs(appSettings));
			return true;
		}

		public bool Delete<T>(T item) where T : class => throw new NotImplementedException();

	}

}
