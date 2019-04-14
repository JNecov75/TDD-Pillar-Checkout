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

        //Inventory Tests
        [TestMethod]
        public void WhenAnItemIsAddedToInventoryItStoresTheName()
        {
            Assert.AreEqual("soup", _shop.Inventory.Find(x=> x.Name == "soup").Name);
            Assert.AreEqual("ground beef", _shop.Inventory.Find(x=> x.Name == "ground beef").Name);
        }

        [TestMethod]
        public void WhenAnItemIsAddedToInventoryItStoresThePrice()
        {
            Assert.AreEqual(1.89, _shop.Inventory.Find(x=> x.Name == "soup").Price);
            Assert.AreEqual(5.99, _shop.Inventory.Find(x=> x.Name == "ground beef").Price);
        }

        [TestMethod]
        public void InventoryCanBeInitializedWithASetOfProducts()
        {
            Assert.AreEqual(2, _shop.Inventory.Count);
        }

        [TestMethod]
        public void WhenAPriceChangesTheInventoryPriceUpdates()
        {
            Assert.AreEqual(1.89, _shop.Inventory.Find(x=> x.Name == "soup").Price);
            _shop.UpdatePrice("soup", 1.99);
            Assert.AreEqual(1.99, _shop.Inventory.Find(x=> x.Name == "soup").Price);
            Assert.AreEqual(5.99, _shop.Inventory.Find(x=> x.Name == "ground beef").Price);
            _shop.UpdatePrice("ground beef", 6.29);
            Assert.AreEqual(6.29, _shop.Inventory.Find(x=> x.Name == "ground beef").Price);
        }

        [TestMethod]
        public void WhenAnItemIsAddedToInventoryItStoresTheWeightForListedPrice()
        {
            Assert.AreEqual(1, _shop.Inventory.Find(x=> x.Name == "ground beef").Weight);
        }

        //Cart Tests
        [TestMethod]
        public void WhenAnItemIsScannedItAddsToTheCart()
        {
            _shop.ScanItem("soup");
            Assert.AreEqual("soup", _shop.Cart.Find(x=> x.Name == "soup").Name);
            Assert.AreEqual(1.89, _shop.Cart.Find(x=> x.Name == "soup").Price);
            Assert.AreEqual(1, _shop.Cart.Find(x=> x.Name == "soup").Quantity);
        }

        [TestMethod]
        public void WhenAnItemIsAddedToCartItIncrementsQuantity()
        {
            _shop.ScanItem("soup");
            Assert.AreEqual(1, _shop.Cart.Find(x=> x.Name == "soup").Quantity);
            _shop.ScanItem("soup");
            Assert.AreEqual(2, _shop.Cart.Find(x=> x.Name == "soup").Quantity);
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
            _shop.ScanItem("ground beef");
            Assert.AreEqual(9.77, _shop.GetCartTotal());
        }
    }
}
