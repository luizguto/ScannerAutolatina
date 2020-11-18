using EECIV.Entities.Enum;
using EECIV.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace EECIV.Entities.Sensor
{
    //Sonda lambda
    public class HEGOSensor : ISensor
    {
        public string Name { get; set; }

        public SensorType Type => SensorType.HEGO;

        public float ECUValue { get; set; }

        public object ECUValueToSensorValue()
        {
            throw new NotImplementedException();
        }
    }
}
