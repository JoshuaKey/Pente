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
        public static Board board;
        public static bool player1Turn;

        public static void Initialize()
        {
            player1 = new Player();
            player2 = new Player();
            board = new Board(19, 19);
            player1Turn = true;
        }

        public static void SetPlayerNames(string p1Name, string p2Name)
        {
            player1.name = p1Name;
            player2.name = p2Name;
        }

        public static void PlacePiece(int x, int y, out string announcement)
        {
            if (board.GetState(x, y) == TileState.EMPTY)
            {
                TileState state = player1Turn ? player1.color : player2.color;
                board.Place(state, x, y);
                announcement = GetAnnouncement();
                player1Turn = !player1Turn;
            }
            else
            {
                announcement = "";
            }
        }

        private static string GetAnnouncement()
        {
            string announcement = "";

            if (HasTria())
            {
                announcement = "Tria";
            }
            else if (HasTessera())
            {
                announcement = "Tessera";
            }

            return announcement;
        }

        private static bool HasTria()
        {
            return false;
        }

        private static bool HasTessera()
        {
            return false;
        }
    }
}
