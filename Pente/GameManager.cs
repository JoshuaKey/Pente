using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;

namespace Pente
{
    public static class GameManager
    {
        public static Player player1;
        public static Player player2;
        public static Board board;
        private static int turnCount;
        public static bool player1Turn;
        public static bool BoardLocked { get; private set; }

        public static void Initialize(int boardSize)
        {
			if (boardSize < 9 || boardSize > 39 || boardSize % 2 == 0) throw new ArgumentOutOfRangeException();
            player1 = new Player();
            player2 = new Player();
            player1.name = "Player 1";
            player2.name = "Player 2";
            player1.color = TileState.BLACK;
            player2.color = TileState.WHITE;
            board = new Board(boardSize, boardSize);
            board.Place(TileState.BLACK, board.Width / 2, board.Height / 2);
            turnCount = 2;
            player1Turn = false;
            BoardLocked = false;
        }
        public static void SetPlayerNames(string p1Name, string p2Name)
        {
			Player p1 = player1;
			Player p2 = player2;
            player1.name = string.IsNullOrEmpty(p1Name) ? "Player 1" : p1Name;
            player2.name = string.IsNullOrEmpty(p2Name) ? "Player 2" : p2Name;
            if (p2Name == "Computer")
            {
                player2.isComputer = true;
            }
			player1.color = TileState.BLACK;
			player2.color = TileState.WHITE;
        }

        public static Player GetCurrentPlayer()
        {
            Player player = player1Turn ? player1 : player2;

            return player;
        }

        public static void SwitchPlayerTurn()
        {
            player1Turn = !player1Turn;
            turnCount++;
        }

        public static bool PlacePiece(int x, int y, out string announcement)
        {
            bool success = true;
            announcement = "";
            bool p1secondMove = GetCurrentPlayer().turnCount == 0 && player1Turn;
            if ((p1secondMove && !IsValidSecondMove(x, y)))
            {
                announcement = "Second";
                return false;
            }
            if (board.IsValid(x, y))
            {
                if (board.IsFull())
                {
                    BoardLocked = true;
                    return false;
                }
                if (board.GetState(x, y) == TileState.EMPTY && !BoardLocked)
                {
                    TileState state = player1Turn ? player1.color : player2.color;
                    board.Place(state, x, y);
                    announcement = GetAnnouncementFromPlacement(x, y);
                    if (announcement == "Pente" || announcement == "Capture")
                    {
                        success = false;
                    }
                    GetCurrentPlayer().turnCount++;
                    SwitchPlayerTurn();
                }
                else
                {
                    success = false;
                    announcement = "";
                }
            }
            else
            {
                success = false;
                player1Turn = !player1Turn;
                announcement = "";
            }

            return success;
        }

        private static bool IsValidSecondMove(int x, int y)
        {
            bool valid = false;

            int center = board.Width / 2;
            int xDist = Math.Abs(center - x);
            int yDist = Math.Abs(center - y);
            if (xDist >= 3 || yDist >= 3)
            {
                valid = true;
            }

            return valid;
        }

        private static string GetAnnouncementFromPlacement(int x, int y)
        {
            string announcement = "";

            if (board.IsValid(x, y))
            {
				int captures = HasCaptures(x, y);
				GetCurrentPlayer().captures += captures;
				if (GetCurrentPlayer().captures >= 5)
				{
					announcement = "Capture";
					BoardLocked = true;
				}
				else if (HasPente(x, y))
				{
					announcement = "Pente";
					BoardLocked = true;
				}
				else if (HasTessera(x, y))
				{
					announcement = "Tessera";
				}
				else if (HasTria(x, y))
				{
					announcement = "Tria";
				}
				else if (captures >= 1)
				{
					announcement = "Capture" + captures;
				}
			}

            return announcement;
        }

        public static void MakeComputerMove(out string announcement)
        {
            if (!GetCurrentPlayer().isComputer)
            {
                announcement = "";
                return;
            }
            
            int x = -1;
            int y = -1;
            Random rand = new Random();
            do
            {
                x = rand.Next(0, board.Width);
                y = rand.Next(0, board.Height);
            } while (!board.IsValid(x, y));

            PlacePiece(x, y, out announcement);
        }
        private static bool HasTria(int x, int y)
		{
			TileState thisState = board.GetState(x, y);
			TileState otherState = thisState == TileState.WHITE ? TileState.BLACK : TileState.WHITE;
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
							if (!board.IsInRange(x + dx * j, y + dy * j)) break;
							TileState state = board.GetState(x + dx * j, y + dy * j);
							if (state == thisState) ++numInRow;
							if (state == otherState) --numInRow;
						}
						if (numInRow == 3 && board.IsInRange(x + dx * (i - 1), y + dy * (i - 1)) && board.GetState(x + dx * (i - 1), y + dy * (i - 1)) == otherState
							&& numInRow == 3 && board.IsInRange(x + dx * (i + 4), y + dy * (i + 4)) && board.GetState(x + dx * (i + 4), y + dy * (i + 4)) == otherState) return false;
						if (numInRow == 3) return true;
					}
				}
			}
			return false;
        }
        private static bool HasTessera(int x, int y)
		{
			TileState thisState = board.GetState(x, y);
			TileState otherState = thisState == TileState.WHITE ? TileState.BLACK : TileState.WHITE;
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
							if (!board.IsInRange(x + dx * j, y + dy * j)) break;
							if (board.GetState(x + dx * j, y + dy * j) == thisState) ++numInRow;
						}
						if (numInRow == 4 && board.IsInRange(x + dx * (i - 1), y + dy * (i - 1)) && board.GetState(x + dx * (i - 1), y + dy * (i - 1)) == otherState
							&& numInRow == 4 && board.IsInRange(x + dx * (i + 4), y + dy * (i + 4)) && board.GetState(x + dx * (i + 4), y + dy * (i + 4)) == otherState) return false;
						if (numInRow == 4) return true;
					}
				}
			}
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
							if (!board.IsInRange(x + dx * j, y + dy * j)) break;
							if (board.GetState(x, y) == board.GetState(x + dx * j, y + dy * j)) ++numInRow;
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
					if ((dx == 0 && dy == 0) || !board.IsInRange(x + dx * 3, y + dy * 3)) continue;
					if (board.GetState(x + dx, y + dy) == otherState &&
						board.GetState(x + dx * 2, y + dy * 2) == otherState &&
						board.GetState(x + dx * 3, y + dy * 3) == thisState)
					{
						captures++;
						board.Remove(x + dx, y + dy);
						board.Remove(x + dx * 2, y + dy * 2);
					}
				}
			}
			return captures;
		}
		public static bool Save()
		{
			SaveFileDialog sFileDiag = new SaveFileDialog();
			sFileDiag.Title = "Save Game Location";
			sFileDiag.Filter = "Pente Save|*.save";
			sFileDiag.ShowDialog();
			if (sFileDiag.FileName == "") return false;
			Serialize(sFileDiag.FileName);
			return true;
		}
		public static bool Load()
		{
			OpenFileDialog oFileDiag = new OpenFileDialog();
			oFileDiag.Title = "Load Game";
			oFileDiag.Filter = "Pente Save|*.save";
			oFileDiag.ShowDialog();
			if (oFileDiag.FileName == "") return false;
			Deserialize(oFileDiag.FileName);
			return true;
		}
		public static void Serialize(string filepath)
		{
			Stream fstream = File.Open(filepath, FileMode.OpenOrCreate);
			BinaryFormatter format = new BinaryFormatter();
			object[] objs = new object[]
			{
				player1,
				player2,
				board,
				player1Turn,
                BoardLocked,
                turnCount
			};
			format.Serialize(fstream, objs);
			fstream.Close();
		}
		public static void Deserialize(string filepath)
		{
			Stream fstream = File.Open(filepath, FileMode.Open);
			BinaryFormatter format = new BinaryFormatter();
			object[] objs = (object[])format.Deserialize(fstream);
			player1 = (Player)objs[0];
			player2 = (Player)objs[1];
			board = (Board)objs[2];
			player1Turn = (bool)objs[3];
            BoardLocked = (bool)objs[4];
            turnCount = (int)objs[5];
			fstream.Close();
		}
    }
}
