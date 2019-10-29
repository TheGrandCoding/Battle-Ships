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
                    p1.Ships[a, b] = 'O';
                    p2.Ships[a, b] = 'O';
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
            value = opp.Ships[int.Parse(ShipNum[0].ToString()), int.Parse(ShipNum[1].ToString())];
            if (value  == 'O')
            {
                p.Send("Miss:"+ShipNum);
                opp.Send("OMiss:"+ShipNum);
            }else if(value  == 'X'||value  == 'M')
            {
                p.Send("Invalid");
            }
            else
            {
                p.Send("Hit:"+ShipNum);
                opp.Send("OHit:"+ShipNum);
            }
        }
    }
}