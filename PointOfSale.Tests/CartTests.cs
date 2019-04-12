using Microsoft.VisualStudio.TestTools.UnitTesting;
using PointOfSale.Library;

namespace PointOfSale.Tests
{
    [TestClass]
    public class ShopTests
    {
        [TestMethod]
        public void WhenAnItemIsAddedToInventoryItStoresTheName()
        {
            Shop shop = new Shop();
            shop.AddToInventory(new Item("soup"));
            shop.AddToInventory(new Item("ground beef"));
            Assert.AreEqual("soup", shop.Inventory[0].Name);
            Assert.AreEqual("ground beef", shop.Inventory[1].Name);
        }

        [TestMethod]
        public void WhenAnItemIsAddedToInventoryItStoresThePrice()
        {
            Shop shop = new Shop();
            shop.AddToInventory(new Item("", "$1.89"));
            shop.AddToInventory(new Item("", "$5.99"));
            Assert.AreEqual("$1.89", shop.Inventory[0].Price);
            Assert.AreEqual("$5.99", shop.Inventory[1].Price);
        }
    }
}
