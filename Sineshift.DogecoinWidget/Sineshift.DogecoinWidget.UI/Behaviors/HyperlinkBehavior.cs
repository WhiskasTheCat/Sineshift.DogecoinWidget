using Sineshift.DogecoinWidget.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Sineshift.DogecoinWidget.UI
{
	public static class HyperlinkBehavior
	{
		public static bool GetEnableHyperlinkNavigation(DependencyObject obj)
		{
			return (bool)obj.GetValue(EnableHyperlinkNavigationProperty);
		}

		public static void SetEnableHyperlinkNavigation(DependencyObject obj, bool value)
		{
			obj.SetValue(EnableHyperlinkNavigationProperty, value);
		}

		public static readonly DependencyProperty EnableHyperlinkNavigationProperty =
			DependencyProperty.RegisterAttached("EnableHyperlinkNavigation", typeof(bool), typeof(HyperlinkBehavior), new PropertyMetadata(false, OnEnableHyperlinkNavigationChanged));

		private static void OnEnableHyperlinkNavigationChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			var hyperlink = obj as Hyperlink;
			if (hyperlink == null || !(bool)e.NewValue)
			{
				return;
			}

			hyperlink.RequestNavigate += (sender2, e2) =>
			{
				try
				{
					System.Diagnostics.Process.Start(e2.Uri.ToString());
				}
				catch(Exception ex)
				{
					ExceptionUtil.LogAndShowWarning("Could not navigate to address: ", ex);
				}
			};
		}
	}
}
