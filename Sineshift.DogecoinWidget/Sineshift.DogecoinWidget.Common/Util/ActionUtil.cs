using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace Sineshift.DogecoinWidget.Common
{
    public static class ActionUtil
	{
        public static void ExecuteDelayed(Action action, TimeSpan delay)
        {
            var timer = new DispatcherTimer();
            timer.Tick += OnExecute;
            timer.Interval = delay;
            timer.Tag = action;
            timer.Start();
        }

        private static void OnExecute(object sender, EventArgs e)
        {
            DispatcherTimer timer = sender as DispatcherTimer;
			timer.Stop();
			timer.Tick -= OnExecute;
            var action = timer.Tag as Action;
            action();
        }
    }
}
