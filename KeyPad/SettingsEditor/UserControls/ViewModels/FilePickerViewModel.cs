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

namespace KeyPad.SettingsEditor.UserControls.ViewModels {

	internal class FilePickerViewModel : IObservableViewModel {

		private string _fileLocation;
		private FileType _fileType;

		public FilePickerViewModel() {
			this.OpenCommand = new DelegateCommand<object>((param) => GetExeLocation());
		}

		public string Title => String.Empty;
		public ICommand OpenCommand { get; private set; }

		public string Location {
			get => _fileLocation;
			set {
				_fileLocation = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(Location)));
			}
		}

		public FileType FileType {
			get => _fileType;
			set {
				if (_fileType == value)
					return;
				_fileType = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(Location)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler<EventArgs> LocationChanged;

		private void GetExeLocation() {
			OpenFileDialog dlg = new OpenFileDialog();

			switch (FileType) {
				case FileType.Executable:
					dlg.Filter = "Executable Files (*.exe) | *.exe";
					break;
				case FileType.Text:
					dlg.Filter = "Text Files (*.txt) | *.txt";
					break;
			}

			bool? result = dlg.ShowDialog();
			if (result == true) {
				this.Location = dlg.FileName;
				LocationChanged(this, EventArgs.Empty);
			}

		}

	}

}
