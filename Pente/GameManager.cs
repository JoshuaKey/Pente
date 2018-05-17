﻿using System;
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
            if (p2Name == "Computer")
            {
                player2.isComputer = true;
            }
			player1.color = TileState.WHITE;
			player2.color = TileState.BLACK;
        }

        public static void PlacePiece(int x, int y, out string announcement)
        {
            if (board.GetState(x, y) == TileState.EMPTY)
            {
                TileState state = player1Turn ? player1.color : player2.color;
                board.Place(state, x, y);
				announcement = GetAnnouncement(x, y);
                player1Turn = !player1Turn;
				Console.WriteLine("Value: " + board.GetState(x, y));
            }
            else
            {
                announcement = "";
            }
        }

        private static string GetAnnouncement(int x, int y)
        {
            string announcement = "";

			if (HasTessera(x, y))
			{
				announcement = "Tessera";
			}
			else if (HasTria(x, y))
			{
				announcement = "Tria";
			}

			return announcement;
        }

        private static bool HasTria(int x, int y)
        {
			for (int dx = -1; dx <= 1; ++dx)
			{
				for (int dy = -1; dy <= 1; ++dy)
				{
					if (dx == 0 && dy == 0) continue;
					for (int i = -3; i <= 0; ++i)
					{
						int numInRow = 0;
						for (int j = i; j <= 3 + i; ++j)
						{
							try
							{
								if (board.GetState(x, y) == board.GetState(x + dx * j, y + dy * j)) ++numInRow;
							}
							catch (IndexOutOfRangeException) { break; }
						}
						if (numInRow == 3) return true;
					}
				}
			}
			return false;
        }

        private static bool HasTessera(int x, int y)
        {
			//Console.WriteLine("Tessera Check");
			//Console.WriteLine("Value: " + board.GetState(x, y));
			//for (int dx = -1; dx <= 1; ++dx)
			//{
			//	for (int dy = -1; dy <= 1; ++dy)
			//	{
			//		if (dx == 0 && dy == 0) continue;
			//		for (int i = -3; i <= 0; ++i)
			//		{
			//			int numInRow = 0;
			//			for (int j = i; j <= 3 + i; ++j)
			//			{
			//				try
			//				{
			//					Console.WriteLine("x: " + x + " y: " + y + " dx: " + dx + " dy: " + dy + " j: " + j);
			//					Console.WriteLine("Value: " + board.GetState(x + dx * j, y + dy * j));
			//					if (board.GetState(x, y) == board.GetState(x + dx * j, y + dy * j)) ++numInRow;
			//				}
			//				catch (IndexOutOfRangeException) { break; }
			//			}
			//			if (numInRow == 4) return true;
			//		}
			//	}
			//}
			return false;
        }

		private static bool HasPente(int x, int y)
		{
			for (int dx = -1; dx <= 1; ++dx)
			{
				for (int dy = -1; dy <= 1; ++dy)
				{
					if (dx == 0 && dy == 0) continue;
					for (int i = -4; i <= 0; ++i)
					{
						int numInRow = 0;
						for (int j = i; j <= 4 + i; ++j)
						{
							try
							{
								if (board.GetState(x, y) == board.GetState(x + dx * j, y + dy * j)) ++numInRow;
							}
							catch (IndexOutOfRangeException) { break; }
						} 
						if (numInRow == 5) return true;
					}
				}
			}
			return false;
		}

		private static int HasCaptures(int x, int y)
		{
			int captures = 0;
			TileState thisState = board.GetState(x, y);
			TileState otherState = thisState == TileState.WHITE ? TileState.BLACK : TileState.WHITE;
			for (int dx = -1; dx <= 1; ++dx)
			{
				for (int dy = -1; dy <= 1; ++dy)
				{
					if (dx == 0 && dy == 0) continue;
					try
					{
						if (board.GetState(x + dx, y + dy) == otherState &&
							board.GetState(x + dx * 2, y + dy * 2) == otherState &&
							board.GetState(x + dx * 3, y + dy * 3) == thisState)
						{
							captures++;
							board.Remove(x + dx, y + dy);
							board.Remove(x + dx * 2, y + dy * 2);
						}
					}
					catch (IndexOutOfRangeException) { break; }
				}
			}
			return captures;
		}

		//private static int NumInRow(int x, int y, int dx, int dy, int length)
		//{
		//	int num = 0;
		//	for (int i = 0; i < length; ++i)
		//	{
		//		try
		//		{
		//			if (board.GetState(x, y) == board.GetState(x + dx * i, y + dy * i)) ++num;
		//		} catch (IndexOutOfRangeException) { break; }
		//	}
		//	return num;
		//}
    }
}
