using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using EECIV.Factory;
using EECIV.Implementation;
using EECIV.Implementation.Logger;
using EECIV.Inteface;

namespace EECIV
{
    class Program
    {
        public static string Retorno = String.Empty;

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
        }

        static void Main(string[] args)
        {
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
                 builder.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true);
             })
             .ConfigureServices((context, services) =>
             {
                 ConfigureServices(context.Configuration, services);
             })
             .Build();

            IScanner scanner = host.Services.GetService<IScanner>();

            scanner.Start();

            Console.ReadLine();

            scanner.Stop();

        }

    }
}
