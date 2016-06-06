using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VirtualizingWaterfall;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace VirtualizingWaterfall
{
    public sealed class VirtualizingWaterfall : Control
    {

        private ScrollViewer _scrollViewer;
        private WaterfallPresenter _waterfallPresenter;

        public WaterfallPresenter Panel
        {
            get
            {
                return _waterfallPresenter;
            }
        }

        IList<IFixedRenderSize> _dataSource;

        public IList<IFixedRenderSize> DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                _dataSource = value;
                if (_waterfallPresenter != null)
                {
                    _waterfallPresenter.OriginalDataSource = value;
                }
            }
        }

        private BaseUIElementGenerator _containerGenerator;

        public BaseUIElementGenerator ContainerGenerator
        {
            set
            {
                _containerGenerator = value;
                if(_waterfallPresenter != null)
                {
                    _waterfallPresenter.ContainerGenerator = value;
                }
            }
        }

        public delegate void LoadMoreDataEventHandler(VirtualizingWaterfall waterfall, double offsetToBottom);

        /// <summary>
        /// This is only a simple event to show that scroller is near the bottom.
        /// What you should do is loading data in your own method.
        /// </summary>
        public event LoadMoreDataEventHandler LoadMoreItemEvent;

        public VirtualizingWaterfall()
        {
            this.DefaultStyleKey = typeof(VirtualizingWaterfall);
        }

        protected override void OnApplyTemplate()
        {
            //Initialize _scrollViewer.
            _scrollViewer = base.GetTemplateChild("scrollViewer") as ScrollViewer;
            _scrollViewer.ViewChanging += OnScrollViewerChanging;
            _scrollViewer.ViewChanged += OnScrollViewerChanged;

            _waterfallPresenter = base.GetTemplateChild("waterfallPresenter") as WaterfallPresenter;
            _waterfallPresenter.ContainerGenerator = _containerGenerator;
            _waterfallPresenter.OriginalDataSource = _dataSource;
            //Todo: Set ContainerGenerator of _waterfallPresenter.
            base.OnApplyTemplate();
        }

        private void OnScrollViewerChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            _waterfallPresenter.UpdateArrangeAreas(_scrollViewer.VerticalOffset, _scrollViewer.VerticalOffset + _scrollViewer.ActualHeight);
        }

        private void OnScrollViewerChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            double offset = e.FinalView.VerticalOffset;
            ScrollViewer scrollViewer = sender as ScrollViewer;
            if (scrollViewer.ExtentHeight - this.ActualHeight - offset < 500)
            {
                if (LoadMoreItemEvent != null)
                {
                    LoadMoreItemEvent(this, scrollViewer.ExtentHeight - this.ActualHeight - offset);
                    _waterfallPresenter.UpdateOriginalDataSource();
                }
            }
        }

        /// <summary>
        /// If you change the data source by adding and removing the elements from the collection
        /// rather than changing the reference, please call this method to ensure data is ready to be readered.
        /// 
        /// Important: Adding or removing should be used at the end of the data list.
        /// </summary>
        public void UpdataDataSource()
        {
            if(_waterfallPresenter != null)
            {
                _waterfallPresenter.UpdateOriginalDataSource();
            }
        }
    }
}
