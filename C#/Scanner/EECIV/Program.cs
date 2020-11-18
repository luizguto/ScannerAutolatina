using EECIV.Entities;
using EECIV.Entities.Enum;
using EECIV.Factory;
using EECIV.Interface;
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
            string port = "COM3";
            Console.WriteLine("Iniciando comunicação com o Arduino...");
            Console.WriteLine($"Conectando na porta: { port }");
            SerialPort serialPort = new SerialPort("COM3", baudRate: 9800, parity: Parity.None, dataBits: 8, stopBits: StopBits.Two);

            try
            {

                Console.WriteLine("Registrando Handler de recebimento de dados");
                serialPort.DataReceived += SerialPort_DataReceived;

                Console.WriteLine("Abrindo conexão");
                serialPort.Open();
            }
            catch (Exception)
            {
                Console.WriteLine("Erro ao conectar ao Arduino");
                throw;
            }

            do
            {
                Thread.Sleep(1000);
            } while (string.IsNullOrEmpty(Console.ReadLine()));

            Console.WriteLine("Fechando conexão");
            serialPort.Close();
        }

        private static void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            e.ToString();
        }

        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string dataReceived = sp.ReadExisting();

            var collectedData = Newtonsoft.Json.JsonConvert.DeserializeObject<ArduinoCollect>(dataReceived);

            ISensor sensor = SensorFactory.CreateSensor((SensorType)collectedData.SensorType);
            ElasticConnection connection = new ElasticConnection();
            connection.SendData(new CollectedData(sensor));

            Console.Write(dataReceived);
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
