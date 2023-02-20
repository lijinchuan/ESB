using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESBLocalServiceTest
{
    public class TestESBEervice : LJC.FrameWork.SOA.ESBService
    {
        public TestESBEervice()
            : base(101, true, false)
        {
        }

        public override object DoResponse(int funcId, byte[] Param, string clientid)
        {
            //var str = LJC.FrameWork.EntityBuf.EntityBufCore.DeSerialize<string>(Param);
            //Console.WriteLine("收到消息:" + str);
            //return funcId + ":" + str;

            return base.DoResponse(funcId, Param, clientid);
        }
    }
}
