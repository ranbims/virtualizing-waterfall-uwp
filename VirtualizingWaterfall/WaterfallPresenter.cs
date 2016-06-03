using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualizingWaterfall;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace VirtualizingWaterfall
{
    public class WaterfallPresenter : Panel
    {
        #region OriginalDataSource
        public IList<IFixedRenderSize> OriginalDataSource
        {
            get { return (IList<IFixedRenderSize>)GetValue(SingleFlowMaxWidthProperty); }
            set { SetValue(SingleFlowMaxWidthProperty, value); }
        }

        public static readonly DependencyProperty OriginalDataSourceProperty =
            DependencyProperty.Register(
                nameof(OriginalDataSource), typeof(IList<IFixedRenderSize>), typeof(WaterfallPresenter),
                new PropertyMetadata(null, OnOriginalDataSourceChanged));
        private static void OnOriginalDataSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            double newValue = (double)e.NewValue;
            double oldValue = (double)e.OldValue;
            if (newValue != 0d && newValue != oldValue)
            {
                (d as WaterfallPresenter).InvalidateMeasure();
            }
        }
        #endregion

        #region SingleFlowMaxWidth
        public double SingleFlowMaxWidth
        {
            get { return (double)GetValue(SingleFlowMaxWidthProperty); }
            set { SetValue(SingleFlowMaxWidthProperty, value); }
        }

        public static readonly DependencyProperty SingleFlowMaxWidthProperty =
            DependencyProperty.Register(
                nameof(SingleFlowMaxWidth), typeof(double), typeof(WaterfallPresenter),
                new PropertyMetadata(0d, OnSingleFlowMaxWidthChanged));

        private static void OnSingleFlowMaxWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            double newValue = (double)e.NewValue;
            double oldValue = (double)e.OldValue;
            if (newValue != 0d && newValue != oldValue)
            {
                (d as WaterfallPresenter).InvalidateMeasure();
            }
        }
        #endregion

        #region VirtualizedAreaHeight
        public double VirtualizedAreaHeight
        {
            get { return (double)GetValue(VirtualizedAreaHeightProperty); }
            set { SetValue(VirtualizedAreaHeightProperty, value); }
        }

        public static readonly DependencyProperty VirtualizedAreaHeightProperty =
            DependencyProperty.Register(
                nameof(VirtualizedAreaHeight), typeof(double), typeof(WaterfallPresenter),
                new PropertyMetadata(0d, OnVirtualizedAreaHeightChanged));

        private static void OnVirtualizedAreaHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            double newValue = (double)e.NewValue;
            double oldValue = (double)e.OldValue;
            if (newValue != 0d && newValue != oldValue)
            {
                WaterfallPresenter panel = d as WaterfallPresenter;
                //panel._virtualizedAreaBottom = panel._virtualizedAreaTop + newValue;
                panel.InvalidateArrange();
            }
        }
        #endregion

        #region VirtualizedAreaOffset
        public double VirtualizedAreaOffset
        {
            get { return (double)GetValue(VirtualizedAreaOffsetProperty); }
            set { SetValue(VirtualizedAreaOffsetProperty, value); }
        }

        public static readonly DependencyProperty VirtualizedAreaOffsetProperty =
            DependencyProperty.Register(
                nameof(VirtualizedAreaOffset), typeof(double), typeof(WaterfallPresenter),
                new PropertyMetadata(0d, OnVirtualizedAreaOffsetChanged));

        private static void OnVirtualizedAreaOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            double newValue = (double)e.NewValue;
            double oldValue = (double)e.OldValue;
            if (newValue != oldValue)
            {
                WaterfallPresenter panel = d as WaterfallPresenter;
                //panel.VirtualizedAreaOffset = panel.VirtualizedAreaOffset + newValue;
                panel.InvalidateArrange();
            }
        }
        #endregion

        #region WaterfallCount
        public int WaterfallCount
        {
            get { return (int)GetValue(WaterfallCountProperty); }
            set { SetValue(WaterfallCountProperty, value); }
        }

        public static readonly DependencyProperty WaterfallCountProperty =
            DependencyProperty.Register(
                nameof(WaterfallCount), typeof(int), typeof(WaterfallPresenter),
                new PropertyMetadata(2, OnWaterfallCountChanged));

        private static void OnWaterfallCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(WaterfallCount), e.NewValue, "Must not be less than 2.");
            }
            Debug.WriteLine("OnWaterfallCountChanged" + e.NewValue);
            if (e.NewValue != e.OldValue)
            {
                (d as FrameworkElement).InvalidateMeasure();
            }
        }
        #endregion

        #region WaterfallInterval
        public double WaterfallInterval
        {
            get { return (double)GetValue(WaterfallIntervalProperty); }
            set { SetValue(WaterfallIntervalProperty, value); }
        }

        public static readonly DependencyProperty WaterfallIntervalProperty =
            DependencyProperty.Register(
                nameof(WaterfallInterval), typeof(double), typeof(WaterfallPresenter),
                new PropertyMetadata(0.0, OnWaterfallIntervalChanged));

        private static void OnWaterfallIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue < 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(WaterfallInterval), e.NewValue, "Must not be negative.");
            }
            (d as FrameworkElement).InvalidateMeasure();
        }
        #endregion


        protected override Size MeasureOverride(Size availableSize)
        {
            Debug.WriteLine("Waterfall Panel: Measure");
            int flowCount = calculateWaterfallCount(availableSize.Width);
            SortedSet<KeyValuePair<int, double>> flowLengthSet = new SortedSet<KeyValuePair<int, double>>(_flowComparer);
            for (int i = 0; i < flowCount; i++)
            {
                flowLengthSet.Add(new KeyValuePair<int, double>(i, 0d));
            }

            double flowWidth = (availableSize.Width - WaterfallInterval * (flowCount - 1)) / flowCount;
            double[] xs = new double[flowCount];
            for (int i = 0; i < flowCount; i++)
            {
                flowLengthSet.Add(new KeyValuePair<int, double>(i, 0d));
                xs[i] = i * flowWidth;
            }

            _arrangeAreas.Clear();
            //foreach (IFixedRenderSize elem in OriginalDataSource)
            for(int i=0;i<OriginalDataSource.Count;i++)
            {
                IFixedRenderSize elem = OriginalDataSource[i];
                double elemLen = elem.FixedRenderHeight;
                var flow = flowLengthSet.Min;
                //Initial the arrange rect.
                int flowIdx = flow.Key;
                double flowLength = flow.Value;
                Point pt = new Point(xs[flowIdx], flowLength);
                ArrangeArea area = new ArrangeArea();
                area.ArrangeRect = new Rect(pt.X, pt.Y, flowWidth, elemLen);
                //Resort the flows according to height.
                flowLengthSet.Remove(flow);
                flowLengthSet.Add(new KeyValuePair<int, double>(flow.Key, flow.Value + elemLen));
            }
            return new Size(availableSize.Width, flowLengthSet.Max.Value);
        }

        /// <summary>
        /// Arrange the children which is scroll to visual area.
        /// It is necessary to decide which to arrange before this call.
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach(ArrangeArea area in _arrangeAreas)
            {
                //if(area.MappingIndex != ArrangeArea.NO_MAPPING)
                //{
                //    Children[area.MappingIndex].Arrange(area.ArrangeRect);
                //}
                if(area.MappingUIElement != null)
                {
                    area.MappingUIElement.Arrange(area.ArrangeRect);
                }
            }
            return base.ArrangeOverride(finalSize);
        }

        //protected override Size ArrangeOverride(Size finalSize)
        //{
        //    Debug.WriteLine("Waterfall Panel: Arrange");
        //    int flowCount = calculateWaterfallCount(finalSize.Width);
        //    SortedSet<KeyValuePair<int, double>> flowLengthSet = new SortedSet<KeyValuePair<int, double>>(_flowComparer);
        //    double flowWidth = (finalSize.Width - WaterfallInterval * (flowCount - 1)) / flowCount;
        //    double[] xs = new double[flowCount];
        //    for (int i = 0; i < flowCount; i++)
        //    {
        //        flowLengthSet.Add(new KeyValuePair<int, double>(i, 0d));
        //        xs[i] = i * flowWidth;
        //    }
        //    foreach (UIElement elem in Children)
        //    {
        //        Size elemSize = elem.DesiredSize;
        //        double elemLen = elemSize.Height;

        //        var flow = flowLengthSet.Min;
        //        int flowIdx = flow.Key;
        //        double flowLength = flow.Value;

        //        Point pt = new Point(xs[flowIdx], flowLength);

        //        //Rect arrangeRect = new Rect(pt, elemSize);
        //        Rect arrangeRect = new Rect(pt.X, pt.Y, flowWidth, elemSize.Height);
        //        elem.Arrange(arrangeRect);
        //        double renderHeight = elem.RenderSize.Height;
        //        flowLengthSet.Remove(flow);
        //        flowLengthSet.Add(new KeyValuePair<int, double>(flow.Key, flow.Value + renderHeight));
        //    }
        //    return finalSize;
        //}


        private WaterflowComparer _flowComparer = new WaterflowComparer();

        private int calculateWaterfallCount(double availableWidth)
        {
            if (SingleFlowMaxWidth == 0d)
                return WaterfallCount;
            return (int)Math.Ceiling(availableWidth / SingleFlowMaxWidth);
        }

        internal class WaterflowComparer : IComparer<KeyValuePair<int, double>>
        {
            public int Compare(KeyValuePair<int, double> x, KeyValuePair<int, double> y)
            {
                return x.Value == y.Value ? x.Key - y.Key :
                    (x.Value > y.Value ? 1 : -1);
            }
        }

        #region Virtualizing properties and method
        private bool isAreaShouldBeVirtualized(Rect rect)
        {
            return (rect.Bottom < VirtualizedAreaOffset || rect.Top > VirtualizedAreaOffset + VirtualizedAreaHeight);
        }

        public interface IVirtualizing
        {
            void Virtualize();
            void Realize();
        }
        #endregion


        private IList<ArrangeArea> _arrangeAreas = new List<ArrangeArea>();


        /// <summary>
        /// There should be a container to hold the real object.
        /// </summary>
        //private ContainerGenerator<UIElement> _containerGenerator;

        //private BaseUIElementGenerator _containerGenerator;

        public BaseUIElementGenerator ContainerGenerator { set; get; }

        public void Update_arrangeAreas(double top, double bottom)
        {
            //foreach(ArrangeArea area in _arrangeAreas)
            for(int i=0;i<_arrangeAreas.Count;i++)
            {
                ArrangeArea area = _arrangeAreas[i];
                Rect rect = area.ArrangeRect;
                if(rect.Bottom > top || rect.Top < bottom)
                {
                    //virtualize
                    ContainerGenerator.CollectCache(area.MappingUIElement);
                    area.MappingUIElement = null;
                }
                else
                {
                    //realize;
                    //Todo: Add a child to container or set values of the item.
                    //Using container or item itself is still not decided.
                    if(area.MappingUIElement == null)
                        area.MappingUIElement = ContainerGenerator.GenerateUIElement(OriginalDataSource[i]);
                }
            }
            InvalidateArrange();
        }
    }
}
