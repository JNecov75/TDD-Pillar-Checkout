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


        [TestInitialize()]
        public void Startup() {
            _shop.InitializeInventory();
        }
        [TestMethod]
        public void WhenAnItemIsAddedToInventoryItStoresTheName()
        {
            Assert.AreEqual("soup", _shop.Inventory[0].Name);
            Assert.AreEqual("ground beef", _shop.Inventory[1].Name);
        }

        [TestMethod]
        public void WhenAnItemIsAddedToInventoryItStoresThePrice()
        {
            Assert.AreEqual(1.89, _shop.Inventory[0].Price);
            Assert.AreEqual(5.99, _shop.Inventory[1].Price);
        }

        [TestMethod]
        public void InventoryCanBeInitializedWithASetOfProducts()
        {
            Assert.AreEqual(2, _shop.Inventory.Count);
        }

        [TestMethod]
        public void WhenAnItemIsScannedAddItToTheCart()
        {
            _shop.ScanItem("soup");
            Assert.AreEqual("soup", _shop.Cart[0].Name);
            Assert.AreEqual(1.89, _shop.Cart[0].Price);
            Assert.AreEqual(1, _shop.Cart[0].Quantity);
        }

        [TestMethod]
        public void WhenAnItemIsAddedToCartItIncrementsQuantity()
        {
            _shop.ScanItem("soup");
            Assert.AreEqual(1, _shop.Cart[0].Quantity);
            _shop.ScanItem("soup");
            Assert.AreEqual(2, _shop.Cart[0].Quantity);
        }

        [TestMethod]
        public void WhenAnExistingItemIsAddedToCartItUpdatesItem()
        {
            _shop.ScanItem("soup");
            _shop.ScanItem("soup");
            Assert.AreEqual(1, _shop.Cart.Count);
        }

                [TestMethod]
        public void WhenAnItemIsAddedToCartItUpdatesTotalCost()
        {
            _shop.ScanItem("soup");
            _shop.ScanItem("soup");
            Assert.AreEqual(3.78, _shop.GetCartTotal());
        }
    }
}
