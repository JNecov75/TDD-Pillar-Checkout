using Microsoft.VisualStudio.TestTools.UnitTesting;
using PointOfSale.Library;

namespace PointOfSale.Tests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void WhenAnItemIsScannedItAddsToTheCart()
        {
            Cart cart = new Cart();
            Assert.AreEqual("soup", cart.items[0].name);
        }
    }
}
