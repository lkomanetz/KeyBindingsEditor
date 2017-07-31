using KeyPad.Settings.Models;
using KeyPad.Settings.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
				.Where(x => x.Name.Equals("service_startup"))
				.Single();

			var locationSetting = _settings
				.Where(x => x.Name.Equals("service_location"))
				.Single();

			var nameSetting = _settings
				.Where(x => x.Name.Equals("process_name"))
				.Single();

			if (!Boolean.TryParse(startupSetting.Value.ToString(), out bool result))
				results.Add(new ValidatorResult(false, $"'{startupSetting.Name}' is not 'true|false'"));

			if (!File.Exists(locationSetting.Value.ToString()))
				results.Add(new ValidatorResult(false, $"File '{locationSetting.Value}' does not exist."));

			uint type = 0;
			bool isExeFile = GetBinaryType(locationSetting.Value.ToString(), out type);
			if (!String.IsNullOrEmpty(locationSetting.Value.ToString()) && !isExeFile) {
				results.Add(new ValidatorResult(false, $"File '{locationSetting.Value}' is not an executable."));
			}

			if (results.Count == 0)
				results.Add(new ValidatorResult(true));

			if (String.IsNullOrEmpty(nameSetting.Value.ToString()))
				results.Add(new ValidatorResult(false, "Process Name cannot be null or empty."));

			return results;
		}

		[DllImport("Kernel32.dll", CallingConvention=CallingConvention.Winapi)]
		private static extern bool GetBinaryType(string appName, out uint binaryType);

	}

}