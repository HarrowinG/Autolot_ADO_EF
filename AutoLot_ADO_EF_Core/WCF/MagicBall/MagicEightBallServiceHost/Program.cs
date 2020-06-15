using System;
using System.ServiceModel;
using MagicEightBallServiceLib;

namespace MagicEightBallServiceHost
{
    public class Program
    {
        //1. create endpoint in config
        //2. create ServiceHost
        //3. keep host running)
        static void Main(string[] args)
        {
            Console.WriteLine("***** Console Based WCF Host *****");
            using (var serviceHost = new ServiceHost(typeof(MagicEightBallService)))
            {
                serviceHost.Open();

                Console.WriteLine("Service is ready");
                Console.WriteLine("Press the Enter key to terminate service.");
                Console.ReadLine();
            }
        }
    }
}
