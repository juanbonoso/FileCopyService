using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace FileCopyService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            // Create the ServiceProcessInstaller
            var serviceProcessInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem // Run the service under the local system account
            };

            // Create the ServiceInstaller
            var serviceInstaller = new ServiceInstaller
            {
                ServiceName = "FileCopyService",        // The internal name of the service
                DisplayName = "File Copy Service",     // The name shown in the Services console
                Description = "Copies the top 10 files from ServerDrive to FileShare every 5 minutes.",
                StartType = ServiceStartMode.Automatic // Start the service automatically
            };

            // Add installers to the Installers collection
            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
