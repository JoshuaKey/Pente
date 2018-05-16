using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pente;

namespace PenteTests {

    [TestClass]
    public class BoardTests {

        [TestMethod]
        public void Board_BlackPlacement() {
            Board b = new Board();

            // Black Placement
            b.Place(TileState.BLACK, 1, 1);

            Assert.AreEqual(TileState.BLACK, b.GetState(1, 1));

            Assert.AreEqual(false, b.Check(1, 1));

            b.Remove(1, 1);

            Assert.AreEqual(true, b.Check(1, 1));
        }

        [TestMethod]
        public void Board_WhitePlacement() {
            Board b = new Board();

            // Black Placement
            b.Place(TileState.WHITE, 1, 1);

            Assert.AreEqual(TileState.WHITE, b.GetState(1, 1));

            Assert.AreEqual(false, b.Check(1, 1));

            b.Remove(1, 1);

            Assert.AreEqual(true, b.Check(1, 1));
        }

        [TestMethod]
        public void Board_EmptyPlacement() {
            Board b = new Board();
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
            Board b = new Board();

            // Init Board
            Assert.AreEqual(width, b.width);
            Assert.AreEqual(height, b.height);

            for(int i = 0; i < b.height; i++) {
                for(int y = 0; y < b.width; y++) {

                    Assert.AreEqual(true, b.Check(y, i));

                }
            }
        }
    }
}
