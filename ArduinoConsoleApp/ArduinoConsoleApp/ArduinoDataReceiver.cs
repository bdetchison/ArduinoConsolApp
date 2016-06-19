using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConsoleApp
{
    public class ArduinoDataReceiver : ISerialPortDataReceiver
    {
        private List<VibrationData> vibrationData = new List<VibrationData>();
        private DataAccess dataAccess;

        public ArduinoDataReceiver(string configFilePath)
        {
            using (StreamReader sr = new StreamReader(configFilePath))
            {
                // Read the stream to a string, and write the string to the console.
                dataAccess = new DataAccess(sr.ReadToEnd());
            }
        }

        public void DataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort serialPort = (SerialPort)sender;
                var data = serialPort.ReadLine();

                if (data == "Done\r")
                {
                    if (vibrationData.Count > 0)
                    {
                        dataAccess.InsertData(VelocityCalculator.CalculateVelocity(vibrationData), DateTime.Now);
                        vibrationData.Clear();
                    }
                }
                else
                {
                    Console.Write(data + Environment.NewLine);

                    var splitData = data.Replace("\r", "").Split(':');

                    if (splitData.Length == 4)
                    {
                        vibrationData.Add(new VibrationData
                        {
                            x = float.Parse(splitData[1], CultureInfo.InvariantCulture.NumberFormat),
                            y = float.Parse(splitData[2], CultureInfo.InvariantCulture.NumberFormat),
                            z = float.Parse(splitData[3], CultureInfo.InvariantCulture.NumberFormat),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                //todo add more exception handling
                throw ex;
            }
        }
    }
}
