using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pente;

namespace PenteTests {

    [TestClass]
    public class TileStateTests {

        [TestMethod]
        public void TileState_Names() {
            string[] names = Enum.GetNames(typeof(TileState));
            string[] expectedNames = new string[] { "WHITE", "BLACK", "EMPTY" };

            Assert.AreEqual(3, names.Length);

            for(int i = 0; i < names.Length; i++) {
                Assert.AreEqual(expectedNames[i], names[i]);
            }
        }
    }
}
