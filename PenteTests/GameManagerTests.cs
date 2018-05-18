using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pente;

namespace PenteTests {

    [TestClass]
    public class GameManagerTests {

        [TestMethod]
        public void GameManager_Init() {
            GameManager.Initialize();
            Assert.AreEqual("Player 1", GameManager.player1.name);
            Assert.AreEqual("Player 2", GameManager.player2.name);

            Assert.AreNotEqual(null, GameManager.board);
            Assert.AreNotEqual(null, GameManager.player1);
            Assert.AreNotEqual(null, GameManager.player2);
            Assert.AreEqual(true, GameManager.player1Turn);

            Assert.AreEqual(0, GameManager.player1.captures);
            Assert.AreEqual(0, GameManager.player2.captures);
            Assert.AreEqual(TileState.WHITE, GameManager.player1.color);
            Assert.AreEqual(TileState.BLACK, GameManager.player2.color);

            Assert.AreEqual(false, GameManager.player1.isComputer);

            Assert.AreEqual(19, GameManager.board.Height);
            Assert.AreEqual(19, GameManager.board.Width);
            Assert.AreNotEqual(null, GameManager.board.tiles);
        }

        [TestMethod]
        public void GameManager_BadInit() {

        }

        [TestMethod]
        public void GameManager_GameLoop() {
            GameManager.Initialize();

            GameManager.SetPlayerNames("Josh", "Lucas");
            Assert.AreEqual(TileState.WHITE, GameManager.player1.color);
            Assert.AreEqual(TileState.BLACK, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            GameManager.PlacePiece(half, half, out temp);
            Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half));

            GameManager.PlacePiece(half-1, half-1, out temp);
            Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half-1, half-1));

            GameManager.PlacePiece(half-4, half, out temp);
            Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half-4, half));

            GameManager.PlacePiece(half - 3, half - 3, out temp);
            Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 3, half - 3));
        }

        [TestMethod]
        public void GameManager_PlayerName() {
            GameManager.Initialize();

            GameManager.SetPlayerNames("Josh", "Lucas");
            Assert.AreEqual("Josh", GameManager.player1.name);
            Assert.AreEqual("Lucas", GameManager.player2.name);

            GameManager.SetPlayerNames("", "");
            Assert.AreEqual("Player 1", GameManager.player1.name);
            Assert.AreEqual("Player 2", GameManager.player2.name);

            GameManager.SetPlayerNames("", "Lucas");
            Assert.AreEqual("Player 1", GameManager.player1.name);
            Assert.AreEqual("Lucas", GameManager.player2.name);
            Assert.AreEqual(false, GameManager.player2.isComputer);

            GameManager.SetPlayerNames("Josh", "");
            Assert.AreEqual("Josh", GameManager.player1.name);
            Assert.AreEqual("Player 2", GameManager.player2.name);
            Assert.AreEqual(false, GameManager.player2.isComputer);

            GameManager.SetPlayerNames("Josh", "Computer");
            Assert.AreEqual("Josh", GameManager.player1.name);
            Assert.AreEqual("Computer", GameManager.player2.name);
            Assert.AreEqual(true, GameManager.player2.isComputer);
        }

        [TestMethod]
        public void GameManager_Computer() {
            GameManager.Initialize();

            GameManager.SetPlayerNames("Josh", "Computer");
            Assert.AreEqual("Josh", GameManager.player1.name);
            Assert.AreEqual("Computer", GameManager.player2.name);
            Assert.AreEqual(true, GameManager.player2.isComputer);

            string temp;
            int half = GameManager.board.Width / 2;

            // Player First turn
            GameManager.PlacePiece(half, half, out temp);

            int blackAmo = 0;
            for(int i = 0; i < GameManager.board.Height; i++) {
                for(int y = 0; y < GameManager.board.Width; y++) {
                    if(GameManager.board.GetState(y, i) == TileState.BLACK) {
                        blackAmo++;
                    }
                }
            }
            Assert.AreEqual(1, blackAmo);
            Assert.AreEqual(true, GameManager.player1Turn);

            // Player second Turn
            GameManager.PlacePiece(half + 4, half, out temp);

            blackAmo = 0;
            for (int i = 0; i < GameManager.board.Height; i++) {
                for (int y = 0; y < GameManager.board.Width; y++) {
                    if (GameManager.board.GetState(y, i) == TileState.BLACK) {
                        blackAmo++;
                    }
                }
            }
            Assert.AreEqual(2, blackAmo);
            Assert.AreEqual(true, GameManager.player1Turn);
        }

        [TestMethod]
        public void GameManager_MultipleCaptures() {
            GameManager.Initialize();
            Assert.AreEqual(TileState.WHITE, GameManager.player1.color);
            Assert.AreEqual(TileState.BLACK, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            // Multiple Captures Vertical Up, Horizontal Right, Diagonal Down Right
            {
                GameManager.Initialize();
                GameManager.PlacePiece(half, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half, half - 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half - 1)); // Vertical Setup

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half, half + 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1)); // Vertical Upp

                GameManager.PlacePiece(half + 3, half - 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 3, half - 1)); // Diagnol Setup

                GameManager.PlacePiece(half + 1, half + 2, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 1, half + 2)); // Horizontal Right

                GameManager.PlacePiece(half + 3, half + 2, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 3, half + 2)); // Horizontal Setup

                GameManager.PlacePiece(half + 2, half + 2, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 2, half + 2)); // Horizontal Right

                GameManager.PlacePiece(half - 1, half, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half -1, half)); // Random Black

                GameManager.PlacePiece(half + 2, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 2, half)); // Diagonal

                GameManager.PlacePiece(half - 2, half, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 2, half)); // Random Black

                GameManager.PlacePiece(half + 1, half + 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 1, half + 1)); // Diagonal

                GameManager.PlacePiece(half, half + 2, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2)); // Finish

                // Vertical
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half + 1));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half));

                // Horizontal
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half + 1, half + 2));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half + 2, half + 2));

                // Diagnoal
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half + 1, half + 1));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half + 2, half));
            }
        }

        [TestMethod]
        public void GameManager_HorizontalCapture() {
            GameManager.Initialize();
            Assert.AreEqual(TileState.WHITE, GameManager.player1.color);
            Assert.AreEqual(TileState.BLACK, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;



            // Horizontal Left (-, )
            {
                GameManager.Initialize();
                GameManager.PlacePiece(half, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half + 1, half, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 1, half)); // Opposite direction

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half - 1, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 1, half)); // Current direction

                GameManager.PlacePiece(half - 2, half, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 2, half)); // Capture

                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half - 1, half));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half));
            }
            // Horizontal Right (+, )
            {
                GameManager.Initialize();
                GameManager.PlacePiece(half, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half - 1, half, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 1, half)); // Opposite direction

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half + 1, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 1, half)); // Current direction

                GameManager.PlacePiece(half + 2, half, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 2, half)); // Capture

                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half + 1, half));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half));
            }
        }

        [TestMethod]
        public void GameManager_VerticalCapture() {
            GameManager.Initialize();
            Assert.AreEqual(TileState.WHITE, GameManager.player1.color);
            Assert.AreEqual(TileState.BLACK, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            // Vertical Up ( , +)
            {
                GameManager.Initialize();
                GameManager.PlacePiece(half, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half, half - 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half - 1)); // Opposite direction

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2)); 
                }


                GameManager.PlacePiece(half, half + 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1)); // Current direction

                GameManager.PlacePiece(half, half + 2, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2)); // Capture

                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half + 1));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half));
            }
            // Vertical Down ( , -)
            {
                GameManager.Initialize();
                GameManager.PlacePiece(half, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half, half + 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 1)); // Opposite direction

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half, half - 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half - 1)); // Current direction

                GameManager.PlacePiece(half, half - 2, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half - 2)); // Capture

                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half - 1));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half));
            }

        }

        [TestMethod]
        public void GameManager_DiagonalCapture() {
            GameManager.Initialize();
            Assert.AreEqual(TileState.WHITE, GameManager.player1.color);
            Assert.AreEqual(TileState.BLACK, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            // Diagonal Down, Left (-, -)
            {
                GameManager.Initialize();
                GameManager.PlacePiece(half, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half + 1, half + 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 1, half + 1)); // Opposite direction

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half - 1, half - 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 1, half - 1)); // Current direction

                GameManager.PlacePiece(half - 2, half - 2, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 2, half - 2)); // Capture

                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half - 1, half - 1));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half));
            }
            // Diagonal Down, Right (+, -)
            {
                GameManager.Initialize();
                GameManager.PlacePiece(half, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half + 1, half - 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 1, half - 1)); // Opposite direction

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half + 1, half - 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 1, half - 1)); // Current direction

                GameManager.PlacePiece(half + 2, half - 2, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 2, half - 2)); // Capture

                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half + 1, half - 1));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half));
            }
            // Diagonal Up, Left (-, +)
            {
                GameManager.Initialize();
                GameManager.PlacePiece(half, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half + 1, half - 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 1, half - 1)); // Opposite direction

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half - 1, half + 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 1, half + 1)); // Current direction

                GameManager.PlacePiece(half - 2, half + 2, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 2, half + 2)); // Capture

                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half - 1, half + 1));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half));
            }
            // Diagonal Up, Right (+, +)
            {
                GameManager.Initialize();
                GameManager.PlacePiece(half, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half - 1, half - 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 1, half - 1)); // Opposite direction

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half + 1, half + 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 1, half + 1)); // Current direction

                GameManager.PlacePiece(half + 2, half + 2, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 2, half + 2)); // Capture

                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half + 1, half + 1));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half));
            }
        }

        [TestMethod]
        public void GameManager_WhitePlacementIntoCapture() {
            GameManager.Initialize();
            Assert.AreEqual(TileState.WHITE, GameManager.player1.color);
            Assert.AreEqual(TileState.BLACK, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            // Vertical
            {
                GameManager.Initialize();
                GameManager.PlacePiece(half, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half, half + 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 1)); // Up

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half + 1, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 1, half)); // Random

                GameManager.PlacePiece(half, half - 2, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half - 2)); // Down 2

                GameManager.PlacePiece(half, half - 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half - 1)); // Placement


                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half - 1));
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half));
            }

            // Horizontal
            {
                GameManager.Initialize();
                GameManager.PlacePiece(half, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half + 1, half, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 1, half)); // Right

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half, half + 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1)); // Random

                GameManager.PlacePiece(half - 2, half, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 2, half)); // Left 2

                GameManager.PlacePiece(half - 1, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 1, half)); // Placement


                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 1, half));
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half));
            }

            // Diagonal
            {
                GameManager.Initialize();
                GameManager.PlacePiece(half, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half - 1, half + 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 1, half + 1)); // Left 1, Up 1

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half, half + 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1)); // Random

                GameManager.PlacePiece(half + 2, half - 2, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 2, half - 2)); // Down 2 Right 2

                GameManager.PlacePiece(half + 1, half - 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 1, half - 1)); // Placement


                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 1, half - 1));
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half));
            }
        }

        [TestMethod]
        public void GameManager_BlackPlacementIntoCapture() {
            GameManager.Initialize();
            Assert.AreEqual(TileState.WHITE, GameManager.player1.color);
            Assert.AreEqual(TileState.BLACK, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            // Vertical
            {
                GameManager.Initialize();
                GameManager.PlacePiece(half, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half, half - 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half - 1)); // Down

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half, half - 3, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half - 3)); // Placment

                GameManager.PlacePiece(half, half - 2, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half - 2)); // Down 2

                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half - 1));
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half - 2));
            }

            // Horizontal
            {
                GameManager.Initialize();
                GameManager.PlacePiece(half, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half - 1, half, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 1, half)); // Down

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half - 3, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 3, half)); // Placment

                GameManager.PlacePiece(half - 2, half, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 2, half)); // Down 2

                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 1, half));
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 2, half));
            }

            // Diagnol
            {
                GameManager.Initialize();
                GameManager.PlacePiece(half, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half - 1, half - 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 1, half - 1)); // Down

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half - 3, half - 3, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 3, half - 3)); // Placment

                GameManager.PlacePiece(half - 2, half - 2, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 2, half - 2)); // Down 2

                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 1, half - 2));
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 2, half - 2));
            }
        }

        [TestMethod]
        public void GameManager_ValidPlacement() {
            GameManager.Initialize();
            Assert.AreEqual(TileState.WHITE, GameManager.player1.color);
            Assert.AreEqual(TileState.BLACK, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            // Out of Bounds
            bool hasException = false;
            try {
                GameManager.PlacePiece(-1, -1, out temp);
            } catch(IndexOutOfRangeException e) {
                hasException = true;
            }
            Assert.AreEqual(true, hasException);

            // Out of Bounds
            hasException = false;
            try {
                GameManager.PlacePiece(GameManager.board.Width, GameManager.board.Height, out temp);
            } catch (IndexOutOfRangeException e) {
                hasException = true;
            }
            Assert.AreEqual(true, hasException);

            // Good Placement
            hasException = false;
            try {
                GameManager.PlacePiece(half, half, out temp);
            } catch (IndexOutOfRangeException e) {
                hasException = true;
            }
            Assert.AreEqual(false, hasException);

            // Onto itself
            hasException = false;
            try {
                GameManager.PlacePiece(half, half, out temp);
            } catch (IndexOutOfRangeException e) {
                hasException = true;
            }
            Assert.AreEqual(false, hasException);
            Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half));
            Assert.AreEqual(false, GameManager.player1Turn); // Still Player2's turn
            // Assert Turn Amo
        }

        [TestMethod]
        public void GameManager_WinPlacement() {
            GameManager.Initialize();
            Assert.AreEqual(TileState.WHITE, GameManager.player1.color);
            Assert.AreEqual(TileState.BLACK, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            GameManager.PlacePiece(half, half, out temp);
            Assert.AreNotEqual("Win", temp);

            GameManager.PlacePiece(half + 1, half + 1, out temp);
            Assert.AreNotEqual("Win", temp);

            GameManager.PlacePiece(half + 4, half, out temp);
            Assert.AreNotEqual("Win", temp);
        }

        [TestMethod]
        public void GameManager_WinCapture() {
            GameManager.Initialize();
            Assert.AreEqual(TileState.WHITE, GameManager.player1.color);
            Assert.AreEqual(TileState.BLACK, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            GameManager.PlacePiece(half, half, out temp);
            Assert.AreNotEqual("Win", temp);

            GameManager.PlacePiece(half + 1, half + 1, out temp);
            Assert.AreNotEqual("Win", temp);

            GameManager.PlacePiece(half + 4, half, out temp);
            Assert.AreNotEqual("Win", temp);
        }

        [TestMethod]
        public void GameManager_DrawCheck() {
            GameManager.Initialize();
            Assert.AreEqual(TileState.WHITE, GameManager.player1.color);
            Assert.AreEqual(TileState.BLACK, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            GameManager.PlacePiece(half, half, out temp);
            Assert.AreNotEqual("Win", temp);

            GameManager.PlacePiece(half + 1, half + 1, out temp);
            Assert.AreNotEqual("Win", temp);

            GameManager.PlacePiece(half + 4, half, out temp);
            Assert.AreNotEqual("Win", temp);
        }

        [TestMethod]
        public void GameManager_Tria() {
            GameManager.Initialize();
            Assert.AreEqual(TileState.WHITE, GameManager.player1.color);
            Assert.AreEqual(TileState.BLACK, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            // Both
            {
                GameManager.Initialize();
                // Before
                GameManager.PlacePiece(half, half, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half + 1, half, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half, half + 1, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half + 1, half + 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Is
                GameManager.PlacePiece(half, half + 2, out temp); // White
                Assert.AreEqual("Tria", temp);

                GameManager.PlacePiece(half + 1, half + 2, out temp); // Black
                Assert.AreEqual("Tria", temp);
            }

            // Top Side Not
            {
                GameManager.Initialize();
                // Before
                GameManager.PlacePiece(half, half, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half, half - 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half, half + 1, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half + 1, half + 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Is
                GameManager.PlacePiece(half, half + 2, out temp); // White
                Assert.AreEqual("Tria", temp);
            }

            // Bot Side Not
            {
                GameManager.Initialize();
                // Before
                GameManager.PlacePiece(half, half, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half, half + 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half, half - 1, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half + 1, half - 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Is
                GameManager.PlacePiece(half, half - 2, out temp); // White
                Assert.AreEqual("Tria", temp);
            }

            // Both side Not
            {
                GameManager.Initialize();
                // Before
                GameManager.PlacePiece(half, half, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half, half - 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half, half + 1, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half, half + 3, out temp); // Black
                Assert.AreEqual("", temp);

                // Is
                GameManager.PlacePiece(half, half + 2, out temp); // White
                Assert.AreNotEqual("Tria", temp);
            }

            // Almost Tessera Down
            {
                GameManager.Initialize();
                // Before
                GameManager.PlacePiece(half, half, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half + 1, half, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half, half + 1, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half + 1, half + 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Is
                GameManager.PlacePiece(half, half + 3, out temp); // White
                Assert.AreEqual("Tria", temp);
            }

            // Almost Tessera Right
            {
                GameManager.Initialize();
                // Before
                GameManager.PlacePiece(half, half, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half, half + 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half + 1, half, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half + 1, half + 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Is
                GameManager.PlacePiece(half + 3, half, out temp); // White
                Assert.AreEqual("Tria", temp);
            }

            // Almost Tessera Diagonal
            {
                GameManager.Initialize();
                // Before
                GameManager.PlacePiece(half, half, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half -1, half, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half + 1, half + 1, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half -1, half -1, out temp); // Black
                Assert.AreEqual("", temp);

                // Is
                GameManager.PlacePiece(half + 3, half + 3, out temp); // White
                Assert.AreEqual("Tria", temp);
            }
        }

        [TestMethod]
        public void GameManager_Tessera() {
            GameManager.Initialize();
            Assert.AreEqual(TileState.WHITE, GameManager.player1.color);
            Assert.AreEqual(TileState.BLACK, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            // Empty Both Sides
            {
                GameManager.Initialize();
                // Before
                GameManager.PlacePiece(half, half, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half + 1, half, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half, half + 1, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half + 1, half + 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Not
                GameManager.PlacePiece(half, half + 2, out temp); // White
                Assert.AreNotEqual("Tessera", temp);

                GameManager.PlacePiece(half + 1, half + 2, out temp); // Black
                Assert.AreNotEqual("Tessera", temp);

                // Is
                GameManager.PlacePiece(half, half + 3, out temp); // White
                Assert.AreEqual("Tessera", temp);

                GameManager.PlacePiece(half + 1, half + 3, out temp); // Black
                Assert.AreEqual("Tessera", temp);
            }

            // Top side not 
            {
                GameManager.Initialize();
                // Before
                GameManager.PlacePiece(half, half, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half, half + 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half, half - 1, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half + 1, half + 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Not
                GameManager.PlacePiece(half, half - 2, out temp); // White
                Assert.AreNotEqual("Tessera", temp);

                GameManager.PlacePiece(half + 1, half + 2, out temp); // Black
                Assert.AreNotEqual("Tessera", temp);

                // Is
                GameManager.PlacePiece(half, half - 3, out temp); // White
                Assert.AreEqual("Tessera", temp);

            }

            // Bot side not 
            {
                GameManager.Initialize();
                // Before
                GameManager.PlacePiece(half, half, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half, half - 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half, half + 1, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half + 1, half + 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Not
                GameManager.PlacePiece(half, half + 2, out temp); // White
                Assert.AreNotEqual("Tessera", temp);

                GameManager.PlacePiece(half + 1, half + 2, out temp); // Black
                Assert.AreNotEqual("Tessera", temp);

                // Is
                GameManager.PlacePiece(half, half + 3, out temp); // White
                Assert.AreEqual("Tessera", temp);

            }

            // Both
            {
                GameManager.Initialize();
                // Before
                GameManager.PlacePiece(half, half, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half, half + 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 2));
                }

                GameManager.PlacePiece(half, half - 1, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half, half - 4, out temp); // Black
                Assert.AreEqual("", temp);

                // Not
                GameManager.PlacePiece(half, half - 2, out temp); // White
                Assert.AreNotEqual("Tessera", temp);

                GameManager.PlacePiece(half + 1, half + 2, out temp); // Black
                Assert.AreNotEqual("Tessera", temp);

                // Is
                GameManager.PlacePiece(half, half - 3, out temp); // White
                Assert.AreNotEqual("Tessera", temp);

            }
        }

        [TestMethod]
        public void GameManager_InitialMove() {
            GameManager.Initialize();
            Assert.AreEqual(TileState.WHITE, GameManager.player1.color);
            Assert.AreEqual(TileState.BLACK, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            // Not Centered
            bool hasException = false;
            try {
                GameManager.Initialize();
                GameManager.PlacePiece(half - 1, half - 1, out temp);
            } catch (IndexOutOfRangeException e) {
                hasException = true;
            }
            Assert.AreEqual(false, hasException);
            Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half - 1, half - 1));
            Assert.AreEqual(true, GameManager.player1Turn);

            // Not Centered
            hasException = false;
            try {
                GameManager.Initialize();
                GameManager.PlacePiece(half + 1, half + 1, out temp);
            } catch (IndexOutOfRangeException e) {
                hasException = true;
            }
            Assert.AreEqual(false, hasException);
            Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half + 1, half + 1));
            Assert.AreEqual(true, GameManager.player1Turn);


            // Not Centered
            hasException = false;
            try {
                GameManager.Initialize();
                GameManager.PlacePiece(half, half, out temp);
            } catch (IndexOutOfRangeException e) {
                hasException = true;
            }
            Assert.AreEqual(false, hasException);
            Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half));
            Assert.AreEqual(false, GameManager.player1Turn);
        }

        [TestMethod]
        public void GameManager_MoveLimitation() {
            GameManager.Initialize();
            Assert.AreEqual(TileState.WHITE, GameManager.player1.color);
            Assert.AreEqual(TileState.BLACK, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;
            // Distance is a Square Grid

            // InValid Placements
            { 
                // Right
                bool hasException = false;
                try {
                    GameManager.Initialize();
                    GameManager.PlacePiece(half, half, out temp);
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half + 2, half, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half + 2, half));
                Assert.AreEqual(true, GameManager.player1Turn);

                // Left
                hasException = false;
                try {
                    GameManager.Initialize();
                    GameManager.PlacePiece(half, half, out temp);
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half - 2, half, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half - 2, half));
                Assert.AreEqual(true, GameManager.player1Turn);

                // Up
                hasException = false;
                try {
                    GameManager.Initialize();
                    GameManager.PlacePiece(half, half, out temp);
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half, half + 2, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half + 2));
                Assert.AreEqual(true, GameManager.player1Turn);

                // Down
                hasException = false;
                try {
                    GameManager.Initialize();
                    GameManager.PlacePiece(half, half, out temp);
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half, half - 2, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half - 2));
                Assert.AreEqual(true, GameManager.player1Turn);

                // Down Right
                hasException = false;
                try {
                    GameManager.Initialize();
                    GameManager.PlacePiece(half, half, out temp);
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half + 2, half - 2, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half + 2, half - 2));
                Assert.AreEqual(true, GameManager.player1Turn);

                // Down Left
                hasException = false;
                try {
                    GameManager.Initialize();
                    GameManager.PlacePiece(half, half, out temp);
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half - 2, half - 2, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half - 2, half - 2));
                Assert.AreEqual(true, GameManager.player1Turn);

                // Up Left
                hasException = false;
                try {
                    GameManager.Initialize();
                    GameManager.PlacePiece(half, half, out temp);
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half - 2, half + 2, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half - 2, half + 2));
                Assert.AreEqual(true, GameManager.player1Turn);

                // Up Right
                hasException = false;
                try {
                    GameManager.Initialize();
                    GameManager.PlacePiece(half, half, out temp);
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half + 2, half + 2, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half + 2, half + 2));
                Assert.AreEqual(true, GameManager.player1Turn);
            }

            // Valid Placements
            {
                // Right
                bool hasException = false;
                try {
                    GameManager.Initialize();
                    GameManager.PlacePiece(half, half, out temp);
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half + 3, half, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 3, half));
                Assert.AreEqual(false, GameManager.player1Turn);

                // Left
                hasException = false;
                try {
                    GameManager.Initialize();
                    GameManager.PlacePiece(half, half, out temp);
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half - 3, half, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 3, half));
                Assert.AreEqual(false, GameManager.player1Turn);

                // Up
                hasException = false;
                try {
                    GameManager.Initialize();
                    GameManager.PlacePiece(half, half, out temp);
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half, half + 3, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 3));
                Assert.AreEqual(false, GameManager.player1Turn);

                // Down
                hasException = false;
                try {
                    GameManager.Initialize();
                    GameManager.PlacePiece(half, half, out temp);
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half, half - 3, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half - 3));
                Assert.AreEqual(false, GameManager.player1Turn);

                // Down Right
                hasException = false;
                try {
                    GameManager.Initialize();
                    GameManager.PlacePiece(half, half, out temp);
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half + 3, half - 3, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 3, half - 3));
                Assert.AreEqual(false, GameManager.player1Turn);

                // Down Left
                hasException = false;
                try {
                    GameManager.Initialize();
                    GameManager.PlacePiece(half, half, out temp);
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half - 3, half - 3, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 3, half - 3));
                Assert.AreEqual(false, GameManager.player1Turn);

                // Up Left
                hasException = false;
                try {
                    GameManager.Initialize();
                    GameManager.PlacePiece(half, half, out temp);
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half - 3, half + 3, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 3, half + 3));
                Assert.AreEqual(false, GameManager.player1Turn);

                // Up Right
                hasException = false;
                try {
                    GameManager.Initialize();
                    GameManager.PlacePiece(half, half, out temp);
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half + 3, half + 3, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 3, half + 3));
                Assert.AreEqual(false, GameManager.player1Turn);
            }
        }

        [TestMethod]
        public void GameManager_CanSave() {
            GameManager.Initialize();

            string temp;
            int half = GameManager.board.Width / 2;

            GameManager.PlacePiece(half, half, out temp);
            GameManager.PlacePiece(half + 1, half, out temp);
            GameManager.PlacePiece(half + 4, half, out temp);

            //GameManager.Save();
        }

        [TestMethod]
        public void GameManager_CanLoad() {
            //GameManager.Load();
        }

        [TestMethod]
        public void GameManager_SaveLoad() {
            GameManager.Initialize();

            Player player1;
            Player player2;
            bool player1Turn;
            TileState[,] tiles;
            string temp;
            int half = GameManager.board.Width / 2;

            // Empty Board
            {
                GameManager.Initialize();
                Save(out player1, out player2, out player1Turn, out tiles);

                //GameManager.Save();
                GameManager.Initialize();
                //GameManager.Load();

                AssertSaveLoad(player1, player2, player1Turn, tiles);
            }

            // Basic Board
            {
                GameManager.Initialize();
                GameManager.PlacePiece(half, half, out temp);
                GameManager.PlacePiece(half - 1, half - 1, out temp);
                GameManager.PlacePiece(half - 4, half - 3, out temp);
                Save(out player1, out player2, out player1Turn, out tiles);

                //GameManager.Save();
                GameManager.Initialize();
                //GameManager.Load();

                AssertSaveLoad(player1, player2, player1Turn, tiles);
            }

            // Capture Board
            {
                GameManager.Initialize();
                GameManager.PlacePiece(half, half, out temp);
                GameManager.PlacePiece(half - 1, half, out temp);

                GameManager.PlacePiece(half - 4, half, out temp);
                GameManager.PlacePiece(half - 5, half - 5, out temp);

                GameManager.PlacePiece(half + 1, half, out temp);
                GameManager.PlacePiece(half + 2, half, out temp); // Should Capture
                Save(out player1, out player2, out player1Turn, out tiles);

                //GameManager.Save();
                GameManager.Initialize();
                //GameManager.Load();

                AssertSaveLoad(player1, player2, player1Turn, tiles);
            }

            // Computer Board
            {
                GameManager.Initialize();
                GameManager.SetPlayerNames("Josh", "Computer");
                GameManager.PlacePiece(half, half, out temp);
                //GameManager.PlacePiece(half - 1, half, out temp);

                GameManager.PlacePiece(half - 4, half, out temp);
                //GameManager.PlacePiece(half - 5, half - 5, out temp);

                GameManager.PlacePiece(half + 1, half, out temp);
                //GameManager.PlacePiece(half + 2, half, out temp); // Should Capture
                Save(out player1, out player2, out player1Turn, out tiles);

                //GameManager.Save();
                GameManager.Initialize();
                //GameManager.Load();

                AssertSaveLoad(player1, player2, player1Turn, tiles);
            }
        }

        public void Save(out Player player1, out Player player2, out bool player1Turn, out TileState[,] tiles) {
            tiles = new TileState[GameManager.board.Width, GameManager.board.Height];

            for (int i = 0; i < GameManager.board.Height; i++) {
                for (int y = 0; y < GameManager.board.Width; y++) {
                    tiles[y, i] = GameManager.board.GetState(y, i);
                }
            }

            player1Turn = GameManager.player1Turn;

            player1 = new Player();
            player1.captures = GameManager.player1.captures;
            player1.name = GameManager.player1.name;
            player1.isComputer = GameManager.player1.isComputer;
            player1.color = GameManager.player1.color;

            player2 = new Player();
            player2.captures = GameManager.player2.captures;
            player2.name = GameManager.player2.name;
            player2.isComputer = GameManager.player2.isComputer;
            player2.color = GameManager.player2.color;
        }
        public void AssertSaveLoad(Player player1, Player player2, bool player1Turn, TileState[,] tiles) {
            Assert.AreEqual(player1.color, GameManager.player1.color);
            Assert.AreEqual(player1.captures, GameManager.player1.captures);
            Assert.AreEqual(player1.isComputer, GameManager.player1.isComputer);
            Assert.AreEqual(player1.name, GameManager.player1.name);

            Assert.AreEqual(player2.color, GameManager.player2.color);
            Assert.AreEqual(player2.captures, GameManager.player2.captures);
            Assert.AreEqual(player2.isComputer, GameManager.player2.isComputer);
            Assert.AreEqual(player2.name, GameManager.player2.name);

            Assert.AreEqual(player1Turn, GameManager.player1Turn);

            for (int i = 0; i < GameManager.board.Height; i++) {
                for (int y = 0; y < GameManager.board.Width; y++) {
                    Assert.AreEqual(tiles[y, i], GameManager.board.GetState(y, i));
                }
            }
        }
    }
}
