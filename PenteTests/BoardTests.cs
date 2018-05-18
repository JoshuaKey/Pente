using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pente;

namespace PenteTests {

    [TestClass]
    public class BoardTests {

        [TestMethod]
        public void Board_Placement() {
            Board b = new Board(19, 19);

            // Black
            for (int i = 0; i < b.Height; i++) {
                for (int y = 0; y < b.Width; y++) {
                    b.Place(TileState.BLACK, y, i);
                }
            }

            for (int i = 0; i < b.Height; i++) {
                for (int y = 0; y < b.Width; y++) {
                    Assert.AreEqual(TileState.BLACK, b.GetState(y, i));
                }
            }

            for (int i = 0; i < b.Height; i++) {
                for (int y = 0; y < b.Width; y++) {
                    b.Remove(y, i);
                }
            }

            // White
            for (int i = 0; i < b.Height; i++) {
                for (int y = 0; y < b.Width; y++) {
                    b.Place(TileState.WHITE, y, i);
                }
            }

            for (int i = 0; i < b.Height; i++) {
                for (int y = 0; y < b.Width; y++) {
                    Assert.AreEqual(TileState.WHITE, b.GetState(y, i));
                }
            }
        }

        [TestMethod]
        public void Board_BadPlacement() {
            Board b = new Board(19, 19);

            b.Place(TileState.WHITE, 1, 1);
            b.Place(TileState.BLACK, 1, 1);
            // What happens when we place on top of piece
        }

        [TestMethod]
        public void Board_OutOfIndex() {
            Board b = new Board(19, 19);

            bool hasException = false;
            // Placement
            {
                try {
                    b.Place(TileState.WHITE, 100, 100);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(true, hasException);

                hasException = false;
                try {
                    b.Place(TileState.WHITE, -1, -1);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(true, hasException);

                hasException = false;
                try {
                    b.Place(TileState.BLACK, 100, 100);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(true, hasException);

                hasException = false;
                try {
                    b.Place(TileState.BLACK, -1, -1);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(true, hasException);
            }

            // Check
            {
                hasException = false;
                try {
                    b.Check(-1, -1);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(true, hasException);

                hasException = false;
                try {
                    b.Check(100, 100);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(true, hasException);
            }

            //Get State
            {
                hasException = false;
                try {
                    b.GetState(100, 100);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(true, hasException);

                hasException = false;
                try {
                    b.GetState(-1, -1);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(true, hasException);
            }

            // Remove
            {
                hasException = false;
                try {
                    b.Remove(-1, -1);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(true, hasException);

                hasException = false;
                try {
                    b.Remove(100, 100);
                } catch (IndexOutOfRangeException e) {
                    hasException = true;
                }
                Assert.AreEqual(true, hasException);
            }
        }

        [TestMethod]
        public void Board_Remove() {
            Board b = new Board(19, 19);

            for (int i = 0; i < b.Height; i++) {
                for (int y = 0; y < b.Width; y++) {
                    b.Place(TileState.BLACK, y, i);
                }
            }

            for (int i = 0; i < b.Height; i++) {
                for (int y = 0; y < b.Width; y++) {
                    b.Remove(y, i);
                }
            }

            for (int i = 0; i < b.Height; i++) {
                for (int y = 0; y < b.Width; y++) {
                    Assert.AreEqual(true, b.Check(y, i));
                }
            }
        }

        [TestMethod]
        public void Board_Check() {
            Board b = new Board(19, 19);

            for (int i = 0; i < b.Height; i++) {
                for (int y = 0; y < b.Width; y++) {
                    Assert.AreEqual(true, b.Check(i, y));
                }
            }

            for (int i = 0; i < b.Height; i++) {
                for (int y = 0; y < b.Width; y++) {
                    b.Place(TileState.BLACK, y, i);
                }
            }

            for (int i = 0; i < b.Height; i++) {
                for (int y = 0; y < b.Width; y++) {
                    Assert.AreEqual(false, b.Check(i, y));
                }
            }
        }

        [TestMethod]
        public void Board_Clear() {
            // ????
        }

        [TestMethod]
        public void Board_BlackPlacement() {
            Board b = new Board(19, 19);

            // Black Placement
            b.Place(TileState.BLACK, 1, 1);

            Assert.AreEqual(TileState.BLACK, b.GetState(1, 1));

            Assert.AreEqual(false, b.Check(1, 1));

            b.Remove(1, 1);

            Assert.AreEqual(true, b.Check(1, 1));
        }

        [TestMethod]
        public void Board_WhitePlacement() {
            Board b = new Board(19, 19);

            // Black Placement
            b.Place(TileState.WHITE, 1, 1);

            Assert.AreEqual(TileState.WHITE, b.GetState(1, 1));

            Assert.AreEqual(false, b.Check(1, 1));

            b.Remove(1, 1);

            Assert.AreEqual(true, b.Check(1, 1));
        }

        [TestMethod]
        public void Board_EmptyPlacement() {
            Board b = new Board(19, 19);
            b.Place(TileState.EMPTY, 1, 1); // What to do when Place Empty?

            Assert.AreEqual(TileState.EMPTY, b.GetState(1, 1));

            Assert.AreEqual(true, b.Check(1, 1));

            b.Remove(1, 1); // Can Remove Element not there?

            Assert.AreEqual(true, b.Check(1, 1));
        }

        [TestMethod]
        public void Board_Initialize() {
            int width = 19;
            int height = 19;
            Board b = new Board(19, 19);

            // Init Board
            Assert.AreNotEqual(null, b.tiles);
            Assert.AreEqual(width, b.Width);
            Assert.AreEqual(height, b.Height);

            for(int i = 0; i < b.Height; i++) {
                for(int y = 0; y < b.Width; y++) {

                    Assert.AreEqual(true, b.Check(y, i));

                }
            }
        }
    }
}
