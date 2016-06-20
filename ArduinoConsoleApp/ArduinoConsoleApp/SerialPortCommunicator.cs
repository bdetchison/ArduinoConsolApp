using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConsoleApp
{
    public class SerialPortCommunicator
    {
        public SerialPort LocalSerialPort { get; set; }
        
        public SerialPortCommunicator(ISerialPortDataReceiver dataPointReceiver)
        {
            LocalSerialPort = new SerialPort();
            this.LocalSerialPort.BaudRate = 9600;
            this.LocalSerialPort.PortName = "COM5";
            this.LocalSerialPort.Encoding = Encoding.ASCII;

            this.LocalSerialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(dataPointReceiver.DataRecieved);
        }
    }
}
