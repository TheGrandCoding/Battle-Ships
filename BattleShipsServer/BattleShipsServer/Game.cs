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
            p1.Send("Opp:"+p2.name);
            p2.Send("Opp:"+p1.name);
            for (int a = 0; a < 10; a++)
            {
                for (int b = 0; b < 10; b++)
                {
                    p1.Board[a, b] = 'O';
                    p2.Board[a, b] = 'O';
                }
            }
            Program.CurrentGames.Remove(this);
        }
        public void Play()
        {
            if (p1.ShipSent == true && p2.ShipSent == true)
            {
                int Turn = rnd.Next(0, 2);
                if(Turn == 0)
                {
                    p1.Send("Turn");
                }
                else
                {
                    p2.Send("Turn");
                }
            }
        }
        public void CheckShip(string ShipNum , Player p)
        {
            char value;
            Player opp;
            if (p == p1)
            {
                opp = p2;
            }
            else
            {
                opp = p1;
            }
            value = opp.Board[int.Parse(ShipNum[0].ToString()), int.Parse(ShipNum[1].ToString())];
            if (value  == 'O')
            {
                opp.Board[int.Parse(ShipNum[0].ToString()), int.Parse(ShipNum[1].ToString())] = 'M';
                p.Send("Miss:"+ShipNum);
                opp.Send("OMiss:"+ShipNum);

                opp.Send("Turn");
            }
            else if(value  == 'X'||value  == 'M')
            {
                p.Send("Invalid");
            }
            else
            {
                opp.Board[int.Parse(ShipNum[0].ToString()), int.Parse(ShipNum[1].ToString())] = 'X';
                string SunkShip = CheckShipTaken(value,opp);
                p.Send("Hit:"+ShipNum);
                opp.Send("OHit:"+ShipNum);
                if(SunkShip != null)
                {
                    p.Send("OSunk:"+SunkShip);
                }
                opp.Send("Turn");
            }
        }
        private string CheckShipTaken(char ShipLetter , Player player)
        {
            bool Sunk = true;
            foreach(char c in player.Board)
            {
                if(c == ShipLetter)
                {
                    Sunk = false;
                }
            }
            if(Sunk == true)
            {
                string ShipSunk = "";
                foreach(string s in player.Ships)
                {
                    if (s.StartsWith(ShipLetter.ToString()))
                    {
                        var splitlist = s.Split(':');
                        if(ShipSunk == "")
                        {
                            ShipSunk = splitlist[1];
                        }
                        else
                        {
                            ShipSunk +=","+ splitlist[1];
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
    }
}