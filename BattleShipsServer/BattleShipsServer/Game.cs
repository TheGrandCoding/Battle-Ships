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
        public void StartGame()
        {
            p1.Send("Opp:"+p2.name);
            p2.Send("Opp:"+p1.name);
            for (int a = 0; a < p1.Ships.GetLength(0); a++)
            {
                for (int b = 0; b < p1.Ships.GetLength(0); b++)
                {
                    p1.Ships[a, b] = 'O';
                    p1.Ships[a, b] = 'O';
                }
            }
            Program.CurrentGames.Remove(this);
        }
    }
}
