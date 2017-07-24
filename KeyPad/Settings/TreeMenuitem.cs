using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KeyPad.Settings {

	public class TreeMenuItem : IMenuItem, INotifyPropertyChanged {
		private bool _isSelected;

		public string Title { get; set; }
		public ICommand Action { get; set; }
		public IList<TreeMenuItem> Children { get; set; }

		public bool IsSelected {
			get => _isSelected;
			set {
				if (_isSelected != value) {
					_isSelected = value;
					PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };
	}

}
