﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pente
{
	[Serializable]
    public class Board
    {
        public Piece[,] tiles;
        public int Width { get { return tiles.GetLength(0); } }
        public int Height { get { return tiles.GetLength(1); } }
		//private int Count { get; set; } = 0;

        public Board(int columns, int rows)
        {
            tiles = new Piece[columns, rows];
			Clear();
        }

        public void Place(TileState type, int x, int y)
        {
            tiles[x, y].TileState = type;
        }

        public bool Check(int x, int y)
        {
            if (tiles[x, y].TileState == TileState.EMPTY)
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
            if (tiles[x, y] != null)
            {
                tiles[x, y].TileState = TileState.EMPTY;
            }
        }

        public TileState GetState(int x, int y)
        {
            return tiles[x, y].TileState;
        }

        public bool IsValid(int x, int y)
        {
            return tiles[x, y] != null;
        }

        public void Clear()
        {
            for(int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    tiles[x, y] = new Piece();
                    Remove(x, y);
                }
            }
        }

		public bool IsFull()
		{
			for (int i = 0; i < tiles.GetLength(0); ++i) for (int j = 0; j < tiles.GetLength(1); ++j) if (tiles[i, j].TileState == TileState.EMPTY) return false;
			return true;
		}

		public bool IsInRange(int x, int y)
		{
			return x < tiles.GetLength(0) && x > -1 && y < tiles.GetLength(1) && y > -1;
		}
    }
}
