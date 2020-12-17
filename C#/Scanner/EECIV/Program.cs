using EECIV.Entities;
using EECIV.Entities.Enum;
using EECIV.Factory;
using EECIV.Implementation;
using EECIV.Implementation.Logger;
using EECIV.Inteface;
using EECIV.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace EECIV
{
    class Program
    {
        public static string Retorno = String.Empty;
        private static IConfiguration _configuration = null;

        private static ILoggingBuilder ConfigureLogging(ILoggingBuilder logging)
        {
            return logging.AddProvider(new ColorConsoleLoggerProvider(
                                     new ColorConsoleLoggerConfiguration
                                     {
                                         LogLevel = LogLevel.Error,
                                         Color = ConsoleColor.Red
                                     }))
                             .AddColorConsoleLogger()
                             .AddColorConsoleLogger(configuration =>
                             {
                                 configuration.LogLevel = LogLevel.Warning;
                                 configuration.Color = ConsoleColor.DarkMagenta;
                             });
        }

        private static IConfiguration ConfigureConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();


            return _configuration;
        }

        private static void ConfigureServices(IConfiguration configuration, IServiceCollection serviceCollection)
        {
            //Realiza a leitura das configurações para uso de porta Serial.
            ISerialConfiguration serialConfiguration = new SerialConfiguration();
            configuration.GetSection("SerialConnection").Bind(serialConfiguration);

            IElasticsearchConfiguration elasticConfiguration = new ElasticsearchConfiguration();
            configuration.GetSection("elasticSearch").Bind(elasticConfiguration);

            IDataAccess _dataAccess = DataAccessFactory.Create(elasticConfiguration, null);

            // DI
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddSingleton<ISerialConfiguration>(serialConfiguration);
            serviceCollection.AddSingleton<IElasticsearchConfiguration>(elasticConfiguration);
            serviceCollection.AddSingleton<IDataAccess>(_dataAccess);
            serviceCollection.AddSingleton<IScanner, Scanner>();

            //do the actual work here
            //var serviceProvider = serviceCollection.BuildServiceProvider();
        }

        //static IHostBuilder CreateHostBuilder(string[] args)
        //{
        //    return Host.CreateDefaultBuilder(args)
        //        .ConfigureAppConfiguration(configuration => ConfigureConfiguration())
        //    .ConfigureLogging(builder => ConfigureLogging(builder))
        //    .ConfigureServices(service => ConfigureServices());
        //}

        static void Main(string[] args)
        {
            //IHostBuilder hostBuilder = CreateHostBuilder(args);

            var host = Host.CreateDefaultBuilder()
            .ConfigureLogging(builder =>
                builder.ClearProviders()
                .AddProvider(
                    new ColorConsoleLoggerProvider(
                        new ColorConsoleLoggerConfiguration
                        {
                            LogLevel = LogLevel.Error,
                            Color = ConsoleColor.Red
                        }))
                .AddColorConsoleLogger()
                .AddColorConsoleLogger(configuration =>
                {
                    configuration.LogLevel = LogLevel.Warning;
                    configuration.Color = ConsoleColor.DarkMagenta;
                }))
             .ConfigureAppConfiguration((context, builder) =>
             {
                 // Add other configuration files...
                 builder.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true);

             })
             .ConfigureServices((context, services) =>
             {
                 ConfigureServices(context.Configuration, services);
             })
             .Build();

            //IHost host = hostBuilder.Build();

            ILogger logger = host.Services.GetService<ILogger>();


            IScanner scanner = host.Services.GetService<IScanner>();


            scanner.Start();

            Console.ReadLine();

            scanner.Stop();

            /*string port = "COM3";
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

            Console.ReadLine();
            Stop();
            Console.WriteLine("Fechando conexão");
            serialPort.Close();
            */
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

                    //_filaProcessamento.Enqueue(collectedData);
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
