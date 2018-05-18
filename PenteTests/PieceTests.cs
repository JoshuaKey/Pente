using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pente;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PenteTests {

    [TestClass]
    public class PieceTests {

        [TestMethod]
        public void Piece_CanFindImages() {
            Piece p = new Piece();
            Assert.AreNotEqual(null, Piece.BlackPath);
            Assert.AreNotEqual(null, Piece.EmptyPath);
            Assert.AreNotEqual(null, Piece.WhitePath);

            Assert.AreEqual(true, File.Exists(Piece.BlackPath));
            Assert.AreEqual(true, File.Exists(Piece.WhitePath));
            Assert.AreEqual(true, File.Exists(Piece.EmptyPath));
        }

        [TestMethod]
        public void Piece_Convert() {
            Piece p = new Piece();

            var white = p.Convert(TileState.WHITE, null, null, null);
            var black = p.Convert(TileState.BLACK, null, null, null);
            var empty = p.Convert(TileState.EMPTY, null, null, null);

            Assert.AreNotEqual(null, white);
            Assert.AreNotEqual(null, black);
            Assert.AreNotEqual(null, empty);

            Assert.IsInstanceOfType(white, typeof(Image));
            Assert.IsInstanceOfType(black, typeof(Image));
            Assert.IsInstanceOfType(empty, typeof(Image));

            BitmapImage whiteImage = (BitmapImage)((Image)white).Source;
            BitmapImage blackImage = (BitmapImage)((Image)black).Source;
            BitmapImage emptyImage = (BitmapImage)((Image)empty).Source;


            Assert.AreEqual(Piece.WhitePath, whiteImage.BaseUri);
            Assert.AreEqual(Piece.BlackPath, blackImage.BaseUri);
            Assert.AreEqual(Piece.EmptyPath, emptyImage.BaseUri);
        }

        [TestMethod]
        public void Piece_ConvertBack() {
            Piece p = new Piece();
            var converted = p.ConvertBack(null, null, null, null);
            Assert.AreEqual(null, converted);
        }

    }
}
