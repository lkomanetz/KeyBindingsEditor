using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KeyPad {

	internal class DelegateCommand<T> : ICommand {

		private Action<T> _cmdAction;
		private Predicate<T> _canExecute;

		public DelegateCommand(Action<T> cmdAction) => _cmdAction = cmdAction;

		public DelegateCommand(Action<T> cmdAction, Predicate<T> canExecute) {
			_cmdAction = cmdAction;
			_canExecute = canExecute;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter) => (_canExecute == null) ? true : _canExecute((T)parameter);
		public void Execute(object parameter) => _cmdAction?.Invoke((T)parameter);
		public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
			

	}

}