using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Broadcaster
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        RMQ rmq = new RMQ();
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void PageLoadConnect(object sender, RoutedEventArgs e)
        {
            rmq.InitRMQConnection();
            rmq.CreateRMQConnection();
            while (!rmq.connection.IsOpen) { /*loooping hingga koneksi terbuka*/ }
            rmq.CreateRMQChannel("workQ");
            while (!rmq.channel.IsOpen) { /*loooping hingga channel terbuka*/ }
            if (rmq.connection.IsOpen && rmq.channel.IsOpen)
            {
                Button.IsEnabled = true;
                Button1.IsEnabled = true;
            }

        }

        private void CloseRMQ(object sender, RoutedEventArgs e)
        {
            rmq.Disconnect();
            Button.IsEnabled = false;
            Button1.IsEnabled = false;
        }

        private void SendData(object sender, RoutedEventArgs e)
        {
            rmq.SendMessage(TextBox.Text, "", TextBox1.Text);
        }
    }
}
