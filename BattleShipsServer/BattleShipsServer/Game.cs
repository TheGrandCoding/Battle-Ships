using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipsServer
{
    class Game
    {
        public string Name;
        public Player p1;
        public Player p2;
        Random rnd = new Random();
        public void StartGame()
        {
            Program.CurrentGames.Remove(this);
            p1.Send("Opp:"+p2.name);
            p2.Send("Opp:"+p1.name);
            p1.GameIn = this; // ?
            p1.Ships.Clear();
            p1.ShipSent = false;
            for (int a = 0; a < 10; a++)
            {
                for (int b = 0; b < 10; b++)
                {
                    p1.Board[a, b] = 'O';
                    p2.Board[a, b] = 'O';
                }
            }
        }
        public void Play()
        {
            if (p1.ShipSent == true && p2.ShipSent == true)
            {
                int Turn = rnd.Next(0, 2);
                if(Turn == 0)
                {
                    p1.Send("Turn");
                    p2.Send("OTurn");
                }
                else
                {
                    p2.Send("Turn");
                    p1.Send("OTurn");
                }
            }
        }
        public void CheckShip(string ShipNum , Player p)
        {
            Player opp;
            if (p == p1)
                opp = p2;
            else
                opp = p1;
            int xpos = int.Parse(ShipNum[0].ToString());
            int ypos = int.Parse(ShipNum[1].ToString());
            char value = opp.Board[xpos,ypos ];
            if (value  == 'O')
            {
                opp.Board[xpos, ypos] = 'M';
                p.Send("Miss:"+ShipNum);
                opp.Send("OMiss:"+ShipNum);
                p.Send("OTurn");
                opp.Send("Turn");
            }
            else if(value  == 'X'||value  == 'M')
            {
                p.Send("Invalid");
            }
            else
            {
                Console.WriteLine(value);
                opp.Board[xpos, ypos] = 'X';
                string SunkShip = GetTakenShip(value,opp);
                p.Send($"Hit:{value}:{ShipNum}");
                opp.Send($"OHit:{value}:{ShipNum}");
                if(SunkShip != null)
                {
                    p.Send($"OSunk:{value}:{SunkShip}");
                    opp.Send($"Sunk:{value}:{SunkShip}");
                    bool GameEnd = CheckGameEnd(opp);
                    if(GameEnd == true)
                    {
                        p.Send("Win");
                        p.GameIn = null;
                        p.Ships.Clear();
                        p.ShipSent = false;
                        opp.Send("Lose");
                        opp.GameIn = null;
                        opp.Ships.Clear();
                        opp.ShipSent = false;
                        return;
                    }
                }
                opp.Send("Turn");
                p.Send("OTurn");
            }
        }
        private bool CheckShipTaken(char ShipLetter , Player player)
        {
            bool Sunk = true;
            foreach(char c in player.Board)
            {
                if(c == ShipLetter)
                {
                    Sunk = false;
                }
            }
            return Sunk;
        }
        private bool CheckGameEnd(Player p)
        {
            return CheckShipTaken('0', p)
                && CheckShipTaken('1', p)
                && CheckShipTaken('2', p)
                && CheckShipTaken('3', p)
                && CheckShipTaken('4', p);
        }
        private string GetTakenShip(char ShipLetter , Player player)
        {
            if(CheckShipTaken(ShipLetter, player))
            {
                string ShipSunk = "";
                foreach (string s in player.Ships)
                {
                    if (s.StartsWith(ShipLetter.ToString()))
                    {
                        var splitlist = s.Split(':');
                        if (ShipSunk == "")
                        {
                            ShipSunk = splitlist[1];
                        }
                        else
                        {
                            ShipSunk += "," + splitlist[1];
                        }
                    }
                }
                return ShipSunk;
            }
            else
            {
                return null;
            }
        }
        public void Messaging(Player p , string message)
        {
            if (p == p1)
            {
                p2.Send(message);
            }
            else if (p == p2)
            {
                p1.Send(message);
            }
        }
        public void EndGame(Player p)
        {
            if (p2 == null)
            {
                Program.CurrentGames.Remove(this);
                p1.GameIn = null;
                return;
            }
            if (p ==p1)
            {
                p2.Send("Win");
            }
            else if (p == p2)
            {
                p1.Send("Win");
            }
            p1.GameIn = null;
            p1.Ships.Clear();
            p1.ShipSent = false;
            p2.GameIn = null;
            p2.Ships.Clear();
            p2.ShipSent = false;
            Program.CurrentGames.Remove(this);
        }
    }
}