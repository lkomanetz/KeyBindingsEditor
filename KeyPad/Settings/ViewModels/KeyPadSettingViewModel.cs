using KeyPad.Settings.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.Settings.ViewModels {

	public class KeyPadSettingViewModel : INotifyPropertyChanged {
		private KeyPadSetting _setting;
		private string _initialValue;

		public KeyPadSettingViewModel(KeyPadSetting setting) {
			_setting = setting;
			_initialValue = _setting.Value;
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public string Name => _setting.Name;

		public string Value {
			get => _setting.Value;
			set {
				if (_setting.Value != value) {
					_setting = new KeyPadSetting(this.Name, value);
					PropertyChanged(this, new PropertyChangedEventArgs("IsDirty"));
				}
			}
		}

		public bool IsDirty => this.Value != _initialValue;

	}

}
