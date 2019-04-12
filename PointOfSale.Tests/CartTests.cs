using Microsoft.VisualStudio.TestTools.UnitTesting;
using PointOfSale.Library;

namespace PointOfSale.Tests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void WhenAnItemIsAddedItStoresTheName()
        {
            Cart cart = new Cart();
            cart.Add(new Item("soup"));
            cart.Add(new Item("ground beef"));
            Assert.AreEqual("soup", cart.Items[0].Name);
            Assert.AreEqual("ground beef", cart.Items[1].Name);
        }
    }
}
