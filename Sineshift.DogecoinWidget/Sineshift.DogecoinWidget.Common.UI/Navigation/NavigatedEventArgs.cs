using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sineshift.DogecoinWidget.Common.UI
{
	public class NavigatedEventArgs : EventArgs
	{
		public NavigatedEventArgs(FrameworkElement previousView, FrameworkElement newView)
		{
			this.PreviousView = previousView;
			this.NewView = newView;
		}

		public FrameworkElement PreviousView
		{
			get;
			private set;
		}

		public FrameworkElement NewView
		{
			get;
			private set;
		}
	}
}
