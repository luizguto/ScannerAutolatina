using EECIV.Entities.Enum;
using EECIV.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace EECIV.Entities
{
    public class CollectedData
    {
        public DateTime DateTime { get; set; }

        public SensorType SensorType { get; set; }

        public object Value { get; set; }

    }
}
