using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Configuration;
namespace Arduino_SerialInterface
{
    /// <summary>
    /// Encapsulates the communication from and to
    /// an Arduino Board which sends weather data
    /// it has stored previously
    /// </summary>
    class WeatherDataContainer
    {
        /// <summary>
        /// Interface for the Serial Port at which an Arduino Board
        /// is connected.
        /// </summary>
        SerialPort arduinoBoard = new SerialPort();

        /// <summary>
        /// Holds a List of <see cref="DataItem"/> in order
        /// to store weather data received from an Arduino Board.
        /// </summary>
        //ObservableCollection<DataItem> weatherDataItems = new ObservableCollection<DataItem>();

        /// <summary>
        /// Raised when new  <see cref="DataItem"/>s are added
        /// to the internal weather data list.
        /// </summary>
        public event EventHandler NewWeatherDataReceived;

        /// <summary>
        /// Gets a list of <see cref="DataItem"/> which was
        /// previsously retrieved from an Arduino Board.
        /// </summary>
        //internal ObservableCollection<DataItem> WeatherDataItems
        //{
        //    get { return weatherDataItems; }
        //}
        /// <summary>
        /// Closes the connection to an Arduino Board.
        /// </summary>
        public void CloseArduinoConnection()
        {
            arduinoBoard.Close();
        }
        /// <summary>
        /// Opens the connection to an Arduino board
        /// </summary>
        public void OpenArduinoConnection()
        {
            //DataItem item = new DataItem();
            //item.Date = DateTime.Now;
            //item.TemparatureCelsius = (float)5.5;
            //weatherDataItems.Add(item);
            //item = new DataItem();
            //item.Date = DateTime.Now;
            //item.TemparatureCelsius = (float)10;
            //weatherDataItems.Add(item);
            //item = new DataItem();
            //item.Date = DateTime.Now;
            //item.TemparatureCelsius = (float)11.3;
            //weatherDataItems.Add(item);
            //item = new DataItem();
            //item.Date = DateTime.Now;
            //item.TemparatureCelsius = (float)8.55;
            //weatherDataItems.Add(item);
            if(!arduinoBoard.IsOpen)
            {
                arduinoBoard.DataReceived += arduinoBoard_DataReceived;
                arduinoBoard.PortName =  ConfigurationSettings.AppSettings["ArduinoPort"];
                arduinoBoard.BaudRate = 9600;
                arduinoBoard.Open();
            }
            else
            {
                throw new InvalidOperationException("The Serial Port is already open!");
            }
        }
        /// <summary>
        /// Sends the command to the Arduino board which triggers the board
        /// to send the weather data it has internally stored.
        /// </summary>
        public void GetWeatherDataFromArduinoBoard()
        {
            if (arduinoBoard.IsOpen)
            {
                arduinoBoard.Write("1#");
            }
            else
            {
                throw new InvalidOperationException("Can't get weather data if the serial Port is closed!");
            }
        }
        /// <summary>
        /// Reads weather data from the arduinoBoard serial port
        /// </summary>
        void arduinoBoard_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = arduinoBoard.ReadTo("\x03");//Read until the EOT code
            //Split into 'date=temparature' formatted text
            string[] dataArray = data.Split(new string[] {"\x02", "$" }, StringSplitOptions.RemoveEmptyEntries); 
            //Iterate through the splitted data and parse it into weather data items
            //and add them to the list of received weather data.
            foreach (string dataItem in dataArray.ToList())
            {
                ////WeatherDataItems.Clear();
                //DataItem weatherDataItem = new DataItem();
                //weatherDataItem.FromString(dataItem);
                //weatherDataItems.Add(weatherDataItem);
            }
            if(NewWeatherDataReceived != null)//If there is someone waiting for this event to be fired
            {
                NewWeatherDataReceived(this, new EventArgs()); //Fire the event, indicating that new WeatherData was added to the list.
            }
        }
    }
}
