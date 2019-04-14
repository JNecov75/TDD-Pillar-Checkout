using Microsoft.VisualStudio.TestTools.UnitTesting;
using PointOfSale.Library;

namespace PointOfSale.Tests
{
    [TestClass]
    public class ShopTests
    {
        private Shop _shop;

        public ShopTests()
        {
            _shop = new Shop();
        }

        [TestMethod]
        public void WhenAnItemIsAddedToInventoryItStoresTheName()
        {
            _shop.AddToInventory(new Item("soup"));
            _shop.AddToInventory(new Item("ground beef"));
            Assert.AreEqual("soup", _shop.Inventory[0].Name);
            Assert.AreEqual("ground beef", _shop.Inventory[1].Name);
        }

        [TestMethod]
        public void WhenAnItemIsAddedToInventoryItStoresThePrice()
        {
            _shop.AddToInventory(new Item("", 1.89));
            _shop.AddToInventory(new Item("", 5.99));
            Assert.AreEqual(1.89, _shop.Inventory[0].Price);
            Assert.AreEqual(5.99, _shop.Inventory[1].Price);
        }

        [TestMethod]
        public void InventoryCanBeInitializedWithASetOfProducts()
        {
            _shop.InitializeInventory();
            Assert.AreEqual(2, _shop.Inventory.Count);
        }

        [TestMethod]
        public void WhenAnItemIsScannedAddItToTheCart()
        {
            _shop.InitializeInventory();
            _shop.ScanItem("soup");
            Assert.AreEqual("soup", _shop.Cart[0].Name);
            Assert.AreEqual(1.89, _shop.Cart[0].Price);
            Assert.AreEqual(1, _shop.Cart[0].Quantity);
        }
    }
}
