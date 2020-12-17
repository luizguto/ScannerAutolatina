using EECIV.Implementation;
using EECIV.Inteface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EECIV.Factory
{
    public class DataAccessFactory
    {

        public static IDataAccess Create(IElasticsearchConfiguration configuration, ILogger logger)
        {
            return new ElasticSearchDataAcess(configuration, logger);
            
        }

    }
}
