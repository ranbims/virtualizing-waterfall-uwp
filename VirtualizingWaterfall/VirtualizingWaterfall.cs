using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        public IList<IFixedRenderSize> DataSource
        {
            get
            {
                return _waterfallPresenter.OriginalDataSource;
            }
            set
            {
                _waterfallPresenter.OriginalDataSource = value;
            }
        }

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
            //Todo: Set ContainerGenerator of _waterfallPresenter.
            base.OnApplyTemplate();
        }

        private void OnScrollViewerChanged(object sender, ScrollViewerViewChangedEventArgs e) { }

        private void OnScrollViewerChanging(object sender, ScrollViewerViewChangingEventArgs e) { }
    }
}
