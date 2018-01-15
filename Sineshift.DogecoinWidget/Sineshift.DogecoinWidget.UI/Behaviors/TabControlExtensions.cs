using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sineshift.DogecoinWidget.UI
{
	public static class TabControlExtensions
	{

		public static string GetTabControlCaption(DependencyObject obj)
		{
			return (string)obj.GetValue(TabControlCaptionProperty);
		}

		public static void SetTabControlCaption(DependencyObject obj, string value)
		{
			obj.SetValue(TabControlCaptionProperty, value);
		}

		public static readonly DependencyProperty TabControlCaptionProperty =
			DependencyProperty.RegisterAttached("TabControlCaption", typeof(string), typeof(TabControlExtensions), new PropertyMetadata(null));



		public static double GetTabControlCaptionWidth(DependencyObject obj)
		{
			return (double)obj.GetValue(TabControlCaptionWidthProperty);
		}

		public static void SetTabControlCaptionWidth(DependencyObject obj, double value)
		{
			obj.SetValue(TabControlCaptionWidthProperty, value);
		}

		public static readonly DependencyProperty TabControlCaptionWidthProperty =
			DependencyProperty.RegisterAttached("TabControlCaptionWidth", typeof(double), typeof(TabControlExtensions), new PropertyMetadata(0.0));



	}
}
