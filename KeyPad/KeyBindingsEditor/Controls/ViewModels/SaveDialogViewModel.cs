using KeyPad.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Text.RegularExpressions;

namespace KeyPad.KeyBindingsEditor.Controls.ViewModels {

	internal class SaveDialogViewModel : IObservableViewModel {

		private SaveDialog _dlg;
		private Window _owner;

		public SaveDialogViewModel(SaveDialog dialog, Window owner) {
			_dlg = dialog;
			_owner = owner;
			ApplyBlur();
			FadeIn();

			this.SaveCommand = new DelegateCommand<object>((param) => {
				Save();
				RemoveBlur();
			});
			this.CancelCommand = new DelegateCommand<object>((param) => {
				_dlg.DialogResult = false;
				RemoveBlur();
			});
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public ICommand SaveCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }
		public bool SaveEnabled => (this.FileName != null) && (this.FileName.Length > 0);

		private string _fileName;
		public string FileName {
			get => _fileName;
			set {
				if (_fileName == value)
					return;
				_fileName = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(FileName)));
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(SaveEnabled)));
			}
		}

		private void Save() {
			if (String.IsNullOrEmpty(this.FileName)) {
				_dlg.DialogResult = false;
				return;
			}

			string modifiedFileName = Regex.Replace(this.FileName, @"\.\w+$", String.Empty);
			_dlg.DialogResult = true;
			_dlg.FileName = modifiedFileName;
			_dlg.Close();
		}

		private void FadeIn() {
			DoubleAnimation fadeIn = new DoubleAnimation() {
				From = 0D,
				To = 1D,
				Duration = new Duration(new TimeSpan(0, 0, 0, 0, 200))
			};
			Storyboard.SetTargetProperty(fadeIn, new PropertyPath(UIElement.OpacityProperty));
			Storyboard sb = new Storyboard();
			sb.Children.Add(fadeIn);
			sb.Begin(_dlg);
		}

		private void ApplyBlur() => _owner.Effect = new BlurEffect() { Radius = 8 };
		
		private void RemoveBlur() => _owner.Effect = null;

	}

}
