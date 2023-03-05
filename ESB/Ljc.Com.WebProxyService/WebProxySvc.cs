using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljc.Com.WebProxyService
{
    internal class WebProxySvc : LJC.FrameWork.SOA.ESBService
    {
        public WebProxySvc()
            : base(101, false, false, "webProxy", System.Net.Dns.GetHostName())
        {
        }

        protected override void OnError(Exception e)
        {
            LJC.FrameWork.LogManager.LogHelper.Instance.Error("服务出错", e);
            base.OnError(e);
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
