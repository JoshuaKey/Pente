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
    }
}
