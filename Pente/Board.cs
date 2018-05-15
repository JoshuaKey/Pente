using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pente
{
    public class Board
    {
        public void Place(BoardState type, int x, int y);

        public bool Check(int x, int y);

        public void Remove(int x, int y);

        public BoardState GetState(int x, int y);
    }
}
