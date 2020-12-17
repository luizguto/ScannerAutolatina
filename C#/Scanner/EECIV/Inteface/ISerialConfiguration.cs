using System;
using System.Collections.Generic;
using System.Text;

namespace EECIV.Inteface
{
    public interface ISerialConfiguration
    {
        string PortName { get; set; }
        
        int BaudRate { get; set; }

        string Parity { get; set; }
        
        int DataBits { get; set; }

        string StopBits { get; set; }

    }
}
