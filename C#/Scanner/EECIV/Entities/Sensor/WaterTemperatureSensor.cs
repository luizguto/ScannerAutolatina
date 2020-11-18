using EECIV.Entities.Enum;
using EECIV.Interface;
using System;

namespace EECIV.Entities.Sensor
{
    //Sensor de temperatura do liquido de arrefecimento
    public class WaterTemperatureSensor : ISensor
    {
        public string Name { get; set; }
        public SensorType Type => SensorType.WaterTemperature;
        public float ECUValue { get; set; }

        public object ECUValueToSensorValue()
        {
            if (ECUValue >= 3.35)
                return 15;
            else if (ECUValue < 3.35 && ECUValue >= 3.10)
                return 20;
            else if (ECUValue < 3.10 && ECUValue >= 2.85)
                return 25;
            else if (ECUValue < 2.85 && ECUValue >= 2.60)
                return 30;
            else if (ECUValue < 2.60 && ECUValue >= 2.35)
                return 35;
            else if (ECUValue < 2.35 && ECUValue >= 2.10)
                return 40;
            else if (ECUValue < 2.10 && ECUValue >= 1.90)
                return 45;
            else if (ECUValue < 1.90 && ECUValue >= 1.70)
                return 50;
            else if (ECUValue < 1.70 && ECUValue >= 1.50)
                return 55;
            else if (ECUValue < 1.50 && ECUValue >= 1.30)
                return 60;
            else if (ECUValue < 1.30 && ECUValue >= 1.18)
                return 65;
            else if (ECUValue < 1.18 && ECUValue >= 1.05)
                return 70;
            else if (ECUValue < 1.05 && ECUValue >= 0.93)
                return 75;
            else if (ECUValue < 0.93 && ECUValue >= 0.80)
                return 80;
            else if (ECUValue < 0.80 && ECUValue >= 0.70)
                return 85;
            else if (ECUValue < 0.70 && ECUValue >= 0.60)
                return 90;
            else if (ECUValue < 0.60 && ECUValue >= 0.55)
                return 95;
            else if (ECUValue < 0.55 && ECUValue >= 0.45)
                return 100;
            else
                return 0;

        }
    }
}
