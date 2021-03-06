using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pente;

namespace PenteTests {

    [TestClass]
    public class GameManagerTests {

        [TestMethod]
        public void GameManager_Init() {
            GameManager.Initialize(19);
            Assert.AreEqual("Player 1", GameManager.player1.name);
            Assert.AreEqual("Player 2", GameManager.player2.name);

            Assert.AreNotEqual(null, GameManager.board);
            Assert.AreNotEqual(null, GameManager.player1);
            Assert.AreNotEqual(null, GameManager.player2);
            Assert.AreEqual(false, GameManager.player1Turn);

            Assert.AreEqual(0, GameManager.player1.captures);
            Assert.AreEqual(0, GameManager.player2.captures);
            Assert.AreEqual(TileState.BLACK, GameManager.player1.color);
            Assert.AreEqual(TileState.WHITE, GameManager.player2.color);

            Assert.AreEqual(false, GameManager.player1.isComputer);

            Assert.AreEqual(19, GameManager.board.Height);
            Assert.AreEqual(19, GameManager.board.Width);
            Assert.AreNotEqual(null, GameManager.board.tiles);
            int half = GameManager.board.Width / 2;
            Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half));
        }

        [TestMethod]
        public void GameManager_BadInit() {
            GameManager.Initialize(19);

            Assert.AreEqual(19, GameManager.board.Height);
            Assert.AreEqual(19, GameManager.board.Width);
            Assert.AreNotEqual(null, GameManager.board.tiles);

            bool hasException = false;
            try {
                GameManager.Initialize(18); // Invalid
            } catch (ArgumentOutOfRangeException e) {
                hasException = true;
            }
            Assert.AreEqual(true, hasException);

            hasException = false;
            try {
                GameManager.Initialize(20); // Invalid
            } catch (ArgumentOutOfRangeException e) {
                hasException = true;
            }
            Assert.AreEqual(true, hasException);

            hasException = false;
            try {
                GameManager.Initialize(9); // Invalid
            } catch (ArgumentOutOfRangeException e) {
                hasException = true;
            }
            Assert.AreEqual(false, hasException);

            hasException = false;
            try {
                GameManager.Initialize(39); // Invalid
            } catch (ArgumentOutOfRangeException e) {
                hasException = true;
            }
            Assert.AreEqual(false, hasException);

            hasException = false;
            try {
                GameManager.Initialize(41); // Invalid
            } catch (ArgumentOutOfRangeException e) {
                hasException = true;
            }
            Assert.AreEqual(true, hasException);

            hasException = false;
            try {
                GameManager.Initialize(7); // Invalid
            } catch (ArgumentOutOfRangeException e) {
                hasException = true;
            }
            Assert.AreEqual(true, hasException);
        }

        [TestMethod]
        public void GameManager_GameLoop() {
            GameManager.Initialize(19);

            GameManager.SetPlayerNames("Josh", "Lucas");
            Assert.AreEqual(TileState.BLACK, GameManager.player1.color);
            Assert.AreEqual(TileState.WHITE, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            
            Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half));

            GameManager.PlacePiece(half-1, half-1, out temp);
            Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half-1, half-1));

            GameManager.PlacePiece(half-4, half, out temp);
            Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half-4, half));

            GameManager.PlacePiece(half - 3, half - 3, out temp);
            Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 3, half - 3));
        }

        [TestMethod]
        public void GameManager_PlayerName() {
            GameManager.Initialize(19);

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
            Assert.AreEqual(false, GameManager.player2.isComputer);

            GameManager.SetPlayerNames("Josh", "Computer", true);
            Assert.AreEqual("Josh", GameManager.player1.name);
            Assert.AreEqual("Computer", GameManager.player2.name);
            Assert.AreEqual(true, GameManager.player2.isComputer);
        }

        [TestMethod]
        public void GameManager_Computer() {
            GameManager.Initialize(19);

            GameManager.SetPlayerNames("Josh", "Computer", true);
            Assert.AreEqual("Josh", GameManager.player1.name);
            Assert.AreEqual("Computer", GameManager.player2.name);
            Assert.AreEqual(true, GameManager.player2.isComputer);

            string temp;
            int half = GameManager.board.Width / 2;

            // Player First turn
            
            GameManager.MakeComputerMove(out temp);

            int blackAmo = 0;
            for(int i = 0; i < GameManager.board.Height; i++) {
                for(int y = 0; y < GameManager.board.Width; y++) {
                    if(GameManager.board.GetState(y, i) == TileState.WHITE) {
                        blackAmo++;
                    }
                }
            }
            Assert.AreEqual(1, blackAmo);
            Assert.AreEqual(true, GameManager.player1Turn);

            // Player second Turn
            GameManager.PlacePiece(half + 4, half, out temp);
            GameManager.MakeComputerMove(out temp);

            blackAmo = 0;
            for (int i = 0; i < GameManager.board.Height; i++) {
                for (int y = 0; y < GameManager.board.Width; y++) {
                    if (GameManager.board.GetState(y, i) == TileState.WHITE) {
                        blackAmo++;
                    }
                }
            }
            Assert.AreEqual(2, blackAmo);
            Assert.AreEqual(true, GameManager.player1Turn);
        }

        [TestMethod]
        public void GameManager_MultipleCaptures() {
            GameManager.Initialize(19);
            Assert.AreEqual(TileState.BLACK, GameManager.player1.color);
            Assert.AreEqual(TileState.WHITE, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            // Multiple Captures Vertical Up, Horizontal Right, Diagonal Down Right
            {
                GameManager.Initialize(19);
                
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half, half - 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half - 1)); // Vertical Setup

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
                }

                GameManager.PlacePiece(half, half + 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 1)); // Vertical Upp

                GameManager.PlacePiece(half + 3, half - 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 3, half - 1)); // Diagnol Setup

                GameManager.PlacePiece(half + 1, half + 2, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 1, half + 2)); // Horizontal Right

                GameManager.PlacePiece(half + 3, half + 2, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 3, half + 2)); // Horizontal Setup

                GameManager.PlacePiece(half + 2, half + 2, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 2, half + 2)); // Horizontal Right

                GameManager.PlacePiece(half - 1, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half -1, half)); // Random Black

                GameManager.PlacePiece(half + 2, half, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 2, half)); // Diagonal

                GameManager.PlacePiece(half - 2, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 2, half)); // Random Black

                GameManager.PlacePiece(half + 1, half + 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 1, half + 1)); // Diagonal

                GameManager.PlacePiece(half, half + 2, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 2)); // Finish

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
            GameManager.Initialize(19);
            Assert.AreEqual(TileState.BLACK, GameManager.player1.color);
            Assert.AreEqual(TileState.WHITE, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;



            // Horizontal Left (-, )
            {
                GameManager.Initialize(19);
                
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half + 1, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 1, half)); // Opposite direction

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
                }

                GameManager.PlacePiece(half - 1, half, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 1, half)); // Current direction

                GameManager.PlacePiece(half - 2, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 2, half)); // Capture

                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half - 1, half));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half));
            }
            // Horizontal Right (+, )
            {
                GameManager.Initialize(19);
                
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half - 1, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 1, half)); // Opposite direction

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
                }

                GameManager.PlacePiece(half + 1, half, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 1, half)); // Current direction

                GameManager.PlacePiece(half + 2, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 2, half)); // Capture

                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half + 1, half));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half));
            }
        }

        [TestMethod]
        public void GameManager_VerticalCapture() {
            GameManager.Initialize(19);
            Assert.AreEqual(TileState.BLACK, GameManager.player1.color);
            Assert.AreEqual(TileState.WHITE, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            // Vertical Up ( , +)
            {
                GameManager.Initialize(19);
                
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half, half - 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half - 1)); // Opposite direction

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6)); 
                }


                GameManager.PlacePiece(half, half + 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 1)); // Current direction

                GameManager.PlacePiece(half, half + 2, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 2)); // Capture

                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half + 1));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half));
            }
            // Vertical Down ( , -)
            {
                GameManager.Initialize(19);
                
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half, half + 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1)); // Opposite direction

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
                }

                GameManager.PlacePiece(half, half - 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half - 1)); // Current direction

                GameManager.PlacePiece(half, half - 2, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half - 2)); // Capture

                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half - 1));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half));
            }

        }

        [TestMethod]
        public void GameManager_DiagonalCapture() {
            GameManager.Initialize(19);
            Assert.AreEqual(TileState.BLACK, GameManager.player1.color);
            Assert.AreEqual(TileState.WHITE, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            // Diagonal Down, Left (-, -)
            {
                GameManager.Initialize(19);
                
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half + 1, half + 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 1, half + 1)); // Opposite direction

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
                }

                GameManager.PlacePiece(half - 1, half - 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 1, half - 1)); // Current direction

                GameManager.PlacePiece(half - 2, half - 2, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 2, half - 2)); // Capture

                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half - 1, half - 1));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half));
            }
            // Diagonal Down, Right (+, -)
            {
                GameManager.Initialize(19);
                
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half - 1, half + 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 1, half + 1)); // Opposite direction

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
                }

                GameManager.PlacePiece(half + 1, half - 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 1, half - 1)); // Current direction

                GameManager.PlacePiece(half + 2, half - 2, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 2, half - 2)); // Capture

                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half + 1, half - 1));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half));
            }
            // Diagonal Up, Left (-, +)
            {
                GameManager.Initialize(19);
                
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half + 1, half - 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 1, half - 1)); // Opposite direction

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
                }

                GameManager.PlacePiece(half - 1, half + 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 1, half + 1)); // Current direction

                GameManager.PlacePiece(half - 2, half + 2, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 2, half + 2)); // Capture

                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half - 1, half + 1));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half));
            }
            // Diagonal Up, Right (+, +)
            {
                GameManager.Initialize(19);
                
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half - 1, half - 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 1, half - 1)); // Opposite direction

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
                }

                GameManager.PlacePiece(half + 1, half + 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 1, half + 1)); // Current direction

                GameManager.PlacePiece(half + 2, half + 2, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 2, half + 2)); // Capture

                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half + 1, half + 1));
                Assert.AreEqual(TileState.EMPTY, GameManager.board.GetState(half, half));
            }
        }

        [TestMethod]
        public void GameManager_WhitePlacementIntoCapture() {
            GameManager.Initialize(19);
            Assert.AreEqual(TileState.BLACK, GameManager.player1.color);
            Assert.AreEqual(TileState.WHITE, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            // Vertical
            {
                GameManager.Initialize(19);
                
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half, half + 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 1)); // Up

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
                }

                GameManager.PlacePiece(half + 1, half, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 1, half)); // Random

                GameManager.PlacePiece(half, half - 2, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half - 2)); // Down 2

                GameManager.PlacePiece(half, half - 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half - 1)); // Placement


                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half - 1));
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half));
            }

            // Horizontal
            {
                GameManager.Initialize(19);
                
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half + 1, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 1, half)); // Right

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
                }

                GameManager.PlacePiece(half, half + 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 1)); // Random

                GameManager.PlacePiece(half - 2, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 2, half)); // Left 2

                GameManager.PlacePiece(half - 1, half, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 1, half)); // Placement


                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 1, half));
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half));
            }

            // Diagonal
            {
                GameManager.Initialize(19);
                
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half - 1, half + 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 1, half + 1)); // Left 1, Up 1

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
                }

                GameManager.PlacePiece(half, half + 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 1)); // Random

                GameManager.PlacePiece(half + 2, half - 2, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half + 2, half - 2)); // Down 2 Right 2

                GameManager.PlacePiece(half + 1, half - 1, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 1, half - 1)); // Placement


                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 1, half - 1));
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half));
            }
        }

        [TestMethod]
        public void GameManager_BlackPlacementIntoCapture() {
            GameManager.Initialize(19);
            Assert.AreEqual(TileState.BLACK, GameManager.player1.color);
            Assert.AreEqual(TileState.WHITE, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            // Vertical
            {
                GameManager.Initialize(19);
                
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half, half - 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half - 1)); // Down

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
                }

                GameManager.PlacePiece(half, half - 3, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half - 3)); // Placment

                GameManager.PlacePiece(half, half - 2, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half - 2)); // Down 2

                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half - 1));
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half - 2));
            }

            // Horizontal
            {
                GameManager.Initialize(19);
                
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half - 1, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 1, half)); // Down

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
                }

                GameManager.PlacePiece(half - 3, half, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 3, half)); // Placment

                GameManager.PlacePiece(half - 2, half, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 2, half)); // Down 2

                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 1, half));
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 2, half));
            }

            // Diagnol
            {
                GameManager.Initialize(19);
                
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half)); // Middle

                GameManager.PlacePiece(half - 1, half - 1, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 1, half - 1)); // Down

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
                }

                GameManager.PlacePiece(half - 3, half - 3, out temp);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 3, half - 3)); // Placment

                GameManager.PlacePiece(half - 2, half - 2, out temp);
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 2, half - 2)); // Down 2

                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 1, half - 1));
                Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half - 2, half - 2));
            }
        }

        [TestMethod]
        public void GameManager_ValidPlacement() {
            GameManager.Initialize(19);
            Assert.AreEqual(TileState.BLACK, GameManager.player1.color);
            Assert.AreEqual(TileState.WHITE, GameManager.player2.color);

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
                
            } catch (IndexOutOfRangeException e) {
                hasException = true;
            }
            Assert.AreEqual(false, hasException);

            // Onto itself
            hasException = false;
            try {
                
            } catch (IndexOutOfRangeException e) {
                hasException = true;
            }
            Assert.AreEqual(false, hasException);
            Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half));
            Assert.AreEqual(false, GameManager.player1Turn); // Still Player2's turn
            // Assert Turn Amo
        }

        [TestMethod]
        public void GameManager_WinPlacementBlack() {
            GameManager.Initialize(19);
            Assert.AreEqual(TileState.BLACK, GameManager.player1.color);
            Assert.AreEqual(TileState.WHITE, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            // Horizontal
            {
                GameManager.Initialize(19);

                
                //Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half, half + 1, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 4, half, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 4, half + 1, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 1, half, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 1, half + 1, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 2, half, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 2, half + 1, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 5, half, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 3, half + 1, out temp);
                Assert.AreEqual("Pente", temp);
            }

            // Vertical
            {
                GameManager.Initialize(19);

                
                //Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 1, half, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half, half + 4, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 1, half + 4, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half, half + 1, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 1, half + 1, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half, half + 2, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 1, half + 2, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half, half + 5, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 1, half + 3, out temp);
                Assert.AreEqual("Pente", temp);
            }

            // Diagonal
            {
                GameManager.Initialize(19);

                
                //Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 1, half + 1, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half - 5, half, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 2, half + 2, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half - 3, half, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 3, half + 3, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half - 2, half, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 4, half + 4, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half - 1, half, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 5, half + 5, out temp);
                Assert.AreEqual("Pente", temp);
            }
        }

        [TestMethod]
        public void GameManager_WinPlacementWhite() {
            GameManager.Initialize(19);
            Assert.AreEqual(TileState.BLACK, GameManager.player1.color);
            Assert.AreEqual(TileState.WHITE, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            // Horizontal
            {
                GameManager.Initialize(19);

                
                //Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half, half + 1, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 4, half, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 4, half + 1, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 1, half, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 1, half + 1, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 2, half, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 2, half + 1, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 3, half, out temp);
                Assert.AreEqual("Pente", temp);
            }

            // Vertical
            {
                GameManager.Initialize(19);

                
                //Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 1, half, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half, half + 4, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 1, half + 4, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half, half + 1, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 1, half + 1, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half, half + 2, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 1, half + 2, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half, half + 3, out temp);
                Assert.AreEqual("Pente", temp);
            }

            // Diagonal
            {
                GameManager.Initialize(19);

                
                //Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half - 1, half, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 4, half + 4, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half - 1, half - 4, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 1, half + 1, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half - 1, half - 1, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 2, half + 2, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half - 1, half - 2, out temp);
                Assert.AreNotEqual("Pente", temp);

                GameManager.PlacePiece(half + 3, half + 3, out temp);
                Assert.AreEqual("Pente", temp);
            }
        }

        [TestMethod]
        public void GameManager_WinCaptureWhite() {
            GameManager.Initialize(19);
            Assert.AreEqual(TileState.BLACK, GameManager.player1.color);
            Assert.AreEqual(TileState.WHITE, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            GameManager.player1.captures = 4;

            
            //Assert.AreNotEqual("Capture", temp);

            GameManager.PlacePiece(half + 1, half, out temp);
            Assert.AreNotEqual("Capture", temp);

            GameManager.PlacePiece(half + 4, half, out temp);
            Assert.AreNotEqual("Capture", temp);

            GameManager.PlacePiece(half + 2, half, out temp);
            Assert.AreNotEqual("Capture", temp);

            GameManager.PlacePiece(half + 3, half, out temp);
            Assert.AreEqual("Capture", temp);
        }

        [TestMethod]
        public void GameManager_WinCaptureBlack() {
            GameManager.Initialize(19);
            Assert.AreEqual(TileState.BLACK, GameManager.player1.color);
            Assert.AreEqual(TileState.WHITE, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            GameManager.player2.captures = 4;

            
            //Assert.AreNotEqual("Capture", temp);

            GameManager.PlacePiece(half + 1, half , out temp);
            Assert.AreNotEqual("Capture", temp);

            GameManager.PlacePiece(half - 3, half, out temp);
            Assert.AreNotEqual("Capture", temp);

            GameManager.PlacePiece(half + 2, half, out temp);
            Assert.AreNotEqual("Capture", temp);

            GameManager.PlacePiece(half - 1, half, out temp);
            Assert.AreNotEqual("Capture", temp);

            GameManager.PlacePiece(half - 2, half, out temp);
            Assert.AreEqual("Capture", temp);
        }

        [TestMethod]
        public void GameManager_DrawCheck() {
            GameManager.Initialize(9);
            Assert.AreEqual(TileState.BLACK, GameManager.player1.color);
            Assert.AreEqual(TileState.WHITE, GameManager.player2.color);

            string temp;
            int width = GameManager.board.Width;
            int half = GameManager.board.Width / 2;
            int start = 0;


            //Assert.AreNotEqual("Pente", temp);


            // Top 8 rows
            for (int i = 0; i < 4; i++) {
                start = 0;
                for (int y = 0; y < 4; y++) {
                    GameManager.PlacePiece(i * 2, start + y, out temp); // White
                    Assert.AreNotEqual("Draw", temp);
                    Assert.AreNotEqual("Pente", temp);

                    GameManager.PlacePiece(i * 2 + 1, start + y, out temp); // Black
                    Assert.AreNotEqual("Draw", temp);
                    Assert.AreNotEqual("Pente", temp);
                }
            }

            // Bottom 8 rows
            for (int i = 0; i < 4; i++) {
                start = 5;
                for (int y = 0; y < 4; y++) {
                    GameManager.PlacePiece(i * 2, start + y, out temp); // White
                    Assert.AreNotEqual("Draw", temp);
                    Assert.AreNotEqual("Pente", temp);

                    GameManager.PlacePiece(i * 2 + 1, start + y, out temp); // Black
                    Assert.AreNotEqual("Draw", temp);
                    Assert.AreNotEqual("Pente", temp);
                }
            }

            GameManager.PlacePiece(1, half, out temp); // White
            Assert.AreNotEqual("Draw", temp);
            Assert.AreNotEqual("Pente", temp);
            GameManager.PlacePiece(0, half, out temp); // Black
            Assert.AreNotEqual("Draw", temp);
            Assert.AreNotEqual("Pente", temp);
            GameManager.PlacePiece(3, half, out temp); // White
            Assert.AreNotEqual("Draw", temp);
            Assert.AreNotEqual("Pente", temp);
            GameManager.PlacePiece(2, half, out temp); // Black
            Assert.AreNotEqual("Draw", temp);
            Assert.AreNotEqual("Pente", temp);
            GameManager.PlacePiece(5, half, out temp); // White
            Assert.AreNotEqual("Draw", temp);
            Assert.AreNotEqual("Pente", temp);
            GameManager.PlacePiece(6, half, out temp); // Black
            Assert.AreNotEqual("Draw", temp);
            Assert.AreNotEqual("Pente", temp);
            GameManager.PlacePiece(7, half, out temp); // White
            Assert.AreNotEqual("Draw", temp);
            Assert.AreNotEqual("Pente", temp);


            for (int y = 0; y < 9; y++) {
                GameManager.PlacePiece(width - 1, y, out temp); // White
                if(y == 8) {
                    Assert.AreEqual("Draw", temp);
                } else {
                    Assert.AreNotEqual("Draw", temp);
                    Assert.AreNotEqual("Pente", temp);
                }
                
            }


            //start = 5;
            //for (int y = 0; y < 4; y++) {
            //    GameManager.PlacePiece(i * 2, start + y, out temp); // White
            //    Assert.AreNotEqual("Draw", temp);
            //    Assert.AreNotEqual("Pente", temp);

            //    GameManager.PlacePiece(i * 2 + 1, start + y, out temp); // Black
            //    Assert.AreNotEqual("Draw", temp);
            //    Assert.AreNotEqual("Pente", temp);
            //}

            //GameManager.PlacePiece(i * 2 + 1, 4, out temp); // White 
            //Assert.AreNotEqual("Draw", temp);
            //Assert.AreNotEqual("Pente", temp);
            //GameManager.PlacePiece(i * 2, 4, out temp); // Black
            //Assert.AreNotEqual("Draw", temp);
            //Assert.AreNotEqual("Pente", temp);
            //}




        }

        [TestMethod]
        public void GameManager_Tria() {
            GameManager.Initialize(19);
            Assert.AreEqual(TileState.BLACK, GameManager.player1.color);
            Assert.AreEqual(TileState.WHITE, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            // Both
            {
                GameManager.Initialize(19);
                // Before
                 // White
                //Assert.AreEqual("", temp);

                GameManager.PlacePiece(half + 1, half, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
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
                GameManager.Initialize(19);
                // Before
                 // White
                //Assert.AreEqual("", temp);

                GameManager.PlacePiece(half, half - 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
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
                GameManager.Initialize(19);
                // Before
                 // White
                //Assert.AreEqual("", temp);

                GameManager.PlacePiece(half, half + 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
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
                GameManager.Initialize(19);
                // Before
                 // White
                //Assert.AreEqual("", temp);

                GameManager.PlacePiece(half, half - 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
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
                GameManager.Initialize(19);
                // Before
                 // White
                //Assert.AreEqual("", temp);

                GameManager.PlacePiece(half + 1, half, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
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
                GameManager.Initialize(19);
                // Before
                 // White
                //Assert.AreEqual("", temp);

                GameManager.PlacePiece(half, half + 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
                }

                GameManager.PlacePiece(half + 1, half, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half + 1, half + 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Is
                GameManager.PlacePiece(half + 3, half, out temp); // White
                Assert.AreEqual("Tria", temp);
            }

            // Almost Tessera Right
            {
                GameManager.Initialize(19);
                // Before
                 // White
                //Assert.AreEqual("", temp);

                GameManager.PlacePiece(half, half + 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
                }

                GameManager.PlacePiece(half - 1, half, out temp); // White
                Assert.AreEqual("", temp);

                GameManager.PlacePiece(half - 1, half + 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Is
                GameManager.PlacePiece(half - 3, half, out temp); // White
                Assert.AreEqual("Tria", temp);
            }

            // Almost Tessera Diagonal
            {
                GameManager.Initialize(19);
                // Before
                 // White
                //Assert.AreEqual("", temp);

                GameManager.PlacePiece(half -1, half, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
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
            GameManager.Initialize(19);
            Assert.AreEqual(TileState.BLACK, GameManager.player1.color);
            Assert.AreEqual(TileState.WHITE, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;

            // Empty Both Sides
            {
                GameManager.Initialize(19);
                // Before
                 // White
                //Assert.AreEqual("", temp);

                GameManager.PlacePiece(half + 1, half, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
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
                GameManager.Initialize(19);
                // Before
                 // White
                //Assert.AreEqual("", temp);

                GameManager.PlacePiece(half, half + 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
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
                GameManager.Initialize(19);
                // Before
                 // White
                //Assert.AreEqual("", temp);

                GameManager.PlacePiece(half, half - 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
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
                GameManager.Initialize(19);
                // Before
                 // White
                //Assert.AreEqual("", temp);

                GameManager.PlacePiece(half, half + 1, out temp); // Black
                Assert.AreEqual("", temp);

                // Random Second Turn
                {
                    GameManager.PlacePiece(half, half + 5, out temp);
                    Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 5));

                    GameManager.PlacePiece(half, half + 6, out temp);
                    Assert.AreEqual(TileState.WHITE, GameManager.board.GetState(half, half + 6));
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
            GameManager.Initialize(19);
            Assert.AreEqual(TileState.BLACK, GameManager.player1.color);
            Assert.AreEqual(TileState.WHITE, GameManager.player2.color);

            int half = GameManager.board.Width / 2;

            GameManager.Initialize(19);
            Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half));
            Assert.AreEqual(false, GameManager.player1Turn);
            
        }

        [TestMethod]
        public void GameManager_MoveLimitation() {
            GameManager.Initialize(19);
            Assert.AreEqual(TileState.BLACK, GameManager.player1.color);
            Assert.AreEqual(TileState.WHITE, GameManager.player2.color);

            string temp;
            int half = GameManager.board.Width / 2;
            // Distance is a Square Grid

            // InValid Placements
            { 
                // Right
                bool hasException = false;
                try {
                    GameManager.Initialize(19);
                    
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
                    GameManager.Initialize(19);
                    
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
                    GameManager.Initialize(19);
                    
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
                    GameManager.Initialize(19);
                    
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
                    GameManager.Initialize(19);
                    
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
                    GameManager.Initialize(19);
                    
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
                    GameManager.Initialize(19);
                    
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
                    GameManager.Initialize(19);
                    
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
                    GameManager.Initialize(19);
                    
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half + 3, half, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 3, half));
                Assert.AreEqual(false, GameManager.player1Turn);

                // Left
                hasException = false;
                try {
                    GameManager.Initialize(19);
                    
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half - 3, half, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 3, half));
                Assert.AreEqual(false, GameManager.player1Turn);

                // Up
                hasException = false;
                try {
                    GameManager.Initialize(19);
                    
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half, half + 3, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half + 3));
                Assert.AreEqual(false, GameManager.player1Turn);

                // Down
                hasException = false;
                try {
                    GameManager.Initialize(19);
                    
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half, half - 3, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half, half - 3));
                Assert.AreEqual(false, GameManager.player1Turn);

                // Down Right
                hasException = false;
                try {
                    GameManager.Initialize(19);
                    
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half + 3, half - 3, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 3, half - 3));
                Assert.AreEqual(false, GameManager.player1Turn);

                // Down Left
                hasException = false;
                try {
                    GameManager.Initialize(19);
                    
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half - 3, half - 3, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 3, half - 3));
                Assert.AreEqual(false, GameManager.player1Turn);

                // Up Left
                hasException = false;
                try {
                    GameManager.Initialize(19);
                    
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half - 3, half + 3, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half - 3, half + 3));
                Assert.AreEqual(false, GameManager.player1Turn);

                // Up Right
                hasException = false;
                try {
                    GameManager.Initialize(19);
                    
                    GameManager.PlacePiece(half + 1, half + 1, out temp);
                    GameManager.PlacePiece(half + 3, half + 3, out temp);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(false, hasException);
                Assert.AreEqual(TileState.BLACK, GameManager.board.GetState(half + 3, half + 3));
                Assert.AreEqual(false, GameManager.player1Turn);
            }
        }

        [TestMethod]
        public void GameManager_CanSave() {
            GameManager.Initialize(19);

            string temp;
            int half = GameManager.board.Width / 2;

            
            GameManager.PlacePiece(half + 1, half, out temp);
            GameManager.PlacePiece(half + 4, half, out temp);

            GameManager.Serialize("CanSave.sav");
        }

        [TestMethod]
        public void GameManager_CanLoad() {
            GameManager.Deserialize("CanSave.sav");
        }

        [TestMethod]
        public void GameManager_SaveLoad() {
            GameManager.Initialize(19);

            Player player1;
            Player player2;
            bool player1Turn;
            bool boardLocked;
            TileState[,] tiles;
            string temp;
            int half = GameManager.board.Width / 2;

            // Empty Board
            {
                GameManager.Initialize(19);
                Save(out player1, out player2, out player1Turn, out tiles, out boardLocked);

                GameManager.Serialize("Save01.sav");
                GameManager.Initialize(19);
                GameManager.Deserialize("Save01.sav");

                AssertSaveLoad(player1, player2, player1Turn, tiles, boardLocked);
            }

            // Basic Board
            {
                GameManager.Initialize(19);
                
                GameManager.PlacePiece(half - 1, half - 1, out temp);
                GameManager.PlacePiece(half - 4, half - 3, out temp);
                Save(out player1, out player2, out player1Turn, out tiles, out boardLocked);

                GameManager.Serialize("Save02.sav");
                GameManager.Initialize(19);
                GameManager.Deserialize("Save02.sav");

                AssertSaveLoad(player1, player2, player1Turn, tiles, boardLocked);
            }

            // Capture Board
            {
                GameManager.Initialize(19);
                
                GameManager.PlacePiece(half - 1, half, out temp);

                GameManager.PlacePiece(half - 4, half, out temp);
                GameManager.PlacePiece(half - 5, half - 5, out temp);

                GameManager.PlacePiece(half + 1, half, out temp);
                GameManager.PlacePiece(half + 2, half, out temp); // Should Capture
                Save(out player1, out player2, out player1Turn, out tiles, out boardLocked);

                GameManager.Serialize("Save03.sav");
                GameManager.Initialize(19);
                GameManager.Deserialize("Save03.sav");

                AssertSaveLoad(player1, player2, player1Turn, tiles, boardLocked);
            }

            // Computer Board
            {
                GameManager.Initialize(19);
                GameManager.SetPlayerNames("Josh", "Computer");
                
                GameManager.MakeComputerMove(out temp);
                //GameManager.PlacePiece(half - 1, half, out temp);

                GameManager.PlacePiece(half - 4, half, out temp);
                GameManager.MakeComputerMove(out temp);
                //GameManager.PlacePiece(half - 5, half - 5, out temp);

                GameManager.PlacePiece(half + 1, half, out temp);
                GameManager.MakeComputerMove(out temp);
                //GameManager.PlacePiece(half + 2, half, out temp); // Should Capture
                Save(out player1, out player2, out player1Turn, out tiles, out boardLocked);

                GameManager.Serialize("Save04.sav");
                GameManager.Initialize(19);
                GameManager.Deserialize("Save04.sav");

                AssertSaveLoad(player1, player2, player1Turn, tiles, boardLocked);
            }
            // Win Board
            {
                GameManager.Initialize(19);
                GameManager.SetPlayerNames("Josh", "Computer");
                
                GameManager.PlacePiece(half - 1, half - 1, out temp);

                GameManager.PlacePiece(half + 4, half, out temp);
                GameManager.PlacePiece(half + 4, half - 1, out temp);

                GameManager.PlacePiece(half + 1, half, out temp);
                GameManager.PlacePiece(half + 1, half - 1, out temp); // Should Capture

                GameManager.PlacePiece(half + 2, half, out temp);
                GameManager.PlacePiece(half + 2, half - 1, out temp); // Should Capture

                GameManager.PlacePiece(half + 3, half, out temp);
                //GameManager.PlacePiece(half + 1, half - 1, out temp); // Should Capture
                Save(out player1, out player2, out player1Turn, out tiles, out boardLocked);

                GameManager.Serialize("Save05.sav");
                GameManager.Initialize(19);
                GameManager.Deserialize("Save05.sav");

                AssertSaveLoad(player1, player2, player1Turn, tiles, boardLocked);
            }
        }

        public void Save(out Player player1, out Player player2, out bool player1Turn, out TileState[,] tiles, out bool boradLocked) {
            tiles = new TileState[GameManager.board.Width, GameManager.board.Height];

            for (int i = 0; i < GameManager.board.Height; i++) {
                for (int y = 0; y < GameManager.board.Width; y++) {
                    tiles[y, i] = GameManager.board.GetState(y, i);
                }
            }

            player1Turn = GameManager.player1Turn;
            boradLocked = GameManager.BoardLocked;

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
        public void AssertSaveLoad(Player player1, Player player2, bool player1Turn, TileState[,] tiles, bool boardLocked) {
            Assert.AreEqual(player1.color, GameManager.player1.color);
            Assert.AreEqual(player1.captures, GameManager.player1.captures);
            Assert.AreEqual(player1.isComputer, GameManager.player1.isComputer);
            Assert.AreEqual(player1.name, GameManager.player1.name);

            Assert.AreEqual(player2.color, GameManager.player2.color);
            Assert.AreEqual(player2.captures, GameManager.player2.captures);
            Assert.AreEqual(player2.isComputer, GameManager.player2.isComputer);
            Assert.AreEqual(player2.name, GameManager.player2.name);

            Assert.AreEqual(player1Turn, GameManager.player1Turn);
            Assert.AreEqual(boardLocked, GameManager.BoardLocked);

            for (int i = 0; i < GameManager.board.Height; i++) {
                for (int y = 0; y < GameManager.board.Width; y++) {
                    Assert.AreEqual(tiles[y, i], GameManager.board.GetState(y, i));
                }
            }
        }
    }
}
