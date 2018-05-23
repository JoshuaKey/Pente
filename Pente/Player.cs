using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pente
{
	[Serializable]
    public class Player
    {
        public string name;
        public TileState color;
        public bool isComputer;
        public int captures;
        public int turnCount;
    }
}
