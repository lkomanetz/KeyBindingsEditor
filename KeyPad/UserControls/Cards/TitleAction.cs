using KeyPad.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KeyPad.UserControls.Cards {

	public class TitleAction : IObservableViewModel {

		private string _actionImage;
		public string ActionImage {
			get => _actionImage;
			set {
				if (_actionImage == value)
					return;
				_actionImage = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(ActionImage)));
			}
		}

		private ICommand _action;
		public ICommand Action {
			get => _action;
			set {
				_action = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(Action)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };
	}

}
