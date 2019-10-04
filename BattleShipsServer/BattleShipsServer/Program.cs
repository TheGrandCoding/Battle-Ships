﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Net;
using System.Net.Sockets;

using System.IO;
using System.Reflection;
using Microsoft.Win32;
using System.Security.Cryptography;

namespace BattleShipsServer
{
    class Program
    {
        public static TcpListener Server;
        public static IPAddress ip;
        public static string LogName;
        static void Main(string[] args)
        {
            MakeLog();
            Log("Start");
            ip = Getipadress();
            Console.WriteLine(ip.ToString());
            Start();
            Console.ReadLine();
        }
        static void Start()
        {
            Server = new TcpListener(IPAddress.Loopback,777);
            Server.Start();
            while (true)
            {
                TcpClient newcon = Server.AcceptTcpClient();
                Console.WriteLine("Accepted V4 connection from " + ((IPEndPoint)newcon.Client.RemoteEndPoint).Address.ToString());
                Thread nu = new Thread(() => NewUser(newcon));
                nu.Start();
            }
        }
        static void NewUser(TcpClient c)
        {
            Player p = new Player();
            p.client = c;
            Thread RD = new Thread(() => RecieveData(p));
            RD.Start();
        }
        static void RecieveData(Player p)
        {
            TcpClient clt = p.client;
            string data = "";
            NetworkStream RecieveDataStream = clt.GetStream();
            byte[] bytes = new byte[256];
            int i;
            while (true)
            {
                if ((i = RecieveDataStream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine(data);
                    Log("hi");
                    // recieve username and game they want to join
                }
            }
        }
        static IPAddress Getipadress()
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
        static void Log(string message)
        {
            StreamWriter swAppend = File.AppendText(LogName);
            swAppend.WriteLine("["+DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString()+"]"+" - "+message);
            swAppend.Close();
        }
        static void MakeLog()
        {
            if (!Directory.Exists("Logs/"))
            {
                Directory.CreateDirectory("Logs");
            }
            LogName = $"logs/" + DateTime.Today.Day.ToString() + " " + DateTime.Today.Month.ToString() + " & " + DateTime.Now.Hour.ToString() + ";" + DateTime.Now.Minute.ToString() +".txt";
            StreamWriter swNew = File.CreateText(LogName);
            swNew.Close();
        }
    }
}

