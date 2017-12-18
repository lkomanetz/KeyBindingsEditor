using KeyPad.Calculators;
using KeyPad.SettingsEditor.Models;
using KeyPad.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.SettingsEditor.ViewModels {

	[Serializable]
	public class ApplicationSettingViewModel :
		INotifyPropertyChanged,
		IDataViewModel<ApplicationSetting> {

		[NonSerialized] private ICalculator<string, object> _hashCalculator;
		[NonSerialized] private string _initialHash;
		private ApplicationSetting _setting;

		public ApplicationSettingViewModel(ApplicationSetting setting, ICalculator<string, object> hashCalculator) {
			_hashCalculator = hashCalculator;
			_setting = setting;
			_initialHash = _hashCalculator.Calculate(this);
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public string Name => _setting.Name;

		public object Value {
			get => _setting.Value;
			set {
				_setting.Value = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsDirty)));
			}
		}

		public bool IsDirty => _initialHash != _hashCalculator.Calculate(this);
		public ApplicationSetting ToDataModel() => _setting;

	}

}