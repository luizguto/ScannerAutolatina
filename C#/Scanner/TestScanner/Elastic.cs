using EECIV;
using EECIV.Entities;
using EECIV.Entities.Sensor;
using EECIV.Interface;
using NUnit.Framework;

namespace TestScanner
{
    public class Elastic
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void InsertWaterTemperatureSensorCollect()
        {
            CollectedData data = new CollectedData();

            ISensor water = new WaterTemperatureSensor();

            water.ECUValue = 1.60F;
            water.Name = "MTE";

            data.SensorType = water.Type;
            data.Value = water.ECUValueToSensorValue();
            data.DateTime = System.DateTime.Now;

            var result = new ElasticConnection().SendData(data);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void InsertAirTemperatureSensorCollect()
        {
            CollectedData data = new CollectedData();

            ISensor air = new AirTemperatureSensor();

            air.ECUValue = 2.80F;
            air.Name = "MTE";

            data.SensorType = air.Type;
            data.Value = air.ECUValueToSensorValue();
            data.DateTime = System.DateTime.Now;

            var result = new ElasticConnection().SendData(data);

            Assert.AreEqual(true, result);
        }
    }
}
