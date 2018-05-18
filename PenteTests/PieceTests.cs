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
            //Assert.AreNotEqual(null, p.BlackPieceImagePath);
            //Assert.AreNotEqual(null, p.EmptyPieceImagePath);
            //Assert.AreNotEqual(null, p.PiecePieceImagePath);

            //Assert.AreEqual(true, File.Exists(p.BlackPieceImagePath));
            //Assert.AreEqual(true, File.Exists(p.WhitePieceImagePath));
            //Assert.AreEqual(true, File.Exists(p.EmptyPieceImagePath));
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

            //Assert.AreEqual(p.WhitePieceImagePath, whiteImage.BaseUri);
            //Assert.AreEqual(p.BlackPieceImagePath, blackImage.BaseUri);
            //Assert.AreEqual(p.EmptyPieceImagePath, emptyImage.BaseUri);
        }

        [TestMethod]
        public void Piece_ConvertBack() {
            Piece p = new Piece();
            var converted = p.ConvertBack(null, null, null, null);
            Assert.AreEqual(null, converted);
        }

    }
}
