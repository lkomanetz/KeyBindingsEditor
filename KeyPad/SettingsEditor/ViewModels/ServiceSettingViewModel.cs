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

	internal class ServiceSettingViewModel : IObservableViewModel {

		private UIElement _uiElement;
		private ServiceSetting _setting;
		private string _initialValue;

		public ServiceSettingViewModel(ServiceSetting setting) {
			_setting = setting;
			_initialValue = _setting.Value;

			CreateUiElement(setting.Name);
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

		public bool IsDirty => this.Value != _initialValue;

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
