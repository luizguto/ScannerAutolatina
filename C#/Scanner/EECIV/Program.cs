using System;
using System.IO.Ports;
using System.Threading;

namespace EECIV
{
    class Program
    {
        public static string Retorno = String.Empty;

        static void Main(string[] args)
        {

            ElasticConnection connection = new ElasticConnection();

            /*SerialPort serialPort = new SerialPort("COM3", baudRate: 9800, parity: Parity.None, dataBits: 8, stopBits: StopBits.Two);
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.ErrorReceived += SerialPort_ErrorReceived;
            
            serialPort.Open();

            //Thread.Sleep(1000);

            //WriteByte(7, serialPort);
            //Thread.Sleep(1000);


            //while (string.IsNullOrEmpty(Retorno))
            //{
                //Thread.Sleep(1000);
                //WriteByte(7, serialPort);

            //Thread.Sleep(1000);
            //Console.WriteLine(serialPort.ReadExisting());
            //}

            //WriteByte(4, serialPort);

            //WriteByte(5, serialPort);

            //WriteByte(1, serialPort);


            while (true)
            {
                Thread.Sleep(1000);

            }

            serialPort.Close();
            Console.WriteLine("Hello World!");
            */
        }

        private static void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            e.ToString();
        }

        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            Console.WriteLine("Data Received:");
            Console.Write(indata);
        }

        public static void WriteByte(byte data, SerialPort comPort)
        {
            //change data to array
            //byte[] dataArray = new byte[1];
            var dataArray = new byte[] { data };
            //dataArray[0] = data;
            comPort.Write(dataArray, 0, 1);   // <-- Exception is thrown here
        }
    }
}
