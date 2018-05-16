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

        public Board(int columns, int rows)
        {
            tiles = new TileState[columns, rows];
        }

        public void Place(TileState type, int x, int y)
        {
            tiles[x, y] = type;
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
    }
}
