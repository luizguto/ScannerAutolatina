using EECIV.Inteface;
using System;
using System.Collections.Generic;
using System.Text;

namespace EECIV.Implementation
{
    public class ElasticsearchConfiguration : IElasticsearchConfiguration
    {
        public string ServerUri { get; set; }

        public string DefaultIndex { get; set; }

    }
}
