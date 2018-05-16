using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pente
{
    public static class GameManager
    {
        public static Player player1;
        public static Player player2;

        public static void Initialize()
        {
            player1 = new Player();
            player2 = new Player();
        }

        public static void SetPlayerNames(string p1Name, string p2Name)
        {
            player1.name = p1Name;
            player2.name = p2Name;
        }
    }
}
