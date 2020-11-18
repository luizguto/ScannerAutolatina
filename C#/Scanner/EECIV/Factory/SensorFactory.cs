using EECIV.Entities.Enum;
using EECIV.Entities.Sensor;
using EECIV.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace EECIV.Factory
{
    public class SensorFactory
    {
        //Cria a instancia do sensor de acordo com o tipo
        public static ISensor CreateSensor(SensorType sensorType)
        {

            switch (sensorType)
            {
                case SensorType.AirTemperature:
                    return new AirTemperatureSensor();
                case SensorType.WaterTemperature:
                    return new WaterTemperatureSensor();
                case SensorType.Hall:
                    return new HallSensor();
                case SensorType.HEGO:
                    return new HEGOSensor();
                case SensorType.ManifoldAbsolutePressure:
                    return new ManifoldAbsolutePressureSensor();
                case SensorType.PSPS:
                    return new PSPSSensor();
                case SensorType.ThrottlePosition:
                    return new ThrottlePositionSensor();
            }

            return null;
        }

    }
}
