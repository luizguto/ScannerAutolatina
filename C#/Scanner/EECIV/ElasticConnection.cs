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
        }


        public bool SendData(CollectedData data)
        {
            var settings = new ConnectionSettings(new Uri("http://192.168.0.223:9200")).DefaultIndex("logus");

            var client = new ElasticClient(settings);

            IndexResponse indexResponse = client.IndexDocument(data);

            return indexResponse.IsValid;
        }
    }
}
