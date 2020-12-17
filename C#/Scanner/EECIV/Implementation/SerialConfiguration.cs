using EECIV.Inteface;
using System;
using System.Collections.Generic;
using System.Text;

namespace EECIV.Implementation
{
    public class SerialConfiguration : ISerialConfiguration
    {
        public string PortName { get; set; }

        public int BaudRate { get; set; }

        public string Parity { get; set; }

        public int DataBits { get; set; }

        public string StopBits { get; set; }

    }
}
