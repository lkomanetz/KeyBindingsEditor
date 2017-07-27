using KeyPad.Settings.Models;
using KeyPad.Settings.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.Settings {

	//TODO(Logan) -> Fix the validator after making changes to ApplicationSettingViewModel class
	public class AppSettingsValidator : IValidator {

		private const string EXE_IDENTIFIER = "MZ";
		private IList<ApplicationSetting> _settings;

		public AppSettingsValidator(IList<ApplicationSetting> settings) => _settings = settings;

		public IList<ValidatorResult> Validate() {
			IList<ValidatorResult> results = new List<ValidatorResult>();
			var startupSetting = _settings
				.Where(x => x.Name.Equals("start_service_on_startup"))
				.Single();
			var locationSetting = _settings
				.Where(x => x.Name.Equals("service_location"))
				.Single();

			if (!Boolean.TryParse(startupSetting.Value.ToString(), out bool result))
				results.Add(new ValidatorResult(false, $"'{startupSetting.Name}' is not 'true|false'"));

			if (!File.Exists(locationSetting.Value.ToString()))
				results.Add(new ValidatorResult(false, $"File '{locationSetting.Value}' does not exist."));

			if (!String.IsNullOrEmpty(locationSetting.Value.ToString()) &&
				!IsExecutable(locationSetting.Value.ToString())) {

				results.Add(new ValidatorResult(false, $"File '{locationSetting.Value}' is not an executable."));
			}

			if (results.Count == 0)
				results.Add(new ValidatorResult(true));

			return results;
		}

		private bool IsExecutable(string filePath) {
			byte[] firstBytes = new byte[2];
			using (FileStream fs = File.Open(filePath, FileMode.Open))
				fs.Read(firstBytes, 0, firstBytes.Length);

			return Encoding.UTF8.GetString(firstBytes) == EXE_IDENTIFIER;
		}

	}

}