using EECIV.Entities.Enum;
using EECIV.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace EECIV.Entities
{
    public class CollectedData
    {

        public CollectedData(ISensor sensor)
        {
            this.SensorType = sensor.Type;
            this.Value = sensor.ECUValueToSensorValue();
            this.DateTime = DateTime.Now;
        }

        public DateTime DateTime { get; set; }

        public SensorType SensorType { get; set; }

        public object Value { get; set; }

    }
}
