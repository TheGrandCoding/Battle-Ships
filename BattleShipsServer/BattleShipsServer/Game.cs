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
            p1.Send("StartGame");//possibly change to opponents name?
            p2.Send("StartGame");
        }
    }
}
