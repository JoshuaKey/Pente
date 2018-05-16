using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pente
{
    public class Board
    {
        public void Place(TileState type, int x, int y)
        {

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
