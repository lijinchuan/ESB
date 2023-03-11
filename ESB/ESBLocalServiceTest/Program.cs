using LJC.FrameWork.SOA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ESBLocalServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //LJC.FrameWork.SOA.ServiceConfig.WriteConfig(new ServiceConfig(),
            //    new List<LJC.FrameWork.SOA.WebMapper>
            //{
            //    new LJC.FrameWork.SOA.WebMapper
            //    {
            //        MappingPort=8082,
            //        MappingRoot=string.Empty,
            //        TragetWebHost="http://127.0.0.1:83",
            //        UseProxyName="7890"
            //    }
            //},new List<LJC.FrameWork.SOA.WebProxy>
            //{
            //    new LJC.FrameWork.SOA.WebProxy
            //    {
            //        Address="127.0.0.1:7890",
            //        Name="7890",
            //        UserName="",
            //        UserPassWord=""
            //    }
            //});

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12| SecurityProtocolType.Ssl3;

            TestESBEervice service = new TestESBEervice();
            service.LoginSuccess += new Action(() =>
            {
                service.RegisterService();
                Console.WriteLine("注册成功");
            });
            service.Error += service_Error;
            service.Login(null, null);

            Console.ReadLine();

            service.UnRegisterService();
            service.Dispose();
        }

        static void service_Error(Exception obj)
        {
            Console.WriteLine(obj.Message);
        }

        
    }
}
