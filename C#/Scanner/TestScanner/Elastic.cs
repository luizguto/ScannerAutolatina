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
            ISensor water = new WaterTemperatureSensor();
            water.ECUValue = 1.60F;
            water.Name = "MTE";

            CollectedData data = new CollectedData(water);
            var result = new ElasticConnection().SendData(data);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void InsertAirTemperatureSensorCollect()
        {
            ISensor air = new AirTemperatureSensor();
            air.ECUValue = 2.80F;
            air.Name = "MTE";

            CollectedData data = new CollectedData(air);

            var result = new ElasticConnection().SendData(data);

            Assert.AreEqual(true, result);
        }
    }
}
