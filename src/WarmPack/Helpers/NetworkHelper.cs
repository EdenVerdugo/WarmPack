using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Extensions;

namespace WarmPack.Helpers
{
    public class NetworkHelper
    {
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static string[] GetLocalIPAdresses()
        {
            List<string> lst = new List<string>();
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    lst.Add(ip.ToString());
                }
            }

            return lst.ToArray();
        }

        public static string GetPublicExtternalIPAddress()
        {
            string ip = string.Empty;
            try
            {
                ip = new WebClient().DownloadString("http://icanhazip.com").Replace("\n","").Replace("\r","");
            }
            catch(Exception ex)
            {
                ex.Log();                
            }
            
            return ip;
        }
    }
}
