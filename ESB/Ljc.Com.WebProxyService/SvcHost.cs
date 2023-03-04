using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Ljc.Com.WebProxyService
{
    public partial class SvcHost : ServiceBase
    {
        private WebProxySvc proxySvc = null;
        private System.Timers.Timer timer = null;
        public SvcHost()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            proxySvc = new WebProxySvc();
            proxySvc.StartService();

            timer = LJC.FrameWork.Comm.TaskHelper.SetInterval(1000 * 180, () =>
            {
                try
                {
                    var config = LJC.FrameWork.SOA.ServiceConfig.ReadConfig();
                    foreach (var item in config.WebMappers)
                    {
                        try
                        {
                            var client = LJC.FrameWork.Comm.HttpClientFactory.GetHttpClient(item.TragetWebHost);
                            _ = client.GetAsync(item.TragetWebHost).Result;

                            LJC.FrameWork.LogManager.LogHelper.Instance.Debug("激活网站成功:"+item.TragetWebHost);
                        }
                        catch (Exception ex)
                        {
                            ex.Data.Add("url", item.TragetWebHost);
                            LJC.FrameWork.LogManager.LogHelper.Instance.Error("激活网站失败", ex);
                        }
                    }
                }
                catch
                {

                }
                return false;
            }, runintime: false);
        }

        protected override void OnStop()
        {
            if (proxySvc != null)
            {
                proxySvc.UnRegisterService();
                proxySvc.Dispose();
            }

            if (timer != null)
            {
                timer.Dispose();
            }
        }
    }
}
