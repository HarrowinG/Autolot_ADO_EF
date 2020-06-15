﻿using System;
using System.ServiceModel;
using MagicEightBallServiceLib;

namespace MagicEightBallServiceHost
{
    public class Program
    {
        //0. Any WCF host run need Administrator privilege
        //1. create endpoint in config
        //2. create ServiceHost in IIS created automatically
        //3. keep host running)
        static void Main(string[] args)
        {
            Console.WriteLine("***** Console Based WCF Host *****");
            using (var serviceHost = new ServiceHost(typeof(MagicEightBallService)))
            {
                serviceHost.Open();
                DisplayHostInfo(serviceHost);

                Console.WriteLine("Service is ready");
                Console.WriteLine("Press the Enter key to terminate service.");
                Console.ReadLine();
            }
        }

        static void DisplayHostInfo(ServiceHost host)
        {
            Console.WriteLine();
            Console.WriteLine("*** Host Info ***");

            foreach (var se in host.Description.Endpoints)
            {
                Console.WriteLine($"Address: {se.Address}");
                Console.WriteLine($"Binding: {se.Binding.Name}");
                Console.WriteLine($"Contract: {se.Contract.Name}");
                Console.WriteLine();
            }
            Console.WriteLine("*******");
        }
    }
}
