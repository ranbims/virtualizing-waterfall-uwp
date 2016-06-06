using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VirtualizingWaterfall
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public List<IFixedRenderSize> dataList;

        public MainPage()
        {
            dataList = new List<IFixedRenderSize>();
            for(int i=0;i<100;i++)
            {
                dataList.Add(new SampleData(i.ToString()));
            }
            
            this.InitializeComponent();
            waterfall.LoadMoreItemEvent += Waterfall_LoadMoreItemEvent;
        }

        private void Waterfall_LoadMoreItemEvent(VirtualizingWaterfall waterfall, double offsetToBottom)
        {
            if(waterfall.DataSource != null)
            {
                for(int i=0;i<20;i++)
                {
                    waterfall.DataSource.Add(new SampleData(i.ToString()));
                }
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            base.OnNavigatedTo(e);
            await Task.Delay(1000);
            waterfall.ContainerGenerator = new SampleUIElementGenerator(waterfall.Panel);
            waterfall.DataSource = dataList;
            
        }
    }
}
