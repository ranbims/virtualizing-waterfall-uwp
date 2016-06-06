using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace VirtualizingWaterfall
{
    public sealed partial class MemoryControl : UserControl
    {

        private ThreadPoolTimer _timer = null;
        private static MemoryControl _instance = null;

        public MemoryControl()
        {
            this.InitializeComponent();
            _instance = this;

#if DEBUG
            this.UpdateState();
            if (_timer == null)
            {
                _timer = ThreadPoolTimer.CreatePeriodicTimer(TimerHandler, TimeSpan.FromSeconds(1));
            }
            //    this.tb_Debug.Text = DataHelper.MtopWrapper.MtopHelper.GetCurrentEnviromentString();

            this.Visibility = Visibility.Visible;
#else
            this.Visibility = Visibility.Collapsed;
#endif
        }


        private async void TimerHandler(ThreadPoolTimer timer)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                this.UpdateState();
            });
        }

        /// <summary>
        /// update timer, network and battery state
        /// </summary>
        private void UpdateState()
        {
            this.tb_Memory.Text = (MemoryManager.AppMemoryUsage / 1024 / 1024).ToString();
        }

        //private void tb_Debug_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    DataHelper.MtopWrapper.MtopHelper.ChangeEnviromentToNext();
        //    this.tb_Debug.Text = DataHelper.MtopWrapper.MtopHelper.GetCurrentEnviromentString();
        //}

        public static void DebugOutput(string msg)
        {
#if DEBUG
            _instance.tb_Message.Text += "\r\n" + msg;
            // HelpFunctions.Output(msg);
#endif
        }

        public static void ClearOutput()
        {
            _instance.tb_Message.Text = string.Empty;
        }
    }
}
