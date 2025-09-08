using CryptoMonitor.Data;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CryptoMonitor
{
    public partial class AppPanel : Window
    {
        private const int INTERVAL = 20;

        private List<int> data = new List<int>();
        private DispatcherTimer timer;

        public AppPanel()
        {
            InitializeComponent();

            this.Left = SystemParameters.PrimaryScreenWidth - 15 - this.Width;
            this.Top = SystemParameters.PrimaryScreenHeight - 60 - this.Height;

            Process();

            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(INTERVAL);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Process();
        }

        private async void Process()
        {
            int p = await BinanceApi.GetBtcPrice();

            data.Add(p);
            if (data.Count > 180)
            {
                data.RemoveAt(0);
            }

            lblPrice.Content = p.ToString("### ##0 $");

            bool plus = true;
            if (data.Count > 1) plus = (data[data.Count - 1] > data[data.Count - 2]);
            lblPrice.Foreground = plus ? Brushes.LightGreen : Brushes.Red;

            MemoryStream msImage = LineChart.GenerateImageIntoMemoryStream(data);
            msImage.Seek(0, SeekOrigin.Begin);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = msImage;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            imgChart.Source = bitmap;
        }

        private void Window_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
