﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConsoleApp
{
    public static class VelocityCalculator
    {
        public static double CalculateVelocity(List<VibrationData> vibrationData)
        {
            //rough estimation of 
            var firstReading = vibrationData.First();
            var lastReading = vibrationData.Last();
            var timeInSeconds = 2;  //Assume readings taken over 3 seconds

            var xVelocity = IntegrateAccelerometerData(firstReading.x, lastReading.x, timeInSeconds);
            var yVelocity = IntegrateAccelerometerData(firstReading.y, lastReading.y, timeInSeconds);
            var zVelocity = IntegrateAccelerometerData(firstReading.z, lastReading.z, timeInSeconds);


            return Math.Sqrt( Math.Pow(xVelocity,2) + Math.Pow(yVelocity,2) + Math.Pow(zVelocity,2));
        }

        private static double IntegrateAccelerometerData(double before, double after, int timeInSeconds)
        {
            return (after - before) / (timeInSeconds);
        }


    }
}
