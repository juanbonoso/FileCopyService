using System;
using System.ServiceProcess;

namespace FileCopyService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "--console")
            {
                // Run as a console application
                Console.WriteLine("Running as a console application for debugging...");
                var service = new MyFileCopyService();
                service.Start(); // Call custom start method for debugging

                Console.WriteLine("Press Enter to stop the service...");
                Console.ReadLine(); // Use Console.ReadLine instead of Console.ReadKey

                service.Stop(); // Call custom stop method for debugging
            }
            else
            {
                // Run as a Windows Service
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new MyFileCopyService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
