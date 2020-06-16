using System.ServiceProcess;

namespace MathWindowsServiceHost
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MathService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
