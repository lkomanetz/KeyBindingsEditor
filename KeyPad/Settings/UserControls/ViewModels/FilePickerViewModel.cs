using KeyPad.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KeyPad.Settings.UserControls.ViewModels {

	public class FilePickerViewModel : IViewModel, IObservableViewModel {
		private string _fileLocation;

		public FilePickerViewModel() {
			this.OpenCommand = new DelegateCommand<object>((param) => GetExeLocation());
		}

		public string Title => String.Empty;
		public bool IsDirty => false;
		public ICommand OpenCommand { get; private set; }

		public string Location {
			get => _fileLocation;
			set {
				_fileLocation = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(Location)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler<EventArgs> LocationChanged;

		private void GetExeLocation() {
			OpenFileDialog dlg = new OpenFileDialog() {
				Filter = "Exeutable Files (*.exe) | *.exe"
			};

			bool? result = dlg.ShowDialog();
			if (result == true) {
				this.Location = dlg.FileName;
				LocationChanged(this, EventArgs.Empty);
			}

		}

	}

}
