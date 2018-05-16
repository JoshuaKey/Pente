using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pente
{
    public class Board
    {
        public TileState[,] tiles;
        public int Width { get { return tiles.GetLength(0); } }
        public int Height { get { return tiles.GetLength(1); } }

        public Board(int columns, int rows)
        {
            tiles = new TileState[columns, rows];

        }

        public string Place(TileState type, int x, int y)
        {
            tiles[x, y] = type;
            return GetAnnouncement();
        }

        public bool Check(int x, int y)
        {
            return false;
        }

        public void Remove(int x, int y)
        {

        }

        public TileState GetState(int x, int y)
        {
            return TileState.EMPTY;
        }

        private string GetAnnouncement()
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

        private bool HasTria()
        {
            return false;
        }

        private bool HasTessera()
        {
            return false;
        }
    }
}
