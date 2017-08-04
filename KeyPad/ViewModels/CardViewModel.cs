using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KeyPad.ViewModels {

	public class CardViewModel :
		IViewModel,
		IObservableViewModel {

		public CardViewModel() {
			this.ToggleCollapseState = new DelegateCommand<object>((param) => this.IsCollapsed = !this.IsCollapsed);
		}

		public ICommand ToggleCollapseState { get; private set; }
		public Visibility ContentVisibility => (_isCollapsed) ? Visibility.Visible : Visibility.Collapsed;

		private string _title;
		public string Title {
			get => _title;
			set {
				if (_title == value)
					return;
				_title = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(Title)));
			}

		}

		private IList<TitleAction> _actions;
		public IList<TitleAction> TitleActions {
			get => _actions;
			set {
				_actions = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(TitleActions)));
			}
		}

		private IList<IViewModel> _content;
		public IList<IViewModel> CardContent {
			get => _content;
			set {
				_content = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(CardContent)));
			}
		}

		private bool _isCollapsed;
		public bool IsCollapsed {
			get => _isCollapsed;
			set {
				if (_isCollapsed == value)
					return;
				_isCollapsed = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(ContentVisibility)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

	}

}
