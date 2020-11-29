using System;
using System.Collections.Generic;
using System.Text;

namespace EECIV.Inteface
{
    public interface IDataAccess
    {

        void Connect();

        void Disconect();

        bool Send(object value);

    }
}
