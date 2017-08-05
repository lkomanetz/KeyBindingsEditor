using KeyPad.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;

namespace KeyPad.KeyBindingsEditor.Controls.ViewModels {

	internal class SaveDialogViewModel : IObservableViewModel {

		private SaveDialog _dlg;
		public SaveDialogViewModel(SaveDialog dialog) {
			_dlg = dialog;
			this.SaveCommand = new DelegateCommand<object>((param) => Save());
			this.CancelCommand = new DelegateCommand<object>((param) => _dlg.DialogResult = false);
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public ICommand SaveCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }

		private string _fileName;
		public string FileName {
			get => _fileName;
			set {
				if (_fileName == value)
					return;
				_fileName = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(FileName)));
			}
		}

		private void Save() {
			if (String.IsNullOrEmpty(this.FileName)) {
				_dlg.DialogResult = false;
				return;
			}

			string modifiedFileName = (!this.FileName.Contains(".txt")) ?
				this.FileName += ".txt" :
				this.FileName;

			_dlg.DialogResult = true;
			_dlg.FileName = modifiedFileName;
			_dlg.Close();
		}

	}

}
