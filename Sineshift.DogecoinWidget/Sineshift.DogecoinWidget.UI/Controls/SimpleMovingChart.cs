using Sineshift.DogecoinWidget.Common;
using Sineshift.DogecoinWidget.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sineshift.DogecoinWidget.UI
{
	public class SimpleMovingChart : ItemsControl
	{
		Canvas canvasPart;

		public SimpleMovingChart()
		{
			this.Loaded += OnLoaded;
		}

		#region DP DollarBrush
		public Brush DollarBrush
		{
			get { return (Brush)GetValue(DollarBrushProperty); }
			set { SetValue(DollarBrushProperty, value); }
		}

		public static readonly DependencyProperty DollarBrushProperty =
			DependencyProperty.Register("DollarBrush", typeof(Brush), typeof(SimpleMovingChart), new PropertyMetadata(default(Brush), (sender, e) => (sender as SimpleMovingChart).OnDollarBrushChanged((Brush)e.OldValue, (Brush)e.NewValue)));

		protected void OnDollarBrushChanged(Brush oldValue, Brush newValue)
		{

		}
		#endregion

		#region DP BitcoinBrush
		public Brush BitcoinBrush
		{
			get { return (Brush)GetValue(BitcoinBrushProperty); }
			set { SetValue(BitcoinBrushProperty, value); }
		}

		public static readonly DependencyProperty BitcoinBrushProperty =
			DependencyProperty.Register("BitcoinBrush", typeof(Brush), typeof(SimpleMovingChart), new PropertyMetadata(default(Brush), (sender, e) => (sender as SimpleMovingChart).OnBitcoinBrushChanged((Brush)e.OldValue, (Brush)e.NewValue)));

		protected void OnBitcoinBrushChanged(Brush oldValue, Brush newValue)
		{

		}
		#endregion

		#region DP VolumeBrush
		public Brush VolumeBrush
		{
			get { return (Brush)GetValue(VolumeBrushProperty); }
			set { SetValue(VolumeBrushProperty, value); }
		}

		public static readonly DependencyProperty VolumeBrushProperty =
			DependencyProperty.Register("VolumeBrush", typeof(Brush), typeof(SimpleMovingChart), new PropertyMetadata(default(Brush), (sender, e) => (sender as SimpleMovingChart).OnVolumeBrushChanged((Brush)e.OldValue, (Brush)e.NewValue)));

		protected void OnVolumeBrushChanged(Brush oldValue, Brush newValue)
		{

		}
		#endregion

		#region DP MaxItemCount
		public int MaxItemCount
		{
			get { return (int)GetValue(MaxItemCountProperty); }
			set { SetValue(MaxItemCountProperty, value); }
		}

		public static readonly DependencyProperty MaxItemCountProperty =
			DependencyProperty.Register("MaxItemCount", typeof(int), typeof(SimpleMovingChart), new PropertyMetadata(default(int), (sender, e) => (sender as SimpleMovingChart).OnMaxItemCountChanged((int)e.OldValue, (int)e.NewValue)));

		protected void OnMaxItemCountChanged(int oldValue, int newValue)
		{
			RedrawChart();
		}
		#endregion

		#region DP LineStyle
		public Style LineStyle
		{
			get { return (Style)GetValue(LineStyleProperty); }
			set { SetValue(LineStyleProperty, value); }
		}

		public static readonly DependencyProperty LineStyleProperty =
			DependencyProperty.Register("LineStyle", typeof(Style), typeof(SimpleMovingChart), new PropertyMetadata(default(Style), (sender, e) => (sender as SimpleMovingChart).OnLineStyleChanged((Style)e.OldValue, (Style)e.NewValue)));

		protected void OnLineStyleChanged(Style oldValue, Style newValue)
		{

		}
		#endregion

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			RedrawChart();
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			canvasPart = GetTemplateChild("PART_Canvas") as Canvas;
			RedrawChart();
		}

		protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			base.OnItemsSourceChanged(oldValue, newValue);
			RedrawChart();
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			RedrawChart();
		}

		private void RedrawChart()
		{
			if (canvasPart == null || ItemsSource == null || MaxItemCount == 0)
			{
				return;
			}

			var pairs = Items.OfType<BitcoinDollarPair>().ToList();

			if (pairs.None())
			{
				return;
			}

			var btcPrices = pairs.Select(p => p.PriceBTC).ToList();
			var usdPrices = pairs.Select(p => p.PriceUSD).ToList();
			var volumes = pairs.Select(p => p.Volume).ToList();

			var btcLine = GetLineFromPrices(btcPrices);
			btcLine.Stroke = BitcoinBrush;

			var usdLine = GetLineFromPrices(usdPrices);
			usdLine.Stroke = DollarBrush;

			var volumeLines = GetVolumeLines(volumes);
			volumeLines.ForEach(l =>
			{
				l.Fill = VolumeBrush;
				l.Stroke = Brushes.Transparent;
				l.StrokeThickness = 0;
			});

			canvasPart.Children.Clear();
			volumeLines.ForEach(l => canvasPart.Children.Add(l));
			canvasPart.Children.Add(btcLine);
			canvasPart.Children.Add(usdLine);	
		}

		private Polyline GetLineFromPrices(List<double> prices)
		{
			var min = prices.Min();
			var max = prices.Max();
			var span = min.AlmostEquals(max) ? max : max - min;
			var widthPerPrice = canvasPart.ActualWidth / MaxItemCount;
			var startX = canvasPart.ActualWidth - prices.Count * widthPerPrice;

			var polyline = new Polyline()
			{
				Style = LineStyle
			};

			for (int i = 0; i < prices.Count; ++i)
			{
				var price = prices[i];
				var x = startX + i * widthPerPrice;
				var relativeY = (price - min) / span;
				var y = canvasPart.ActualHeight * (1.0 - relativeY);

				polyline.Points.Add(new Point(x, y));
			}

			return polyline;
		}

		private List<Shape> GetVolumeLines(List<double> volumes)
		{
			var lines = new List<Shape>();

			var min = volumes.Min();
			var max = volumes.Max();
			var span = min.AlmostEquals(max) ? max : max - min;
			var widthPerPrice = canvasPart.ActualWidth / MaxItemCount;
			var halfWidth = widthPerPrice / 2.0;
			var startX = canvasPart.ActualWidth - volumes.Count * widthPerPrice;

			for (int i = 0; i < volumes.Count; ++i)
			{
				var volume = volumes[i];
				var x = startX + i * widthPerPrice;
				var relativeY = (volume - min) / span;
				var y = canvasPart.ActualHeight * (1.0 - relativeY);
				y = y > canvasPart.ActualHeight - 1 ? canvasPart.ActualHeight - 1 : y;
				var height = canvasPart.ActualHeight - y;

				var line = new Rectangle()
				{
					Style = LineStyle,
					Height = height > 1.0 ? height : 1.0,
					Width = widthPerPrice
				};
				Canvas.SetTop(line, y);
				Canvas.SetLeft(line, x - halfWidth);
				lines.Add(line);
			}

			return lines;
		}
	}
}
