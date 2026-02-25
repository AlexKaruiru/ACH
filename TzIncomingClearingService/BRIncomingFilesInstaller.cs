using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace TzIncomingClearingService
{
    [RunInstaller(true)]
    public partial class BRIncomingFilesInstaller : Installer
    {
        private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller serviceProcessInstaller;

        public BRIncomingFilesInstaller()
        {
            serviceInstaller = new ServiceInstaller();
            serviceProcessInstaller = new ServiceProcessInstaller();

            // Service Installer
            serviceInstaller.ServiceName = "BRIncomingFilesService";
            serviceInstaller.DisplayName = "BR Incoming Files Service";
            serviceInstaller.Description = "This service is responsible for processing BRCore incoming Clearing files from other banks.";
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.DelayedAutoStart = true;

            // Service Process Installer
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;

            Installers.Add(serviceInstaller);
            Installers.Add(serviceProcessInstaller);
            InitializeComponent();
        }
    }
}
