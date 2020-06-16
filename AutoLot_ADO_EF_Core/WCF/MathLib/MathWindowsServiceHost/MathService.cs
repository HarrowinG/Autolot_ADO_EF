using System;
using System.ServiceModel;
using System.ServiceProcess;

namespace MathWindowsServiceHost
{
    public partial class MathService : ServiceBase
    {
        private ServiceHost _myHost;

        public MathService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _myHost?.Close();
//            _myHost = new ServiceHost(typeof(MathService));
//            var address = new Uri("http://localhost:8080/MathServiceLibrary");
//            var binding = new WSHttpBinding();
//            var contract = typeof(IBasicMath);
//            _myHost.AddServiceEndpoint(contract, binding, address);

            var address = new Uri("http://localhost:8080/MathServiceLibrary");
            _myHost = new ServiceHost(typeof(MathService), address);
            _myHost.AddDefaultEndpoints();
            _myHost.Open();
        }

        protected override void OnStop()
        {
            _myHost?.Close();
        }
    }
}
