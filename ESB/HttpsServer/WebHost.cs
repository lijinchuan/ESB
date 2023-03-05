using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HttpsServer
{
    internal class WebHost
    {
        public static string certFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cer/client.pfx");
        public static string certPassword = "ljc@123456789";
        public static bool InstallCertificate(string certFilePath, string password, StoreLocation location = StoreLocation.CurrentUser, StoreName storeName = StoreName.Root)
        {
            try
            {
                if (!File.Exists(certFilePath))
                {
                    return false;
                }
                X509Store store = new X509Store(storeName);
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

        static WebHost()
        {
            InstallCertificate(certFilePath, certPassword,StoreLocation.LocalMachine,StoreName.AuthRoot);
        }


        internal async Task Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 443);
            listener.Start();

            Console.WriteLine($"443 is open");

            X509Certificate2 certificate = new X509Certificate2(certFilePath, certPassword);

            Console.WriteLine("certificate is ready");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();

                SslStream sslStream = new SslStream(client.GetStream(), false);
                try
                {
                    await sslStream.AuthenticateAsServerAsync(certificate, false, SslProtocols.Tls12, true);
                }
                catch (AuthenticationException ex)
                {
                    Console.WriteLine($"SSL authentication failed: {ex.Message}");
                    continue;
                }

                // Handle HTTPS request
                HttpRequest httpRequest = await ParseHttpRequest(sslStream);
                if (httpRequest != null)
                {
                    if (httpRequest.Method == "POST")
                    {
                        // Handle POST request
                        Console.WriteLine($"Received POST request with data: {httpRequest.Body}");
                    }
                    else
                    {
                        // Handle other HTTP methods
                        Console.WriteLine($"Received HTTP {httpRequest.Method} request");
                    }

                    // Send response
                    string response = "HTTP/1.1 200 OK\r\nContent-Type: text/html\r\n\r\n<html><body>Hello World!</body></html>";
                    byte[] buffer = Encoding.UTF8.GetBytes(response);
                    await sslStream.WriteAsync(buffer, 0, buffer.Length);
                }

                sslStream.Close();
                client.Close();
            }
        }

        async Task<HttpRequest> ParseHttpRequest(SslStream sslStream)
        {
            byte[] buffer = new byte[1024];
            StringBuilder requestBuilder = new StringBuilder();
            HttpRequest httpRequest = null;

            do
            {
                var bytesRead = await sslStream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                {
                    break;
                }
                requestBuilder.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));

                if (requestBuilder.ToString().IndexOf("\r\n\r\n") >= 0)
                {
                    // End of HTTP headers reached
                    string[] requestParts = requestBuilder.ToString().Split(new[] { "\r\n\r\n" }, StringSplitOptions.None);
                    string[] headerLines = requestParts[0].Split(new[] { "\r\n" }, StringSplitOptions.None);
                    string[] requestLineParts = headerLines[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    httpRequest = new HttpRequest
                    {
                        Method = requestLineParts[0],
                        Url = requestLineParts[1],
                        Headers = new WebHeaderCollection(),
                        Body = requestParts[1]
                    };

                    // Parse HTTP headers
                    for (int i = 1; i < headerLines.Length; i++)
                    {
                        string[] headerParts = headerLines[i].Split(new[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
                        if (headerParts.Length == 2)
                        {
                            httpRequest.Headers.Add(headerParts[0], headerParts[1].Trim());
                        }
                    }
                }

                break;
            } while (true);

            return httpRequest;
        }
    }

    public class HttpRequest
    {
        public string Method { get; set; }
        public string Url { get; set; }
        public WebHeaderCollection Headers
        {
            get; set;
        }

        public string Body
        {
            get;
            set;
        }
    }
}
