using System;
using System.Collections.Generic;
using System.Text;

namespace EECIV.Inteface
{
    public interface IElasticsearchConfiguration
    {

        string ServerUri { get; set; }

        string DefaultIndex { get; set; }

    }
}
