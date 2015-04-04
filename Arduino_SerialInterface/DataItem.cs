using System;
using System.Collections.Generic;
using System.Linq;
using System;
//using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
//using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Configuration;

namespace Arduino_SerialInterface
{
    class DataItem:INotifyPropertyChanged
    {
        SerialPort arduinoBoard = new SerialPort();
        //DateTime date;
        //MotorSpeed selectedSpeed;
        public static string numberOfTurnsCompleted ="";
        public static string totalNumberofTurns ="1";
        public static int selectedSpeed=1;
        public string TotalNumberOfTurns {
            get {
                return totalNumberofTurns;
            }
            set
            {
                if(totalNumberofTurns != value)
                {
                    totalNumberofTurns = value;
                }
            }
        }

        public string NumberOfTurnsCompleted
        {
            get
            {
                return numberOfTurnsCompleted;
            }
            set
            {
                if(numberOfTurnsCompleted != value)
                {
                    numberOfTurnsCompleted = value;
                    RaisePropertyChanged("NumberOfTurnsCompleted");
                }
            }
        }


        public int SelectedSpeed { get
        {
            return selectedSpeed;
        }
            set
            {
                if (selectedSpeed != value)
                {
                    selectedSpeed = value;
                }
            }
        }

        public event EventHandler NewDataReceived;

        public void CloseArduinoConnection()
        {
            arduinoBoard.Close();
        }
        public void OpenArduinoConnection()
        {
            if (!arduinoBoard.IsOpen)
            {
                Task.Run(() =>
                    {
                        arduinoBoard.DataReceived += arduinoBoard_DataReceived;
                        arduinoBoard.PortName = ConfigurationSettings.AppSettings["ArduinoPort"];
                        arduinoBoard.BaudRate = 9600;
                        arduinoBoard.Open();
                    });
            }
            else
            {
                throw new InvalidOperationException("The Serial Port is already open!");
            }
        }
        //public MotorSpeed SelectedSpeed
        //{
        //    get { return selectedSpeed; }
        //    set
        //    {
        //        if(selectedSpeed != value)
        //        {
        //            selectedSpeed = value;
        //        }
        //    }
        //}

        public void SendDataToArduinoBoard()
        {
            if (arduinoBoard.IsOpen)
            {
                var allData = string.Join("$","$", totalNumberofTurns, selectedSpeed, "#");
                arduinoBoard.Write(allData);
                //arduinoBoard.Write("1#");
            }
            else
            {
                throw new InvalidOperationException("Can't send data if the serial Port is closed!");
            }
        }
        void arduinoBoard_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
                    string data = arduinoBoard.ReadTo("\x03");//Read until the EOT code
                    //multiple data 
                    NumberOfTurnsCompleted = data;

                    //            string[] dataArray = data.Split(new string[] { "\x02", "$" }, StringSplitOptions.RemoveEmptyEntries);

                    if (NewDataReceived != null)//If there is someone waiting for this event to be fired
                    {
                        NewDataReceived(this, new EventArgs()); //Fire the event, indicating that new WeatherData was added to the list.
                    }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            //PropertyChangedEventHandler handler = PropertyChanged;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public enum MotorSpeed
    {
        Low,
        Medium,
        High
    }
}
