using System;
using MagicEightBallServiceClient.ServiceReference1;

namespace MagicEightBallServiceClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*** Ask the Magic 8 Ball ***");

            using (var ball = new EightBallClient())
            {
                Console.Write("Your question: ");
                string question = Console.ReadLine();
                string answer = ball.ObtainAnswerToQuestion(question);
                Console.WriteLine($"8-Ball says: {answer}");
            }

            Console.ReadLine();
        }
    }
}
