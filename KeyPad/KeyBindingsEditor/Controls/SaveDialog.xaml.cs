using KeyPad.KeyBindingsEditor.Controls.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyPad.KeyBindingsEditor.Controls {

	/// <summary>
	/// Interaction logic for SaveDialog.xaml
	/// </summary>
	public partial class SaveDialog : Window {

		private SaveDialogViewModel _viewModel;

		public SaveDialog(Window owner) {
			_viewModel = new SaveDialogViewModel(this);
			this.DataContext = _viewModel;
			this.Owner = owner;
			InitializeComponent();
		}

		public string FileName { get; set; }

	}

}
