using EECIV.Entities.Enum;
using EECIV.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace EECIV.Entities.Sensor
{
    //Sensor MAP
    public class ManifoldAbsolutePressureSensor : ISensor
    {
        public string Name { get; set; }

        public SensorType Type => SensorType.ManifoldAbsolutePressure;

        public float ECUValue { get; set; }

        public object ECUValueToSensorValue()
        {
            throw new NotImplementedException();
        }
    }
}
