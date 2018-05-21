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
        public static int size;
        public static bool player1Turn;
        public static bool locked;
        public static bool BoardLocked { get; set; }

        public static void Initialize(int boardSize)
        {
            player1 = new Player();
            player2 = new Player();
            player1.name = "Player 1";
            player2.name = "Player 2";
            player1.color = TileState.WHITE;
            player2.color = TileState.BLACK;
            size = boardSize;
            board = new Board(size, size);
            player1Turn = true;
            BoardLocked = false;
        }

        public static void SetPlayerNames(string p1Name, string p2Name)
        {
            player1.name = string.IsNullOrEmpty(p1Name) ? "Player 1" : p1Name;
            player2.name = string.IsNullOrEmpty(p2Name) ? "Player 2" : p2Name;
            if (p2Name == "Computer")
            {
                player2.isComputer = true;
            }
			player1.color = TileState.WHITE;
			player2.color = TileState.BLACK;
        }

        public static Player GetCurrentPlayer()
        {
            Player player = player1Turn ? player1 : player2;

            return player;
        }

        public static bool PlacePiece(int x, int y, out string announcement)
        {
            bool success = true;
            if (board.IsValid(x, y))
            {
                if (board.GetState(x, y) == TileState.EMPTY && !BoardLocked)
                {
                    TileState state = player1Turn ? player1.color : player2.color;
                    board.Place(state, x, y);
                    announcement = GetAnnouncement(x, y);
                    if (announcement == "Pente" || announcement == "Capture")
                    {
                        success = false;
                    }
                    player1Turn = !player1Turn;
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

        private static string GetAnnouncement(int x, int y)
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
							try
							{
								TileState state = board.GetState(x + dx * j, y + dy * j);
								if (state == thisState) ++numInRow;
								if (state == otherState) --numInRow;
							}
							catch (IndexOutOfRangeException) { break; }
						}
						bool almostBracket = false;
						try { if (board.GetState(x + dx * (i - 1), y + dy * (i - 1)) == otherState) almostBracket = true; } catch (IndexOutOfRangeException) { almostBracket = true; }
						try { if (almostBracket && board.GetState(x + dx * (i + 4), y + dy * (i + 4)) == otherState) return false; } catch (IndexOutOfRangeException) { if (almostBracket) return false; }
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
							try
							{
								if (board.GetState(x + dx * j, y + dy * j) == thisState) ++numInRow;
							}
							catch (IndexOutOfRangeException) { break; }
						}
						bool almostBracket = false;
						try	{ if (board.GetState(x + dx * (i - 1), y + dy * (i - 1)) == otherState) almostBracket = true;} catch (IndexOutOfRangeException) { almostBracket = true; }
						try { if (almostBracket && board.GetState(x + dx * (i + 4), y + dy * (i + 4)) == otherState) return false; } catch (IndexOutOfRangeException) { if (almostBracket) return false; }
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

		public static bool Save()
		{
			SaveFileDialog sFileDiag = new SaveFileDialog();
			sFileDiag.Title = "Save Game Location";
			sFileDiag.Filter = "Pente Save|*.save";
			sFileDiag.ShowDialog();
			if (sFileDiag.FileName == "") return false;

			object[] objs = new object[]
			{
				player1,
				player2,
				board,
				player1Turn
			};
			Stream fstream = sFileDiag.OpenFile();
			BinaryFormatter format = new BinaryFormatter();
			format.Serialize(fstream, objs);
			fstream.Close();
			return true;
		}

		public static bool Load()
		{
			OpenFileDialog oFileDiag = new OpenFileDialog();
			oFileDiag.Title = "Load Game";
			oFileDiag.Filter = "Pente Save|*.save";
			oFileDiag.ShowDialog();
			if (oFileDiag.FileName == "") return false;

			Stream fstream = oFileDiag.OpenFile();
			BinaryFormatter format = new BinaryFormatter();
			object[] objs = (object[])format.Deserialize(fstream);
			player1 = (Player)objs[0];
			player2 = (Player)objs[1];
			board = (Board)objs[2];
			player1Turn = (bool)objs[3];
			fstream.Close();
			return true;
		}
    }
}
