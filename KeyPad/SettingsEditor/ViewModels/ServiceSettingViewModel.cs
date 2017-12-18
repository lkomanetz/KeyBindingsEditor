using KeyPad.Calculators;
using KeyPad.SettingsEditor.Models;
using KeyPad.SettingsEditor.UserControls;
using KeyPad.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace KeyPad.SettingsEditor.ViewModels {

	[Serializable]
	internal class ServiceSettingViewModel :
		IObservableViewModel,
		IDataViewModel<ServiceSetting> {

		[NonSerialized] private UIElement _uiElement;
		[NonSerialized] private ICalculator<string, object> _hashCalculator;
		[NonSerialized] private string _initialHash;
		private ServiceSetting _setting;

		public ServiceSettingViewModel(ServiceSetting setting, ICalculator<string, object> hashCalculator) {
			_setting = setting;
			_hashCalculator = hashCalculator;
			CreateUiElement(setting.Name);
			_initialHash = _hashCalculator.Calculate(this);
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public string Name => _setting.Name;

		public string Value {
			get => _setting.Value;
			set {
				if (_setting.Value != value) {
					_setting = new ServiceSetting(this.Name, value);
					PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsDirty)));
				}
			}
		}

		public UIElement Element {
			get => _uiElement;
			set {
				_uiElement = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(Element)));
			}
		}

		//TODO(Logan) -> There is a bug with dirty checking with Service Settings throwing an exception.  It's because of the UI Element.
		public bool IsDirty => _initialHash != _hashCalculator.Calculate(this);
		public ServiceSetting ToDataModel() => _setting;

		private void CreateUiElement(string settingName) {
			UIElement elem = null;
			Binding binding = null;
			DependencyProperty prop = null;

			switch (settingName) {
				case ServiceSettingNames.KEYBINDINGS_LOCATION_SETTING:
					elem = new FilePicker();
					((FilePicker)elem).FileType = FileType.Text;
					binding = new Binding(nameof(Value)) {
						Source = this,
						UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
						Mode = BindingMode.TwoWay
					};
					prop = FilePicker.LocationProperty;
					break;
				case ServiceSettingNames.KEYBOARD_PORT_SETTING:
				case ServiceSettingNames.JOYSTICK_PORT_SETTING:
					elem = new TextBox();
					binding = new Binding(nameof(Value)) {
						Source = this,
						UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
						Mode = BindingMode.TwoWay
					};
					prop = TextBox.TextProperty;
					break;
			}

			BindingOperations.SetBinding(elem, prop, binding);
			this.Element = elem;

		}

	}

}
