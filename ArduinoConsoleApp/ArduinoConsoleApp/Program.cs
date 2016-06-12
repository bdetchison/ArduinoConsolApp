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
        private static string connectionString;
        private static DateTime lastTimeGathered;
        private static List<VibrationData> vibrationData = new List<VibrationData>();

        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader(ConfigurationManager.AppSettings.Get("connStringFile")))
            {
                // Read the stream to a string, and write the string to the console.
                connectionString = sr.ReadToEnd();
            }



            SerialPort serialPort = new SerialPort();
            serialPort.BaudRate = 9600;
            serialPort.PortName = "COM5";
            serialPort.Encoding = Encoding.ASCII;


            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(DataRecieved);

            while (true)
            {
                Thread.Sleep(2000);

                if (lastTimeGathered == null || DateTime.Now > lastTimeGathered.AddMinutes(1))
                {
                    lastTimeGathered = DateTime.Now;
                    showLight = 1;
                }
                else
                {
                    showLight = 0;
                }

                serialPort.WriteLine(showLight.ToString());
            }
        }

        private static void DataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort serialPort = (SerialPort)sender;
                var data = serialPort.ReadLine();

                if (data == "Done\r")
                {
                    if (vibrationData.Count > 0)
                    {
                        InsertData(VelocityCalculator.CalculateVelocity(vibrationData), DateTime.Now);
                        vibrationData.Clear();
                    }
                }
                else
                {
                    Console.Write(data + Environment.NewLine);
                    Console.Write(now + Environment.NewLine);

                    var splitData = data.Replace("\r","").Split(':');

                    if (splitData.Length == 4)
                    {
                        vibrationData.Add(new VibrationData
                        {
                            Date = now,
                            x = float.Parse(splitData[1], CultureInfo.InvariantCulture.NumberFormat),
                            y = float.Parse(splitData[2], CultureInfo.InvariantCulture.NumberFormat),
                            z = float.Parse(splitData[3], CultureInfo.InvariantCulture.NumberFormat),
                        });
                    }

                    

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void InsertData(double velocity, DateTime date)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand();

                cmd.CommandText = "InsertVibrationData";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("Velocity", SqlDbType.Decimal).Value = velocity;
                cmd.Parameters.Add("Date", SqlDbType.DateTime2).Value = date;
            

                cmd.Connection = connection;

                connection.Open();
                Console.WriteLine("Connected successfully.");

                cmd.ExecuteNonQuery();
                Console.WriteLine("Insert successfull");

                //Console.WriteLine("Press any key to finish...");
                //Console.ReadKey(true);
            }
        }
    }
}
