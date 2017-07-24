using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KeyPad.Settings.ViewModels {

	public class KeyPadSettingsViewModel : INotifyPropertyChanged {
		private object _currentViewModel;

		public KeyPadSettingsViewModel() {
			CreateSettingsMenu();
		}

		public IList<TreeMenuItem> SettingsMenu { get; private set; }
		public object CurrentViewModel {
			get => _currentViewModel;
			set {
				_currentViewModel = value;
				PropertyChanged(this, new PropertyChangedEventArgs("CurrentViewModel"));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		private void CreateSettingsMenu() {
			this.SettingsMenu = new List<TreeMenuItem>() {
				new TreeMenuItem() {
					Title = "Service Settings",
					Children = new List<TreeMenuItem>() {
						new TreeMenuItem() {
							Title = "Bindings File Location",
							Action = new DelegateCommand<object>((param) => { MessageBox.Show("I clicked a button!"); })
						}
					}
				},
				new TreeMenuItem() {
					Title = "KeyPad Settings",
					Action = new DelegateCommand<object>((param) => { })
				}
			};
		}

	}

}
