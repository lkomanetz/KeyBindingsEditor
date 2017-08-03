﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.ViewModels {

	public class CardViewModel : IViewModel, IObservableViewModel {

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
		public IList<TitleAction> TitleActions
		{
			get => _actions;
			set {
				_actions = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(TitleActions)));
			}
		}

		private IList<IViewModel> _content;
		public IList<IViewModel> CardContent
		{
			get => _content;
			set {
				_content = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(CardContent)));
			}
		}

		public bool IsDirty => false;
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

	}

}