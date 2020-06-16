using System;
using System.Threading;
using MathClient.ServiceReference1;

namespace MathClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            //install windows service before use it with developer cmd installutil
            Console.WriteLine("*** The Async Math Client ***");
            using (var proxy = new BasicMathClient())
            {
                proxy.Open();
                IAsyncResult result = proxy.BeginAdd(2, 3, ar => Console.WriteLine("2 + 3 = {0}", proxy.EndAdd(ar)), null);

                while (!result.IsCompleted)
                {
                    Thread.Sleep(200);
                    Console.WriteLine("Client is working...");
                }
            }

            Console.ReadLine();
        }
    }
}
