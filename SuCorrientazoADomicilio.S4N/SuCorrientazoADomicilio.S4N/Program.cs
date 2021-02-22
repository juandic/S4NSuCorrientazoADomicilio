using Microsoft.Extensions.DependencyInjection;
using S4N.SuCorrientazoADomicilio.Business.Services;
using S4N.SuCorrientazoADomicilio.Business.Services.Interfaces;
using S4N.SuCorrientazoADomicilio.Dto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using S4N.SuCorrientazoADomicilio.Dto.Helper;

namespace S4N.SuCorrientazoADomicilio
{
    class Program
    {
        private static int _maxCoverage;
        private static int _maxDeliveries;
        private static int _dronesQuantity;
        private static string _directoryPath;
        private static CoordinatesDto _initialCoordinates;
        private static IDelivery _deliverySrv;
        private static IFileManager _fileManagerSrv;
        static void Main(string[] args)
        {
            //Generate service collection
            IServiceCollection serviceCollection = new ServiceCollection();
            //Register Business Class
            serviceCollection.AddSingleton<IDelivery, Delivery>();
            serviceCollection.AddSingleton<IFileManager, FileManager>();
            Injector.GenerateProvider(serviceCollection);
            //Get IOC class
            var deliverySrv = Injector.GetService<IDelivery>();
            var fileManagerSrv = Injector.GetService<IFileManager>();

            _deliverySrv = deliverySrv;
            _fileManagerSrv = fileManagerSrv;

            //Initialize parameters
            InitializeParameters();

            //Execute drone deliveries
            ExecuteDrone();

            //var threads = new List<Thread>();

            //for (var i = 1; i <= _dronesQuantity; i++)
            //{
            //    var msg = new RunDelivery(i);
            //    var thread = new Thread(msg.ExecuteDrone);
            //    threads.Add(thread);
            //}

            //foreach (var thread in threads)
            //{
            //    thread.Start();
            //}
        }

        #region Private methods
        private static void ExecuteDrone()
        {
            for (var number = 1; number <= _dronesQuantity; number++)
            {
                var droneNumber = number.ToString("00");
                try
                {
                    //ReadFile
                    var deliveries = _fileManagerSrv.ReadFile($"in{droneNumber}.txt", _directoryPath);

                    //Start delivery
                    var output = _deliverySrv.StartDelivery(deliveries, _initialCoordinates, _maxCoverage, _maxDeliveries);

                    //Write output file results
                    _fileManagerSrv.WriteFile($"0ut{droneNumber}.txt", output, _directoryPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error executing drone {droneNumber} delivery. Error: {e.Message}");
                }
            }
        }

        private static void InitializeParameters()
        {
            var maxCoverage = SettingsHelper.SettingsKeyToInt("MaxCoverage");
            var maxDeliveries = SettingsHelper.SettingsKeyToInt("MaxDeliveriesByDrone");
            var dronesQuantity = SettingsHelper.SettingsKeyToInt("DronesQuantity");

            _directoryPath = SettingsHelper.SettingsKeyToString("FilesDirectoryPath");

            //Set default values
            _dronesQuantity = dronesQuantity > 0 ? dronesQuantity : 1;
            _maxCoverage = maxCoverage > 0 ? maxCoverage : 10;
            _maxDeliveries = maxDeliveries > 0 ? maxDeliveries : 3;
            

            _initialCoordinates = new CoordinatesDto { CardinalDirection = Orientation.Norte, X = 0, Y = 0 };
        }

        private static string GetSettingsValue(string appSetting)
        {
            if (!string.IsNullOrEmpty(appSetting) && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[appSetting]))
            {
                return ConfigurationManager.AppSettings[appSetting];
            }

            return string.Empty;
        }

        private static int GetIntSettingsValue(string appSetting)
        {
            if (!string.IsNullOrEmpty(appSetting) && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[appSetting]))
            {
                int.TryParse(ConfigurationManager.AppSettings[appSetting], out var value);
                return value;
            }

            return 0;
        }
        #endregion
    }
}
