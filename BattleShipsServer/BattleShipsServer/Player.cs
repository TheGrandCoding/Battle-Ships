using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;


namespace BattleShipsServer
{
    class Player
    {
        public TcpClient client;
        public string name;

        public void Send(string message)
        {
            try
            {
                //message = $"%{message}`";
                NetworkStream SendDataStream = client.GetStream();
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
                SendDataStream.Write(msg, 0, msg.Length);
                Program.Log($"Sent to {name} - {message}");
            }
            catch (Exception ex)
            {
                Program.Log("[Error]"+ex.ToString());
            }
        }
    }
}
