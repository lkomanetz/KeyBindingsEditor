using KeyPad.Settings.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.Settings {

	public class AppSettingsValidator : IValidator
	{
		private const string EXE_IDENTIFIER = "MZ";
		private IList<ApplicationSettingViewModel> _settings;

		public AppSettingsValidator(IList<ApplicationSettingViewModel> settings) => _settings = settings;

		public ValidatorResult Validate() {
			var startupSetting = _settings
				.Where(x => x.Name.Equals("start_service_on_startup"))
				.Single();
			var locationSetting = _settings
				.Where(x => x.Name.Equals("service_location"))
				.Single();

			if (!Boolean.TryParse(startupSetting.Value.ToString(), out bool result))
				return new ValidatorResult(false, $"'{startupSetting.Name}' is not 'true|false'");

			if (!File.Exists(locationSetting.Value.ToString()))
				return new ValidatorResult(false, $"File '{locationSetting.Value}' does not exist.");

			if (!IsExecutable(locationSetting.Value.ToString()))
				return new ValidatorResult(false, $"File '{locationSetting.Value}' is not an executable.");

			return new ValidatorResult(true);
		}

		private bool IsExecutable(string filePath) {
			byte[] firstBytes = new byte[2];
			using (FileStream fs = File.Open(filePath, FileMode.Open))
				fs.Read(firstBytes, 0, firstBytes.Length);

			return Encoding.UTF8.GetString(firstBytes) == EXE_IDENTIFIER;
		}

	}

}
