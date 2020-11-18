using EECIV.Entities.Enum;
using EECIV.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace EECIV.Entities.Sensor
{
    //Sensor Hall
    public class HallSensor : ISensor
    {
        public string Name { get; set; }

        public SensorType Type => SensorType.Hall;

        public float ECUValue { get; set; }

        public object ECUValueToSensorValue()
        {
            throw new NotImplementedException();
        }
    }
}
