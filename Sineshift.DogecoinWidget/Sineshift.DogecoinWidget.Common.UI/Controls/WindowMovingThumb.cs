using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Sineshift.DogecoinWidget.Common.UI
{
	public class WindowMovingThumb : Thumb
	{
		public WindowMovingThumb()
		{
			this.DragDelta += OnDragDelta;
		}

		#region DP Window
		public Window Window
		{
			get { return (Window)GetValue(WindowProperty); }
			set { SetValue(WindowProperty, value); }
		}

		public static readonly DependencyProperty WindowProperty =
			DependencyProperty.Register("Window", typeof(Window), typeof(WindowMovingThumb), new PropertyMetadata(default(Window), (sender, e) => (sender as WindowMovingThumb).OnWindowChanged((Window)e.OldValue, (Window)e.NewValue)));

		protected void OnWindowChanged(Window oldValue, Window newValue)
		{

		}
		#endregion

		private void OnDragDelta(object sender, DragDeltaEventArgs e)
		{
			if (Window != null)
			{
				Window.Top += e.VerticalChange;
				Window.Left += e.HorizontalChange;
			}
		}
	}
}
