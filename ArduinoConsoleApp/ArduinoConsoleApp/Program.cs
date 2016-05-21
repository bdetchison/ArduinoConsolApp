using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;

namespace ArduinoConsoleApp
{
    class Program
    {

        private static int counter = 0;
        private static int showLight = 0;
     

        static void Main(string[] args)
        {
            SerialPort serialPort = new SerialPort();
            serialPort.BaudRate = 9600;
            serialPort.PortName = "COM5";


            SerialPort inputSerialPort = new SerialPort();
            inputSerialPort.BaudRate = 9600;
            inputSerialPort.PortName = "COM6";


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
                if (counter % 10 ==0)
                {
                    if (showLight==0)
                    {
                        showLight = 1;
                    }
                    else
                    {
                        showLight = 0;
                    }

                    serialPort.Write(showLight.ToString());
                }
            }
        }

        private static void DataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                SerialPort serialPort = (SerialPort)sender;
                Console.Write(serialPort.ReadLine() + Environment.NewLine);
                Console.Write(counter + Environment.NewLine);
                counter++;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      
    }
}
