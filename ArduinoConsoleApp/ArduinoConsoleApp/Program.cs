using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Threading;

namespace ArduinoConsoleApp
{
    class Program
    {
        private static int showLight = 0;
     

        static void Main(string[] args)
        {
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
                Console.Write(serialPort.ReadLine() + Environment.NewLine);
                counter++;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
