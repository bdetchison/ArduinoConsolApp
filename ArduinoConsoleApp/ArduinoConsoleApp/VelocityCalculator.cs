using System;
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
            var timeInSeconds = 0;
            VibrationData previous=null;
            VibrationData current;

            foreach(VibrationData reading in vibrationData)
            {
                if (previous != null)
                {
                    reading.changeInX = GetChangeInAccecleration(reading.x, previous.x);
                    reading.changeInY = GetChangeInAccecleration(reading.y, previous.y);
                    reading.changeInZ = GetChangeInAccecleration(reading.z, previous.z);
                    reading.time = previous.time + .2f;
                    reading.velocityX = CalculateAxisVelocity(reading.changeInX, previous.changeInX, reading.time, previous.time, previous.velocityX);
                    reading.velocityY = CalculateAxisVelocity(reading.changeInY, previous.changeInY, reading.time, previous.time, previous.velocityY);
                    reading.velocityZ = CalculateAxisVelocity(reading.changeInZ, previous.changeInZ, reading.time, previous.time, previous.velocityZ);

                }
                else
                {
                    reading.changeInX = 0f;
                    reading.changeInY = 0f;
                    reading.changeInZ = 0f;
                    reading.velocityX = 0;
                    reading.velocityY = 0;
                    reading.velocityZ = 0;
                    reading.time = 0;
                }

           
                previous = reading;
            }


            return NormalizeVelocity(vibrationData);
         
            //rough estimation of 
            ////////////var firstReading = vibrationData.First();
            ////////////var lastReading = vibrationData.Last();
            ////////////var timeInSeconds = 2;  //Assume readings taken over 3 seconds

            ////////////var xVelocity = IntegrateAccelerometerData(firstReading.x, lastReading.x, timeInSeconds);
            ////////////var yVelocity = IntegrateAccelerometerData(firstReading.y, lastReading.y, timeInSeconds);
            ////////////var zVelocity = IntegrateAccelerometerData(firstReading.z, lastReading.z, timeInSeconds);


            ////////////return Math.Sqrt( Math.Pow(xVelocity,2) + Math.Pow(yVelocity,2) + Math.Pow(zVelocity,2));
        }

        private static double NormalizeVelocity(List<VibrationData> vibrationData)
        {
          return  Math.Sqrt(Math.Pow((vibrationData.Sum(x => x.velocityX) / vibrationData.Count()), 2) + Math.Pow((vibrationData.Sum(x => x.velocityY) / vibrationData.Count()), 2) + Math.Pow((vibrationData.Sum(x => x.velocityZ) / vibrationData.Count()), 2));

        }

        private static float CalculateAxisVelocity(float changeCurrent, float changePrevious, float timeInSecondsCurrent, float timeInSecondsPrevious, float previousVelocity)
        {
            return previousVelocity + (((changePrevious + changeCurrent) / 2) * (timeInSecondsCurrent - timeInSecondsPrevious));
        }


        private static float GetChangeInAccecleration(float current, float previous)
        {
            return current - previous;
        }

        private static float IntegrateAccelerometerData(float before, float after, int timeInSeconds)
        {
            return (ConvertGtoIps(after) - ConvertGtoIps(before)) / (timeInSeconds);
        }


        private static float ConvertGtoIps(float number)
        {
            return number;
            //return number * 386.08858267716f;
        }


    }
}
