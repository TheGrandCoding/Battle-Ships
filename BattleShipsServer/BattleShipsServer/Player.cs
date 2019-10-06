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
        public Game GameIn;
        public TcpClient client;
        public string name;

        string sentmessage;
        public void Send(string message)
        {
            try
            {
                sentmessage = $"%{message}`";
                NetworkStream SendDataStream = client.GetStream();
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(sentmessage);
                SendDataStream.Write(msg, 0, msg.Length);
                Program.Log($"Sent to {name} - {message}");
            }
            catch (Exception ex)
            {
                Program.Log("[Error]"+ex.ToString());
            }
        }
        Game temp;

        public void RecieveData()
        {
            string data = "";
            NetworkStream RecieveDataStream = client.GetStream();
            byte[] bytes = new byte[256];
            int i;
            while (true)
            {
                if ((i = RecieveDataStream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    string DataBunched = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    string[] messages = DataBunched.Split('%').Where(x => string.IsNullOrWhiteSpace(x) == false && x != "%").ToArray();
                    foreach (var msg in messages)
                    {
                        data = msg.Substring(0, msg.IndexOf("`"));
                        Program.Log($"Recieved from {name} - {data}");
                        if (data.StartsWith("UN:"))
                        {
                            var splitlist = data.Split(':');
                            name = splitlist[1];
                        }
                        else if (data.StartsWith("NewGame:"))
                        {
                            var splitlist = data.Split(':');
                            bool uniquename = true;
                            foreach (Game g in Program.CurrentGames)
                            {
                                if (g.Name == splitlist[1])
                                {
                                    uniquename = false;
                                }
                            }
                            if (uniquename == false)
                            {
                                Send("InvalidName");
                            }
                            else
                            {
                                Game newgame = new Game();
                                Program.CurrentGames.Add(newgame);
                                newgame.Name = splitlist[1];
                                GameIn = newgame;
                                newgame.p1 = this;
                                Send("JoinedGame:" + newgame.Name);
                            }
                        }
                        else if (data == "CurrentGames")
                        {
                            foreach (Game g in Program.CurrentGames)
                            {
                                Send("Game:" + g.Name);
                            }
                        }
                        else if (data.StartsWith("JoinGame:"))
                        {
                            var splitlist = data.Split(':');
                            foreach (Game g in Program.CurrentGames)
                            {
                                if (g.Name == splitlist[1])
                                {
                                    temp = g;
                                }
                            }
                            GameIn = temp;
                            temp.p2 = this;
                            Send("JoinedGame:" + temp.Name);
                            temp.StartGame();
                        }
                    }
                }
            }
        }
    }
}
