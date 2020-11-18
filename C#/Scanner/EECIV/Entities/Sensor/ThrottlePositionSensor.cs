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
            throw new NotImplementedException();
        }
    }
}
