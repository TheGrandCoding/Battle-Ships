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
            p1.GameIn = null;
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
                string SunkShip = GetTakenShip(value,opp);
                p.Send("Hit:"+ShipNum);
                opp.Send("OHit:"+ShipNum);
                if(SunkShip != null)
                {
                    p.Send("OSunk:"+SunkShip);
                    opp.Send("Sunk:" + SunkShip);
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
            bool GameEnd;
            GameEnd = CheckShipTaken('A', p);
            GameEnd = CheckShipTaken('B', p);
            GameEnd = CheckShipTaken('C', p);
            GameEnd = CheckShipTaken('D', p);
            GameEnd = CheckShipTaken('E', p);
            return GameEnd;
        }
        private string GetTakenShip(char ShipLetter , Player player)
        {
            bool Sunk = CheckShipTaken(ShipLetter, player);
            if (Sunk == true)
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
    }
}