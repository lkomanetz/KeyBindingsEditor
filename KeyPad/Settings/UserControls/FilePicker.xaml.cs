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

		private static FilePickerViewModel _viewModel;

		public FilePicker() {
			InitializeComponent();
			_viewModel = (FilePickerViewModel)rootGrid.DataContext;
			_viewModel.LocationChanged += (sender, args) => this.Location = _viewModel.Location;
		}

		public static readonly DependencyProperty LocationProperty =
			DependencyProperty.Register("Location", typeof(string), typeof(FilePicker), new UIPropertyMetadata(null, OnLocationChanged));

		public static readonly DependencyProperty FileTypeProperty =
			DependencyProperty.Register(nameof(FileType), typeof(FileType), typeof(FilePicker), new UIPropertyMetadata(default(FileType), OnFileTypeChanged));

		public string Location {
			get => GetValue(LocationProperty).ToString();
			set => SetValue(LocationProperty, value);
		}

		public FileType FileType {
			get => (FileType)GetValue(FileTypeProperty);
			set => SetValue(FileTypeProperty, value);
		}

		private static void OnLocationChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
			_viewModel.Location = e.NewValue as String; 

		private static void OnFileTypeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
			_viewModel.FileType = (FileType)e.NewValue;

	}

}