using CryptoMonitor.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CryptoMonitor
{
    public partial class AppPanel : Window
    {
        private const int INTERVAL = 30;
        private const int LIMIT = 60;

        private List<decimal> dataBtc = new List<decimal>();
        private List<decimal> dataEth = new List<decimal>();

        private DispatcherTimer timer;

        public AppPanel()
        {
            InitializeComponent();

            this.Left = SystemParameters.PrimaryScreenWidth - 15 - this.Width;
            this.Top = SystemParameters.PrimaryScreenHeight - 60 - this.Height;

            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(INTERVAL);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Process();
            timer.Start();
        }

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            await Process();
        }

        private void Window_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async Task Process()
        {
            var price = await BinanceApi.GetPrice();
            
            dataBtc.Add(price.Btc);
            if (dataBtc.Count > LIMIT) dataBtc.RemoveAt(0);

            ShowData(dataBtc, lblBtc, imgBtc);

            dataEth.Add(price.Eth);
            if (dataEth.Count > LIMIT) dataEth.RemoveAt(0);

            ShowData(dataEth, lblEth, imgEth);
        }

        private void ShowData(List<decimal> data, Label lbl, Image img)
        {
            if (data.Count == 0) return;

            bool plus = true;
            if (data.Count > 1) plus = (data[data.Count - 1] > data[data.Count - 2]);
            lbl.Foreground = plus ? Brushes.LightGreen : Brushes.Red;

            lbl.Content = data[data.Count - 1].ToString("### ##0 $");

            MemoryStream msImage = LineChart.GenerateImageIntoMemoryStream(data);
            msImage.Seek(0, SeekOrigin.Begin);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = msImage;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            img.Source = bitmap;
        }
    }
}
