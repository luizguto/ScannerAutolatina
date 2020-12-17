using EECIV.Inteface;
using Nest;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace EECIV.Implementation
{
    public class ElasticSearchDataAcess : IDataAccess
    {
        private readonly IElasticsearchConfiguration _configuration = null;
        private readonly ILogger _logger = null;

        private ConnectionSettings connectionSettings = null;
        private ElasticClient elasticClient = null;

        private Uri ServerUri
        {
            get
            {
                return new Uri(_configuration.ServerUri);
            }
        }

        public ElasticSearchDataAcess(IElasticsearchConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void Connect()
        {
            try
            {
                connectionSettings = new ConnectionSettings(ServerUri).DefaultIndex(_configuration.DefaultIndex);
                elasticClient = new ElasticClient(connectionSettings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao logar no ElasticSearch");
            }
        }

        public void Disconect()
        {
            elasticClient = null;
            connectionSettings = null;
        }

        public bool Send(object value)
        {
            Connect();

            try
            {
                IndexResponse indexResponse = elasticClient.IndexDocument(value);

                return indexResponse.IsValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar dados para o ElasticSearch");
                return false;
            }
            finally
            {
                Disconect();
            }

        }
    }
}
