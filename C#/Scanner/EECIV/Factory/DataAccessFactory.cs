using EECIV.Implementation;
using EECIV.Inteface;
using System;
using System.Collections.Generic;
using System.Text;

namespace EECIV.Factory
{
    public class DataAccessFactory
    {


        public static IDataAccess Create()
        {

            return new ElasticSearchDataAcess(null, null);
            
        }

    }
}
