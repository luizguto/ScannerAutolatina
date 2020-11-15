using EECIV;
using EECIV.Entities;
using EECIV.Interface;
using NUnit.Framework;

namespace TestScanner
{
    public class SensorTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void WaterTemperatureValue25()
        {
            ISensor water = new WaterTemperatureSensor();

            water.ECUValue = 2.91F;
            int sensor = (int)water.ECUValueToSensorValue();

            Assert.AreEqual(25, sensor);
        }
    }
}
