using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConsoleApp
{
    public class VibrationData
    {
        public DateTime Date { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public float changeInX { get; set; }
        public float changeInY { get; set; }
        public float changeInZ { get; set; }
        public float velocityX { get; set; }
        public float velocityY { get; set; }
        public float velocityZ { get; set; }
        public float time { get; set; }
    }
}
