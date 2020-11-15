using EECIV.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace EECIV
{
    public class ElasticConnection
    {

        public ElasticConnection()
        {

            var settings = new ConnectionSettings(new Uri("http://192.168.0.223:9200")).DefaultIndex("logus");

            var client = new ElasticClient(settings);

            var water = new WaterTemperatureSensor
            {
                Name = "MTE Numero",
                ECUValue = 2.91F
            };

            CollectedData collected = new CollectedData();

            collected.Sensor = water;
            collected.Value = water.ECUValueToSensorValue();
            collected.DateTime = DateTime.Now;

            var indexResponse = client.IndexDocument(collected);
        }

    }
}
