using LJC.FrameWork.SOA.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //LJC.FrameWork.SOA.ESBConfig.WriteConfig(new LJC.FrameWork.SOA.ESBConfig
            //{
            //    AutoStart=true,
            //    ESBServer="127.0.0.1",
            //    ESBPort=20000,
            //    IsSecurity=false,
            //    MaxClientCount=5,
            //    ESBServerConfigItems=new List<LJC.FrameWork.SOA.ESBServerConfigItem>
            //    {
            //        new LJC.FrameWork.SOA.ESBServerConfigItem
            //        {
            //            AutoStart=true,
            //            ESBPort=20001,
            //            ESBServer="127.0.0.1",
            //            IsSecurity=false,
            //            MaxClientCount=2,
            //        }
            //    }
            //});

            var svcInfo = LJC.FrameWork.SOA.ESBClient.DoSOARequest2<QueryServiceNoResponse>(101, 111, null);

            var svcInfo2 = LJC.FrameWork.SOA.ESBClient.DoSOARequest2<QueryServiceNoResponse>(102, 111, null);

            Console.WriteLine();
        }
    }
}
