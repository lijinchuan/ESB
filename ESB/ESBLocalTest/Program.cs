using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESBLocalTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //LJC.Com.LogService.Contract.NetLogHelper.SendLog(new LJC.Com.LogService.Contract.LogInfo
            //{
            //    Info = "服务启动",
            //    Level = LJC.Com.LogService.Contract.LogLevel.Info,
            //    LogFrom = "ESB",
            //    LogTime = DateTime.Now,
            //    LogTitle = "esb3服务启动",
            //    LogType = LJC.Com.LogService.Contract.LogType.Service,
            //    StackTrace = ""
            //});

            int port = int.Parse(LJC.FrameWork.Comm.ConfigHelper.AppConfig("ServerPort"));
            LJC.FrameWork.SOA.ESBServer esb = new LJC.FrameWork.SOA.ESBServer(port);
            esb.Error += new Action<Exception>(esb_Error);
            esb.StartServer();
            Console.Read();
        }

        private static void esb_Error(Exception obj)
        {
            LJC.FrameWork.LogManager.LogHelper.Instance.Error("esb error", obj);
            //LJC.FrameWork.LogManager.Logger.TextLog("出错了", obj, LJC.FrameWork.LogManager.LogCategory.Other);

            
        }
    }
}
