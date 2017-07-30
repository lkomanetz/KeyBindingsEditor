using KeyPad.Settings.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.Settings.ViewModels {

	public class ApplicationSettingViewModel<T> : IViewModel, ISetting<T> {

		private ApplicationSetting _setting;
		private T _initialValue;

		public ApplicationSettingViewModel(ApplicationSetting setting) {
			_setting = setting;
			_initialValue = SafeCast(_setting.Value);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public string Title => String.Empty;
		public string Name => _setting.Name;
		public bool IsDirty => !EqualityComparer<T>.Default.Equals(SafeCast(_setting.Value), _initialValue);

		public T Value {
			get => SafeCast(_setting.Value);
			set {
				_setting.Value = value; //TODO(Logan) -> Get dirty checking working for KeyPad.FileLocation type
				PropertyChanged(this, new PropertyChangedEventArgs("IsDirty"));
			}
		}

		private T SafeCast(object value) {
			if (value is T)
				return (T)value;

			try {
				return (T)Convert.ChangeType(value, typeof(T));
			}
			catch (InvalidCastException) {
				return default(T);
			}
		}

	}

}
