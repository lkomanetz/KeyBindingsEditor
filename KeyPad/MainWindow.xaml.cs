using KeyPad.ViewModels;
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

namespace KeyPad {

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private MainWindowViewModel _mainWindowVm;

		public MainWindow() {
			_mainWindowVm = new MainWindowViewModel(this);
			InitializeComponent();
			this.DataContext = _mainWindowVm;
			App.Current.Exit += (sender, args) => _mainWindowVm.Dispose();
		}

	}

}
