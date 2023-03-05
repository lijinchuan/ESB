using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HttpsServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Svc.InstallCertificate()

            //Svc svc = new Svc(443);

            //svc.Start();

            _ = new WebHost().Start();

            Console.Read();
        }
    }
}
