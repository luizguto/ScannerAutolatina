using EECIV.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace EECIV.Entities
{
    public class CollectedData
    {
        public DateTime DateTime { get; set; }

        public ISensor Sensor { get; set; }

        public object Value { get; set; }

    }
}
