using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArduinoConsoleApp
{
    public class ArduinoDataReceiver : ISerialPortDataReceiver
    {
        private List<VibrationData> vibrationData = new List<VibrationData>();
        private string serviceUrl;

        public ArduinoDataReceiver(string configFilePath)
        {
            using (StreamReader sr = new StreamReader(configFilePath))
            {
                // Read the stream to a string, and write the string to the console.
                serviceUrl = sr.ReadToEnd();
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
                        PostDataToService(VelocityCalculator.CalculateVelocity(vibrationData), DateTime.Now);
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

        private void PostDataToService(double velocity, DateTime date)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(serviceUrl +  string.Format("?velocity={0}&date={1}",velocity,date));
            httpWebRequest.ContentType = "multipart/form-data";
            httpWebRequest.ContentLength= 0;
            httpWebRequest.Method = "POST";
        
            try
            {
                ThreadPool.QueueUserWorkItem(o => { httpWebRequest.GetResponse(); });
            }
            catch
            {
                //TODO: create real way to handle the service being down
                //this is OK for now, if the service is down, keep on trucking
            }
        }

    }
}
