using System.ServiceModel;

namespace MathServiceLibrary
{
    [ServiceContract(Namespace = "http://MyCompany.com")]
    public interface IBasicMath
    {
        [OperationContract]
        int Add(int x, int y);
    }
}
