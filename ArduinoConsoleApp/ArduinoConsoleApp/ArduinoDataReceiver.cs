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

                Console.Write(data + Environment.NewLine);

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
                    var splitData = data.Replace("\r", "").Split(':');

                    if (splitData.Length == 4)
                    {
                        float x;
                        float y;
                        float z;

                        bool xIsFloat = float.TryParse(splitData[1], out x);
                        bool yIsFloat = float.TryParse(splitData[2], out y);
                        bool zIsFloat = float.TryParse(splitData[3], out z);

                        if (xIsFloat && yIsFloat && zIsFloat)
                        {
                            vibrationData.Add(new VibrationData
                            {
                                x = x,
                                y = y,
                                z = z,
                            });
                        }
                        else
                        {
                            string blah = "";
                        }
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
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(serviceUrl + string.Format("?velocity={0}&date={1}", velocity, date));
            httpWebRequest.ContentType = "multipart/form-data";
            httpWebRequest.ContentLength = 0;
            httpWebRequest.Method = "POST";


            ThreadPool.QueueUserWorkItem(o =>
            {
                try
                {
                     var test =httpWebRequest.GetResponse();
                }
                catch(Exception ex)
                {
                    //TODO: create real way to handle the service being down
                    //this is OK for now, if the service is down, keep on trucking
                }
            });
        }
    }
}
