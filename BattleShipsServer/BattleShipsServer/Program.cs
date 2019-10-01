using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Net;
using System.Net.Sockets;

namespace BattleShipsServer
{
    class Program
    {
        public static TcpListener Server;
        public static IPAddress ip;
        static void Main(string[] args)
        {
            ip = Getipadress();
            Start();
        }
        private static void Start()
        {
            Server = new TcpListener(IPAddress.Loopback,777);
        }
        private static IPAddress Getipadress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;//ipv4
                }
            }
            return null;
        }

    }
}
