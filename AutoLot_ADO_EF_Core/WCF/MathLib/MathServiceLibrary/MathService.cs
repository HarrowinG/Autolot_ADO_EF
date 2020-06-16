using System.Threading;

namespace MathServiceLibrary
{
    public class MathService : IBasicMath
    {
        public int Add(int x, int y)
        {
            Thread.Sleep(5000);
            return x + y;
        }
    }
}
