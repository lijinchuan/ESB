using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ESB
{
    partial class ESB : ServiceBase
    {
        public ESB()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            //LJC.FrameWork.LogManager.Logger.Init();
            int port = int.Parse(LJC.FrameWork.Comm.ConfigHelper.AppConfig("ServerPort"));
            LJC.FrameWork.SOA.ESBServer esb = new LJC.FrameWork.SOA.ESBServer(port);
            esb.Error += new Action<Exception>(esb_Error);
            esb.StartServer();

            //LJC.Com.LogService.Contract.NetLogHelper.SendLog(new LJC.Com.LogService.Contract.LogInfo
            //{
            //    Info = "服务启动",
            //    Level = LJC.Com.LogService.Contract.LogLevel.Info,
            //    LogFrom = "ESB",
            //    LogTime = DateTime.Now,
            //    LogTitle = "esb服务启动",
            //    LogType = LJC.Com.LogService.Contract.LogType.Service,
            //    StackTrace = ""
            //});
        }

        private void esb_Error(Exception ex)
        {
            LJC.FrameWork.LogManager.LogHelper.Instance.Error("esb报错", ex);
            //LJC.FrameWork.LogManager.Logger.TextLog("出错了", obj, LJC.FrameWork.LogManager.LogCategory.Other);
            //LJC.Com.LogService.Contract.NetLogHelper.SendLog(new LJC.Com.LogService.Contract.LogInfo
            //{
            //    Info=ex.Message,
            //    Level=LJC.Com.LogService.Contract.LogLevel.Error,
            //    LogFrom="ESB",
            //    LogTime=DateTime.Now,
            //    LogTitle="esb出错",
            //    LogType=LJC.Com.LogService.Contract.LogType.Service,
            //    StackTrace=ex.StackTrace
            //});
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。

            //LJC.Com.LogService.Contract.NetLogHelper.SendLog(new LJC.Com.LogService.Contract.LogInfo
            //{
            //    Info = "服务停止",
            //    Level = LJC.Com.LogService.Contract.LogLevel.Info,
            //    LogFrom = "ESB",
            //    LogTime = DateTime.Now,
            //    LogTitle = "esb服务停止",
            //    LogType = LJC.Com.LogService.Contract.LogType.Service,
            //    StackTrace = ""
            //});
        }
    }
}
