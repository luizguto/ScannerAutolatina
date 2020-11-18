using EECIV.Entities.Enum;
using EECIV.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace EECIV.Entities.Sensor
{
    //Sensor posição da borboleta
    public class ThrottlePositionSensor : ISensor
    {
        public string Name { get; set; }

        public SensorType Type => SensorType.ThrottlePosition;

        public float ECUValue { get; set; }

        public object ECUValueToSensorValue()
        {
            if (ECUValue <= 0.60)
                return 0;
            else if (ECUValue > 0.60 && ECUValue <= 1.58)
                return 21;
            else if (ECUValue > 1.58 && ECUValue <= 2.57)
                return 42;
            else if (ECUValue > 2.57 && ECUValue <= 3.55)
                return 63;
            else if (ECUValue > 3.55 && ECUValue <= 4.54)
                return 84;
            else
                return 100;
        }
    }
}
