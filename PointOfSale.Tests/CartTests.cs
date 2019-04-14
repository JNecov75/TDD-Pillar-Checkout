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
        public void WhenAMarkdownIsAppliedItIsReflectedInInventoryMarkdown()
        {
            Assert.AreEqual(0, _shop.Inventory.Find(x=> x.Name == "soup").Markdown);
            _shop.UpdateMarkdown("soup", 25);
            Assert.AreEqual(.25, _shop.Inventory.Find(x=> x.Name == "soup").Markdown);
            _shop.UpdateMarkdown("soup", 0);
            Assert.AreEqual(0, _shop.Inventory.Find(x=> x.Name == "soup").Markdown);
        }

        //Cart Tests
        [TestMethod]
        public void WhenAnItemIsScannedItAddsToTheCart()
        {
            _shop.ScanItem("soup");
            Assert.AreEqual("soup", _shop.Cart.Find(x=> x.Name == "soup").Name);
            Assert.AreEqual(1.89, _shop.Cart.Find(x=> x.Name == "soup").Price);
            Assert.AreEqual(1, _shop.Cart.Find(x=> x.Name == "soup").UnitCount);
        }

        [TestMethod]
        public void WhenAnItemIsAddedToCartItIncrementsQuantity()
        {
            _shop.ScanItem("soup");
            Assert.AreEqual(1, _shop.Cart.Find(x=> x.Name == "soup").UnitCount);
            _shop.ScanItem("soup");
            Assert.AreEqual(2, _shop.Cart.Find(x=> x.Name == "soup").UnitCount);
        }

        [TestMethod]
        public void WhenAnExistingItemIsAddedToCartItUpdatesItem()
        {
            _shop.ScanItem("soup");
            _shop.ScanItem("soup");
            Assert.AreEqual(1, _shop.Cart.Count);
        }

        [TestMethod]
        public void WhenAnItemWithWeightIsAddedToCartItUpdatesTotalCostOfItem()
        {
            _shop.ScanItem("ground beef", 1);
            Assert.AreEqual(5.99 , _shop.GetCartTotal("ground beef"));
            _shop.ScanItem("ground beef", 0.5);
            Assert.AreEqual(8.98 , _shop.GetCartTotal("ground beef"));
        }

        [TestMethod]
        public void WhenAnItemIsAddedToCartItUpdatesTotalCost()
        {
            _shop.ScanItem("soup");
            _shop.ScanItem("soup");
            _shop.ScanItem("ground beef");
            Assert.AreEqual(9.77, _shop.GetCartTotal());
        }

        [TestMethod]
        public void WhenAMarkdownIsAppliedItIsReflectedInCartItemPrice()
        {
            _shop.ScanItem("soup");
            _shop.UpdateMarkdown("soup", 25);
            Assert.AreEqual(1.42, _shop.GetCartTotal("soup"));
            _shop.ScanItem("soup");
            Assert.AreEqual(2.84, _shop.GetCartTotal("soup"));
            _shop.ScanItem("ground beef");
            _shop.UpdateMarkdown("ground beef", 50);
            Assert.AreEqual(5.84, _shop.GetCartTotal());
        }
    }
}
