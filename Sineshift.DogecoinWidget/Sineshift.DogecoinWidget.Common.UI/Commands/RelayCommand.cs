using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Sineshift.DogecoinWidget.Common.UI
{
	public class RelayCommand : ICommand
	{
		private readonly Predicate<object> canExecuteMethod;
		private readonly Action<object> executeMethod;
		private readonly Func<bool> canExecuteMethodNoArgs;
		private readonly Action executeMethodNoArgs;
 
		public RelayCommand(Action<object> executeMethod) : this(executeMethod, null) { }

		public RelayCommand(Action<object> executeMethod, Predicate<object> canExecuteMethod)
		{
			// validate argument
			if (executeMethod == null)
				throw new ArgumentNullException("executeMethod");

			// set values
			this.executeMethod = executeMethod;
			this.canExecuteMethod = canExecuteMethod;
		}

		public RelayCommand(Action executeMethod)
		{
			// validate argument
			if (executeMethod == null)
				throw new ArgumentNullException("executeMethod");

			// set values
			this.executeMethodNoArgs = executeMethod;
		}

		public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
		{
			// validate argument
			if (executeMethod == null)
				throw new ArgumentNullException("executeMethod");

			// set values
			this.executeMethodNoArgs = executeMethod;
			this.canExecuteMethodNoArgs = canExecuteMethod;
		}

		#region ICommand Members
		///<summary>
		///Occurs when changes occur that affect whether or not the command can execute.
		///</summary>
		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (canExecuteMethod != null || canExecuteMethodNoArgs != null)
					CommandManager.RequerySuggested += value;
			}
			remove
			{
				if (canExecuteMethod != null || canExecuteMethodNoArgs != null)
					CommandManager.RequerySuggested -= value;
			}
		}
		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public void Execute(object parameter)
		{
			if(executeMethod != null)
				executeMethod(parameter);
			if (executeMethodNoArgs != null)
				executeMethodNoArgs();
		}

		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <returns>
		/// true if this command can be executed; otherwise, false.
		/// </returns>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public bool CanExecute(object parameter)
		{
			if (canExecuteMethod != null)
				return canExecuteMethod(parameter);
			if (canExecuteMethodNoArgs != null)
				return canExecuteMethodNoArgs();
			return true;
		}
		#endregion

		public void RaiseCanExecuteChanged(EventArgs e)
		{
			CommandManager.InvalidateRequerySuggested();
		}
	}

	public class RelayCommand<T> : ICommand
	{
		private readonly Predicate<T> canExecuteMethod;
		private readonly Action<T> executeMethod;
		public RelayCommand(Action<T> executeMethod) : this(executeMethod, null) { }

		public RelayCommand(Action<T> executeMethod, Predicate<T> canExecuteMethod)
		{
			// validate argument
			if (executeMethod == null)
				throw new ArgumentNullException("executeMethod");

			// set values
			this.executeMethod = executeMethod;
			this.canExecuteMethod = canExecuteMethod;
		}

		#region ICommand Members
		///<summary>
		///Occurs when changes occur that affect whether or not the command can execute.
		///</summary>
		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (canExecuteMethod != null)
					CommandManager.RequerySuggested += value;
			}
			remove
			{
				if (canExecuteMethod != null)
					CommandManager.RequerySuggested -= value;
			}
		}

		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public void Execute(object parameter)
		{
			executeMethod((T)parameter);
		}
		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <returns>
		/// true if this command can be executed; otherwise, false.
		/// </returns>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public bool CanExecute(object parameter)
		{
			return canExecuteMethod == null ? true : canExecuteMethod((T)parameter);
		}
		#endregion

		public void RaiseCanExecuteChanged(EventArgs e)
		{
			CommandManager.InvalidateRequerySuggested();
		}
	}
}
