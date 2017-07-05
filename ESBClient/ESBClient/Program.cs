using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LJC.FrameWork.SOA;
using System.Threading;
using System.Diagnostics;
using LJC.Com.StockService.Contract;
using LJC.Com.SMService.Contract;

namespace ESBClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            for (int i = 0; i < 1000; i++)
            {
                sw.Restart();
                try
                {
                    //var str = LJC.FrameWork.SOA.ESBClient.DoSOARequest<StockRealQuote>(1, 1007, "000002.sz");
                    //var str = LJC.FrameWork.SOA.ESBClient.DoSOARequest<List<StockBaseInfo>>(1, 1003, string.Empty);
                    //var str = LJC.FrameWork.SOA.ESBClient.DoSOARequest<List<StockSimpleInfo>>(1, 1004, string.Empty);
                    //LJC.Com.StockService.Contract.GetHisDayQuoteRequest
                    //var str = LJC.FrameWork.SOA.ESBClient.DoSOARequest<List<StockQuote>>(1, 1005, new GetHisDayQuoteRequest
                    //{
                    //    InnerCode = "600031.sh",
                    //    Lastest = true,
                    //});

                    //var str = LJC.FrameWork.SOA.ESBClient.DoSOARequest<List<StockQuote>>(1, 1006, new GetHisDayQuoteSpanRequest
                    //{
                    //    InnerCode = "600588.sh",
                    //    Start=new DateTime(2015,2,27),
                    //    End=new DateTime(2015,2,27)
                    //});

                    //var str = LJC.FrameWork.SOA.ESBClient.DoSOARequest<StockBaseInfo>(1, 1001, "600031.SH");

                    //是否开盘
                    //var str = LJC.FrameWork.SOA.ESBClient.DoSOARequest<bool>(1, 9000, DateTime.Now);

                    //下一个交易日期
                    //var str = LJC.FrameWork.SOA.ESBClient.DoSOARequest<DateTime>(1, 9001, DateTime.Now);

                    //下一次开盘时间
                    //var str = LJC.FrameWork.SOA.ESBClient.DoSOARequest<DateTime>(1, 9002, DateTime.Now);

                    //上一次收盘时间
                    //var str = LJC.FrameWork.SOA.ESBClient.DoSOARequest<DateTime>(1, 9003, DateTime.Now);

                    //上一个交易日
                    //var str = LJC.FrameWork.SOA.ESBClient.DoSOARequest<DateTime>(1, 9004, DateTime.Now);

                    //某月交易列表
                    //var str = LJC.FrameWork.SOA.ESBClient.DoSOARequest<List<DateTime>>(1, 9005, new GetCurrMonthTradeDayRequest
                    //{
                    //    Year = 2015,
                    //    Mon = 3,
                    //});

                    var boo = LJC.FrameWork.SOA.ESBClient.DoSOARequest<List<SMMsg>>(ServiceConst.ServiceNo,
                        ServiceConst.GetSMsg_FuncId, DateTime.Now.AddDays(-1));

                    //var bo = LJC.FrameWork.SOA.ESBClient.DoSOARequest<bool>(ServiceConst.ServiceNo,
                    //    ServiceConst.SendSM_FuncId, new SendSMRequest
                    //    {
                    //        Msg = "正在看电影",
                    //        Recivers = new string[] { "15375742243" },
                    //    });

                    //var str = LJC.FrameWork.SOA.ESBClient.DoSOARequest<List<LJC.Com.StockService.Contract.StockRealQuote>>(1, 1011, new string[]
                    //{
                    //    "000002.sz",
                    //    "600031.sh"
                    //});

                    //Console.WriteLine(i.ToString() + " " + str);
                    sw.Stop();
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                Console.WriteLine("费时："+sw.ElapsedMilliseconds);
            }
           Console.Read();
        }

        static void client_LoginFail()
        {
            Console.WriteLine("登录失败");
        }

        static void client_Error(Exception obj)
        {
            Console.WriteLine(obj.Message);
        }
    }
}
