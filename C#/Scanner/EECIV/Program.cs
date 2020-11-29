using EECIV.Entities;
using EECIV.Entities.Enum;
using EECIV.Factory;
using EECIV.Inteface;
using EECIV.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace EECIV
{
    class Program
    {
        public static string Retorno = String.Empty;
        private static ManualResetEvent _disposing = null;
        private static Thread _threadColeta = null;
        private static ConcurrentQueue<ArduinoCollect> _filaProcessamento = null;
        private static List<Thread> _executores = null;
        private static void Start()
        {
            _filaProcessamento = new ConcurrentQueue<ArduinoCollect>();
            _disposing = new ManualResetEvent(false);
            _executores = new List<Thread>();

            //_threadColeta = new Thread(() => {

            //    while (!_disposing.WaitOne(3000))
            //    {

            //    }

            //});

            for (int i = 0; i < 2; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(SendToElastic));
                t.Start(i);
                _executores.Add(t);
            }
        }

        private static void Stop()
        {
            if (_disposing != null)
            {
                _disposing.Set();
            }

            if (_threadColeta != null)
            {
                _threadColeta.Join();
            }

            if (_executores != null)
            {
                _executores.ForEach(x => x.Join());
            }
        }

        private static void SendToElastic(object value)
        {
            while (!_disposing.WaitOne(TimeSpan.FromSeconds(1)))
            {
                ArduinoCollect collectedData = new ArduinoCollect();
                if (_filaProcessamento.TryDequeue(out collectedData))
                {
                    ISensor sensor = SensorFactory.CreateSensor((SensorType)collectedData.SensorType);
                    sensor.ECUValue = (float)(double)collectedData.Value;
                    
                    IDataAccess connection = DataAccessFactory.Create();
                    connection.Send(new CollectedData(sensor));

                    Console.WriteLine($"Enviando para o Elastic: {sensor.Type.ToString()} - {collectedData.Value}");
                }
            }
        }

        static void Main(string[] args)
        {
            string port = "COM3";
            Console.WriteLine("Iniciando comunicação com o Arduino...");
            Console.WriteLine($"Conectando na porta: { port }");
            SerialPort serialPort = new SerialPort(port, baudRate: 9800, parity: Parity.None, dataBits: 8, stopBits: StopBits.Two);

            try
            {

                Console.WriteLine("Registrando Handler de recebimento de dados");
                serialPort.DataReceived += SerialPort_DataReceived;

                Start();

                Console.WriteLine("Abrindo conexão");
                serialPort.Open();
            }
            catch (Exception)
            {
                Console.WriteLine("Erro ao conectar ao Arduino");
                throw;
            }

            //do
            //{
            //    Thread.Sleep(1000);
            //} while (string.IsNullOrEmpty(Console.ReadLine()));

            Console.ReadLine();
            Stop();
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
            string dataReceived = sp.ReadLine();
            Console.WriteLine("Recebendo valor: " + dataReceived);
            ArduinoCollect collectedData = new ArduinoCollect();

            try
            {
                collectedData = Newtonsoft.Json.JsonConvert.DeserializeObject<ArduinoCollect>(dataReceived);

                if (collectedData.SensorType != 0)
                {
                    //ISensor sensor = SensorFactory.CreateSensor((SensorType)collectedData.SensorType);
                    //ElasticConnection connection = new ElasticConnection();
                    ////connection.SendData(new CollectedData(sensor));

                    //Console.WriteLine($"{sensor.Type.ToString()} - {collectedData.Value}");

                    _filaProcessamento.Enqueue(collectedData);
                }
                
            }
            catch
            {
                Console.WriteLine($"Não foi possível converter o dado recebido. { dataReceived }");
            }

            
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
