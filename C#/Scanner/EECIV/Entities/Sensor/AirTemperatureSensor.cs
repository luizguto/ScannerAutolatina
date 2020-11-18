using EECIV.Entities.Enum;
using EECIV.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace EECIV.Entities.Sensor
{
    //Sensor temperatura do ar
    public class AirTemperatureSensor : ISensor
    {
        public string Name { get; set; }
        public SensorType Type => SensorType.AirTemperature;
        public float ECUValue { get; set; }

        public object ECUValueToSensorValue()
        {
            if (ECUValue >= 3.10)
                return 15;
            else if (ECUValue < 3.10 && ECUValue >= 2.10)
                return 20;
            //else if (ECUValue < 3.10 && ECUValue >= 2.85)
            //    return 25;
            //else if (ECUValue < 2.85 && ECUValue >= 2.60)
            //    return 30;
            //else if (ECUValue < 2.60 && ECUValue >= 2.35)
            //    return 35;
            else if (ECUValue < 2.10 && ECUValue >= 1.30)
                return 40;
            //else if (ECUValue < 2.10 && ECUValue >= 1.90)
            //    return 45;
            //else if (ECUValue < 1.90 && ECUValue >= 1.70)
            //    return 50;
            //else if (ECUValue < 1.70 && ECUValue >= 1.50)
            //    return 55;
            else if (ECUValue < 1.30 && ECUValue >= 0.80)
                return 60;
            //else if (ECUValue < 1.30 && ECUValue >= 1.18)
            //    return 65;
            //else if (ECUValue < 1.18 && ECUValue >= 1.05)
            //    return 70;
            //else if (ECUValue < 1.05 && ECUValue >= 0.93)
            //    return 75;
            else if (ECUValue < 0.80 && ECUValue >= 0.60)
                return 80;
            //else if (ECUValue < 0.80 && ECUValue >= 0.70)
            //    return 85;
            else if (ECUValue < 0.60 && ECUValue >= 0.50)
                return 90;
            //else if (ECUValue < 0.60 && ECUValue >= 0.55)
            //    return 95;
            else if (ECUValue < 0.50 && ECUValue >= 0.00)
                return 100;
            else
                return 0;

        }
    }
}
