using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace ArduinoConsoleApp
{
    class Program
    {
        private static int showLight = 0;
        private static DateTime lastTimeGathered;
        private static SerialPortCommunicator serialPortCommunicator;
        private static ISerialPortDataReceiver arduinoDataReceiver;

        static void Main(string[] args)
        {
            arduinoDataReceiver = new ArduinoDataReceiver(ConfigurationManager.AppSettings.Get("connStringFile"));

            serialPortCommunicator = new SerialPortCommunicator(arduinoDataReceiver);
          
            try
            {
                if (!serialPortCommunicator.LocalSerialPort.IsOpen)
                {
                    serialPortCommunicator.LocalSerialPort.Open();
                }

            }
            catch (Exception ex)
            {
                //todo add more exception handling
                throw ex;
            }


            while (true)
            {
                //Thread.Sleep(2050);

                if (lastTimeGathered == null || DateTime.Now > lastTimeGathered.AddSeconds(60))
                {
                    lastTimeGathered = DateTime.Now;
                    showLight = 1;
                    serialPortCommunicator.LocalSerialPort.WriteLine(showLight.ToString());
                }
                else if (DateTime.Now > lastTimeGathered.AddSeconds(2) && showLight == 1)
                {
                    showLight = 0;
                    serialPortCommunicator.LocalSerialPort.WriteLine(showLight.ToString());
                }
            }
        }
    }
}
