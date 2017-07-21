using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace KeyPad.TriggerActions {

	public class ParameterizedCommand : TriggerAction<DependencyObject> {

		protected override void Invoke(object parameter)
		{
			if (base.AssociatedObject == null)
				return;

			ICommand cmd = this.ResolveCommand();
			if (cmd != null && cmd.CanExecute(parameter))
				cmd.Execute(parameter);
		}

		private ICommand ResolveCommand() {
			if (this.Command != null)
				return this.Command;

			if (base.AssociatedObject == null)
				return null;

			PropertyInfo[] propInfo = base.AssociatedObject
				.GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			var cmdProp = propInfo.Where(x => 
				typeof(ICommand).IsAssignableFrom(x.PropertyType) && x.Name.Equals(this.CommandName)
			)
			.SingleOrDefault();

			if (cmdProp == null)
				return null;

			return (ICommand)cmdProp.GetValue(base.AssociatedObject, null);
		}

		private string _cmdName;
		public string CommandName {
			get {
				base.ReadPreamble();
				return this._cmdName;
			}
			set {
				if (_cmdName != value) {
					base.WritePreamble();
					_cmdName = value;
					base.WritePostscript();
				}
			}
		}

		public ICommand Command {
			get => (ICommand)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.Register("Command", typeof(ICommand), typeof(ParameterizedCommand), new UIPropertyMetadata(null));

	}

}
