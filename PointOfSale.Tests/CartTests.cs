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

        [TestMethod]
        public void WhenAnItemIsAddedItStoresThePrice()
        {
            Cart cart = new Cart();
            cart.Add(new Item("", "$1.89"));
            cart.Add(new Item("", "$5.99"));
            Assert.AreEqual("$1.89", cart.Items[0].Price);
            Assert.AreEqual("$5.99", cart.Items[1].Price);
        }
    }
}
