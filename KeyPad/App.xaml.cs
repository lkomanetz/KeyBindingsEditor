using KeyPad.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KeyPad {

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {

		public App() {
			Application.Current.DispatcherUnhandledException += (sender, args) => {
				MessageBox.Show(
					$"{args.Exception.Message}\n{args.Exception.Source}",
					"Error",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
			};

			Application.Current.Startup += (sender, args) => {
				ShutdownMode = ShutdownMode.OnMainWindowClose;
				StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
			};

		}

	}
	
}
