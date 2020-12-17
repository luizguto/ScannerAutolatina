using EECIV.Entities;
using EECIV.Entities.Enum;
using EECIV.Factory;
using EECIV.Inteface;
using EECIV.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace EECIV.Implementation
{
    public class Scanner : IScanner
    {
        private readonly ILogger _logger = null;
        private readonly IDataAccess _dataAccess = null;
        private readonly ISerialConfiguration _serialConfiguration = null;

        private ManualResetEvent _disposing = null;
        private ConcurrentQueue<ArduinoCollect> _processingQueue = null;
        private List<Thread> _executors = null;
        private SerialPort _serialPort = null;


        public Scanner(IDataAccess dataAccess,  ILogger<Scanner> logger, ISerialConfiguration serialConfiguration)
        {
            _serialConfiguration = serialConfiguration;
            _logger = logger;
            _dataAccess = dataAccess;
        }
        

        public void Start()
        {
            _logger.LogInformation("Inicializando scanner...");

            //TODO: Corrigir configurações (Enum)
            _serialPort = new SerialPort(_serialConfiguration.PortName, 
                                         baudRate: _serialConfiguration.BaudRate, 
                                         parity: Parity.None, 
                                         dataBits: _serialConfiguration.DataBits, 
                                         stopBits: StopBits.Two);


            _processingQueue = new ConcurrentQueue<ArduinoCollect>();
            _disposing = new ManualResetEvent(false);
            _executors = new List<Thread>();

            for (int i = 0; i < 2; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(SendData));
                t.Start(i);
                _executors.Add(t);
            }
        }

        private void SendData(object value)
        {
            while (!_disposing.WaitOne(TimeSpan.FromSeconds(1)))
            {
                ArduinoCollect collectedData = new ArduinoCollect();
                if (_processingQueue.TryDequeue(out collectedData))
                {
                    ISensor sensor = SensorFactory.CreateSensor((SensorType)collectedData.SensorType);
                    sensor.ECUValue = (float)(double)collectedData.Value;

                    _dataAccess.Send(new CollectedData(sensor));

                    Console.WriteLine($"Enviando para o Elastic: {sensor.Type.ToString()} - {collectedData.Value}");
                }
            }
        }

        public void Stop()
        {
            if (_disposing != null)
            {
                _disposing.Set();
            }

            if (_executors != null)
            {
                _executors.ForEach(x => x.Join());
            }
        }
    }
}
