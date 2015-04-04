using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChartPainter3D;

namespace Arduino_SerialInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Contains a collection of <see cref="DataItem"/>
        /// and control methods to send commands to the Arduino Board
        /// </summary>
        DataItem data = new DataItem();
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                data.OpenArduinoConnection();
            }
            catch (Exception)
            {
                MessageBox.Show("Error: Can not connect to the Arduino Board - Configure the COM Port in the app.config file and check whether an Arduino Board is connected to your computer.");
            }
        }
        /// <summary>
        /// OnWeatherDataReceived event is catched in
        /// order to update the weather data display on the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void weatherData_NewWeatherDataReceived(object sender, EventArgs e)
        {
            //TODO:new data rx
            //.BeginInvoke(new ThreadStart(DrawChart));
            //Dispatcher.BeginInvoke(new ThreadStart(() =>
               // weatherDataGrid.ItemsSource = weatherData.WeatherDataItems));

        }
        /// <summary>
        /// Click-Event for the Button to get weather data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getWeatherData_Click(object sender, RoutedEventArgs e)
        {
            //var test1 =
            data.SendDataToArduinoBoard();
        }

        private void testClick(object sender, RoutedEventArgs e)
        {
            
            var test = data.NumberOfTurnsCompleted;
            TestName.Text = test;
        }
        //private void DrawChart()
        //{
        //    List<double> temparatures = new List<double>();
        //    List<string> captions = new List<string>();
        //    foreach (WeatherDataItem item in weatherData.WeatherDataItems)
        //    {
        //        temparatures.Add(item.TemparatureCelsius);
        //        captions.Add(string.Format("{0}\n{1} C", item.Date.ToShortDateString(), item.TemparatureCelsius.ToString()));
        //    }
        //    ChartDataContainer container = new ChartDataContainer();
        //    container.Data = temparatures;
        //    container.DataCaptions = captions;
        //    container.MaxSize = new Point(800, 300);
        //    container.XAxisText = "Date / Temparature (Celsius)";
        //    container.YAxisText = "";
        //    container.Offset = new Point(20, 20);
        //    container.ChartElementColor = Colors.BurlyWood;
        //    CylinderChartPainter.DrawChart(container, weatherChartCanvas);
        //}
    }
}
