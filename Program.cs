using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace DynuUpdate2
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationManager.AppSettings;

            var span = 30 * 1000 * 60;
            var my_domain = "";
            var my_password = "";
            try
            {
                my_domain = config["my_domain"];
                if (string.IsNullOrEmpty(my_domain))
                {
                    Log("获取域名出错，结束运行。请配置my_domain项的值。");
                    Console.ReadKey(false);
                    return;
                }
            }
            catch (NullReferenceException)
            {
                Log("获取域名出错，结束运行。请配置my_domain项的值。");
                Console.ReadKey(false);
                return;
            }
            try
            {
                my_password = config["my_password"];
                if (string.IsNullOrEmpty(my_password))
                {
                    Log("获取密码出错，结束运行。");
                    Console.ReadKey(false);
                    return;
                }
            }
            catch (NullReferenceException)
            {
                
                Log("获取密码出错，结束运行。");
                Console.ReadKey(false);
                return;
            }
            try
            {
                span = int.Parse(config["time_span"].ToString()) * 1000 * 60;
            }
            catch (NullReferenceException)
            {
                Log("获取time_span设置错误，更新时间将被设置为30分钟。");
            }
            try
            {
                span = int.Parse(config["time_span"].ToString()) * 1000 * 60;
            }
            catch (NullReferenceException)
            {
                Log("获取time_span设置错误，更新时间将被设置为30分钟。");
            }
            while (true)
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    try
                    {
                        string ip_json = client.DownloadString("https://ipinfo.io/");
                        Log(ip_json);
                        JObject ip_json_obj = JObject.Parse(ip_json);
                        string ip = ip_json_obj["ip"].ToString();
                        string password_sha256 = SHA256(my_password);
                        string update_url = $"https://api.dynu.com/nic/update?hostname={my_domain}&myip={ip}&password={password_sha256} ";
                        var update_response = client.DownloadString(update_url);
                        Log(update_response);
                    }
                    catch (TimeoutException to)
                    {
                        Log("获取超时。");
                    }
                    catch (Newtonsoft.Json.JsonException)
                    {
                        Log("转换JSON错误。");
                    }
                }
                Thread.Sleep(span);
            }
        }
        /// <summary>
        /// 日志方法
        /// </summary>
        /// <param name="log"></param>
        static void Log(string log)
        {
            DateTime time = DateTime.Now;
            Console.WriteLine($"[{time}]:{log}");
        }
        /// <summary>
        /// SHA256摘要
        /// </summary>
        /// <param name="str"></param>
        static string SHA256(string str)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(str));
                foreach (Byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }
    }
}
