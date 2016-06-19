using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConsoleApp
{
    public interface ISerialPortDataReceiver
    {
        void DataRecieved(object sender, SerialDataReceivedEventArgs e);
    }
}
