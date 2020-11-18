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
            if (ECUValue >= 3.50)
                return 10;
            else if (ECUValue < 3.50 && ECUValue >= 3.00)
                return 20;
            else if (ECUValue < 3.00 && ECUValue >= 2.60)
                return 30;
            else if (ECUValue < 2.60 && ECUValue >= 2.10)
                return 40;
            else if (ECUValue < 2.10 && ECUValue >= 1.70)
                return 50;
            else if (ECUValue < 1.70 && ECUValue >= 1.30)
                return 60;
            else if (ECUValue < 1.30 && ECUValue >= 1.00)
                return 70;
            else if (ECUValue < 1.00 && ECUValue >= 0.80)
                return 80;
            else if (ECUValue < 0.80 && ECUValue >= 0.60)
                return 90;
            else if (ECUValue < 0.60 && ECUValue >= 0.45)
                return 100;
            else if (ECUValue < 0.45 && ECUValue >= 0.35)
                return 110;
            else if (ECUValue < 0.35 && ECUValue >= 0.25)
                return 120;
            else
                return 0;

        }
    }
}
