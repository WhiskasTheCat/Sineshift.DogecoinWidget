using Sineshift.DogecoinWidget.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sineshift.DogecoinWidget.UI
{
	public static class CopyOnClickBehavior
	{

		public static string GetCopySource(DependencyObject obj)
		{
			return (string)obj.GetValue(CopySourceProperty);
		}

		public static void SetCopySource(DependencyObject obj, string value)
		{
			obj.SetValue(CopySourceProperty, value);
		}


		public static readonly DependencyProperty CopySourceProperty =
			DependencyProperty.RegisterAttached("CopySource", typeof(string), typeof(CopyOnClickBehavior), new PropertyMetadata(null, OnCopySourceChanged));


		private static void OnCopySourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			if (obj is Button button)
			{
				button.Click += (sender2, e2) =>
				{
					try
					{
						Clipboard.SetText(e.NewValue?.ToString());
					}
					catch(Exception ex)
					{
						ExceptionUtil.LogAndShowWarning("Could not copy address", ex);
					}
				};
			}

		}
	}
}
