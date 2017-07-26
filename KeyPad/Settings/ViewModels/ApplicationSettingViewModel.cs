using KeyPad.Settings.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.Settings.ViewModels {

	public class ApplicationSettingViewModel : INotifyPropertyChanged {

		private ApplicationSetting _setting;
		private object _initialValue;

		public ApplicationSettingViewModel(ApplicationSetting setting) {
			_setting = setting;
			_initialValue = _setting.Value;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public string Name => _setting.Name;
		public bool IsDirty => _setting.Value != _initialValue;
		public object Value {
			get => _setting.Value;
			set {
				_setting = new ApplicationSetting(this.Name, value);
				PropertyChanged(this, new PropertyChangedEventArgs("IsDirty"));
			}
		}

	}

}
