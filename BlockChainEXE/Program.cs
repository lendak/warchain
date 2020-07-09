using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace BlockChainEXE
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost svc = new ServiceHost(typeof(ValidatorImpl));
            /*svc.AddServiceEndpoint(typeof(IValidator),
                new NetTcpBinding(),
                new Uri("net.tcp://localhost:4000/IFizickaLica"));*/
            svc.Open();
            Console.WriteLine("Press [Enter] to stop the service.");
            Console.ReadLine();
        }
    }
}
