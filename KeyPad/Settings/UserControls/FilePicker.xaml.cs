using KeyPad.Settings.UserControls.ViewModels;
using Microsoft.Win32;
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

namespace KeyPad.Settings.UserControls {

	/// <summary>
	/// Interaction logic for FilePicker.xaml
	/// </summary>
	public partial class FilePicker : UserControl {
		private FilePickerViewModel _viewModel;

		public FilePicker() {
			InitializeComponent();
			_viewModel = new FilePickerViewModel();
			this.DataContext = _viewModel;
		}

		public static readonly DependencyProperty LocationProperty =
			DependencyProperty.Register("Location", typeof(FilePickerViewModel), typeof(FilePicker), new UIPropertyMetadata(null));

		public string Location {
			get => GetValue(LocationProperty).ToString();
			set => SetValue(LocationProperty, value);
		}

	}

}
