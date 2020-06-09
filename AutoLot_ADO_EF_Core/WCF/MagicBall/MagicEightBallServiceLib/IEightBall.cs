using System.ServiceModel;

namespace MagicEightBallServiceLib
{
    [ServiceContract]
    public interface IEightBall
    {
        [OperationContract]
        string ObtainAnswerToQuestion(string userQuestion);
    }
}
