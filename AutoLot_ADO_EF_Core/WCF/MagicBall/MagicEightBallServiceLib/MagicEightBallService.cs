using System;

namespace MagicEightBallServiceLib
{
    public class MagicEightBallService : IEightBall
    {
        public MagicEightBallService()
        {
            Console.WriteLine("The 8-ball awaits your question...");
        }

        public string ObtainAnswerToQuestion(string userQuestion)
        {
            string[] answers = { "Future Uncertain", "Yes", "No", "Hazy", "Ask again later", "Definitely" };

            Random r = new Random();
            return answers[r.Next(answers.Length)];
        }
    }
}
