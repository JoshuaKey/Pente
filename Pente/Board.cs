﻿using System;
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

        public void Place(TileState type, int x, int y)
        {
            tiles[x, y] = type;
        }

        public bool Check(int x, int y)
        {
            if (tiles[x, y] == TileState.EMPTY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Remove(int x, int y)
        {
            tiles[x, y] = TileState.EMPTY;
        }

        public TileState GetState(int x, int y)
        {
            return tiles[x, y];
        }

        public void Clear()
        {
            for(int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    Remove(x, y);
                }
            }
        }
    }
}
