using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KeyPad {

	internal class DelegateCommand<T> : ICommand {

		private Action<T> _cmdAction;

		public DelegateCommand(Action<T> cmdAction) => _cmdAction = cmdAction;

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter) => true;

		public void Execute(object parameter) => _cmdAction?.Invoke((T)parameter);
	}

}