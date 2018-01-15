using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sineshift.DogecoinWidget.Common.UI
{
	public class Navigator : DependencyObject
	{
		private ContentControl navigationTarget;
		private readonly IServiceLocator serviceLocator;
		private readonly ILogger logger;

		public Navigator(IServiceLocator serviceLocator, ILogger logger)
		{
			this.serviceLocator = serviceLocator;
			this.logger = logger;
		}

		public event EventHandler<NavigatedEventArgs> Navigated;

		public static bool GetIsNavigationTarget(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsNavigationTargetProperty);
		}

		public static void SetIsNavigationTarget(DependencyObject obj, bool value)
		{
			obj.SetValue(IsNavigationTargetProperty, value);
		}

		public static readonly DependencyProperty IsNavigationTargetProperty =
			DependencyProperty.RegisterAttached("IsNavigationTarget", typeof(bool), typeof(Navigator), new PropertyMetadata(false, OnIsNavigationTargetChanged));


		private static void OnIsNavigationTargetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			Guard.IsOfType<ContentControl>(obj, "IsNavigationTarget can only be set on a ContentControl.");

			var navigator = ServiceLocator.Current.Get<Navigator>();

			if ((bool)e.NewValue)
			{
				navigator.NavigationTarget = obj as ContentControl;
			}
			else
			{
				navigator.NavigationTarget = null;
			}
	
		}

		internal ContentControl NavigationTarget
		{
			get => navigationTarget;
			set => navigationTarget = value;
		}

		public object CurrentView
		{
			get { return navigationTarget?.Content; }
		}

		public void Navigate<T>(object param, NavigationType type = NavigationType.Instant) where T : FrameworkElement
		{
			Guard.IsNotNull(NavigationTarget, "Cannot navigate because no ContentControl has been set as NavigationTarget.");
			logger.Debug($"Navigating to {typeof(T)}");

			var view = serviceLocator.Get<T>();
			var navigationObserver = view.DataContext as INavigationObserver;

			var previousView = NavigationTarget.Content as FrameworkElement;
			if (previousView != null)
			{
				var previousNavigationObserver = previousView.DataContext as INavigationObserver;
				if (previousNavigationObserver != null)
				{
					previousNavigationObserver.NavigatedFrom();
				}
			}

			var properTarget = NavigationTarget as INavigationTarget;
			if (properTarget != null)
			{
				properTarget.RequestNavigate(view, type);
			}
			else
			{
				NavigationTarget.Content = view;
			}

			if (navigationObserver != null)
			{
				navigationObserver.NavigatedTo(param);
			}

			Navigated?.Invoke(this, new NavigatedEventArgs(previousView, view));

			logger.Debug($"Navigated to {typeof(T)}");
		}

		public void Navigate<T>(NavigationType type = NavigationType.Instant) where T : FrameworkElement
		{
			this.Navigate<T>(null, type);
		}
	}
}
