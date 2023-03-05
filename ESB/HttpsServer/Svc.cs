using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HttpsServer
{
    public class Svc
    {
        public static bool InstallCertificate(string certFilePath, string password, StoreLocation location = StoreLocation.CurrentUser, StoreName storeName = StoreName.Root)
        {
            try
            {
                if (!File.Exists(certFilePath))
                {
                    return false;
                }
                X509Store store = new X509Store(StoreName.Root);
                store.Open(OpenFlags.ReadWrite);
                string SerialNumber = System.Configuration.ConfigurationManager.AppSettings["HttpsCertSerialNumber"];
                if (SerialNumber == null)
                {
                    return false;
                }
                X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindBySerialNumber, SerialNumber, false); // vaildOnly = true时搜索无结果。
                store.Close();
                if (certs.Count == 0)
                {
                    //X509KeyStorageFlags.PersistKeySet 保存私钥
                    X509Certificate2 certificate = new X509Certificate2(File.ReadAllBytes(certFilePath), password, X509KeyStorageFlags.PersistKeySet);
                    X509Store x509Store = new X509Store(storeName, location);
                    x509Store.Open(OpenFlags.MaxAllowed);
                    x509Store.Remove(certificate);
                    x509Store.Add(certificate);
                    x509Store.Close();
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        private TcpListener tcpListener;
        private X509Certificate serverCertificate = null;
        private bool isStarted = false;
        private int port = 433;

        public Svc(int port)
        {
            this.port = port;
        }

        private void FindCerts()
        {
            //证书

            //指定证书存储区
            X509Store store = new X509Store(StoreName.Root);
            //打开存储区
            store.Open(OpenFlags.ReadWrite);
            //读取配置文件中证书的HttpsCertSerialNumber（唯一码）
            string SerialNumber = System.Configuration.ConfigurationManager.AppSettings["HttpsCertSerialNumber"];
            if (SerialNumber == null)
            {
                Console.WriteLine("Failed to read SerialNumber.");
                return;
            }
            // 检索证书 
            X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindBySerialNumber, SerialNumber, false); // vaildOnly = true时搜索无结果。
            if (certs.Count == 0)
            {
                isStarted = false;
                Console.WriteLine("Failed to read certificate.");
                return;
            }
            //关闭存储区
            store.Close();
            Console.WriteLine("Read the certificate successfully.");
            serverCertificate = certs[0];

        }

        private void ProcessClient(TcpClient client)
        {
            string ip = (client.Client.RemoteEndPoint as IPEndPoint).Address.ToString();
            //打印client IP
            Console.WriteLine($"client IP={ip}");
            SslStream sslStream = new SslStream(client.GetStream(), false);
            //serverCertificate:服务证书，需要带私钥
            //clientCertificateRequired：客户端证书是否必填（默认 false）
            //enabledSslProtocols：SSL 协议（默认DefaultSslProtocols）
            //checkCertificateRevocation：检查证书吊销（默认 false）
            //sslStream.AuthenticateAsServer(serverCertificate, false, SslProtocols.Tls11, true);
            sslStream.AuthenticateAsServer(serverCertificate);
            
            sslStream.ReadTimeout = 2000;
            sslStream.WriteTimeout = 2000;
            string content = ReadMessage(sslStream);
            //解析 content

            sslStream.Close();
            client.Close();
        }
        private string ReadMessage(SslStream sslStream)
        {
            byte[] buffer = new byte[1024 * 1024];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;
            try
            {
                do
                {
                    Console.WriteLine($"buffer.length:{buffer.Length}");
                    bytes = sslStream.Read(buffer, 0, buffer.Length);
                    Decoder decoder = Encoding.UTF8.GetDecoder();
                    char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                    decoder.GetChars(buffer, 0, bytes, chars, 0);
                    messageData.Append(chars);
                    if (messageData.ToString().IndexOf("") != -1)
                    {
                        break;
                    }
                }
                while (bytes != 0);
            }
            catch (Exception e)
            {
                return null;
            }
            return messageData.ToString();
        }

        public void Start()
        {
            Task task = Task.Factory.StartNew(() =>
            {
                FindCerts();
                tcpListener = new TcpListener(IPAddress.Any, port);
                tcpListener.Start();
                Console.WriteLine("this server is ready to wait a client.");
                isStarted = true;
                while (isStarted)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    //多线程创建客户端，防止因前一API延时设置等因素影响其他通信
                    Task.Factory.StartNew(() => ProcessClient(tcpClient));
                }
                tcpListener.Stop();
            });
        }
    }
}
