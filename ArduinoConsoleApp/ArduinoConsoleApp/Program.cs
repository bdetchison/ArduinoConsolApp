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

namespace ArduinoConsoleApp
{
    class Program
    {
        private static int showLight = 0;
        private static string connectionString;

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
                Thread.Sleep(10000);

                if (showLight == 0)
                {
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
            //throw new NotImplementedException();
            try
            {
                SerialPort serialPort = (SerialPort)sender;
                var data = serialPort.ReadLine();
                float result;

                Console.Write(data + Environment.NewLine);

                data = data.Replace(" ", "").Replace("\r", "");
                var splitData = data.Split(':');

                if (splitData.Length == 2  && float.TryParse(splitData[1], out result))
                {
                    InsertData(result);
                }

                //counter++;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void InsertData(float data)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand();

                cmd.CommandText = "InsertVibrationData";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("Velocity", SqlDbType.Decimal).Value = data;
                cmd.Parameters.Add("Date", SqlDbType.DateTime2).Value = DateTime.Now;
            

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
