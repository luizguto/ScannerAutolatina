using EECIV.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace EECIV.Interface
{
    public interface ISensor
    {

        string Name { get; set; }
        
        SensorType Type { get; }

        float ECUValue { get; set; }

        object ECUValueToSensorValue();

    }
}
