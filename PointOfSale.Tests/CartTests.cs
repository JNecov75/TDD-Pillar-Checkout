using Microsoft.VisualStudio.TestTools.UnitTesting;
using PointOfSale.Library;

namespace PointOfSale.Tests
{
    [TestClass]
    public class ShopTests
    {
        private Shop _shop;
        private Item _inventorySoup;
        private Item _inventoryGroundBeef;

        public ShopTests()
        {
            _shop = new Shop();
        }

        [TestInitialize()]
        public void Startup() {
            _shop.InitializeInventory();
            _inventorySoup = _shop.Inventory.Find(x=> x.Name == "soup");
            _inventoryGroundBeef = _shop.Inventory.Find(x=> x.Name == "ground beef");
        }

        //Inventory Tests
        [TestMethod]
        public void WhenAnItemIsAddedToInventoryItStoresTheName()
        {
            Assert.AreEqual("soup", _inventorySoup.Name);
            Assert.AreEqual("ground beef", _inventoryGroundBeef.Name);
        }

        [TestMethod]
        public void WhenAnItemIsAddedToInventoryItStoresThePrice()
        {
            Assert.AreEqual(1.89m, _inventorySoup.Price);
            Assert.AreEqual(5.99m, _inventoryGroundBeef.Price);
        }

        [TestMethod]
        public void InventoryCanBeInitializedWithASetOfProducts()
        {
            Assert.AreEqual(2, _shop.Inventory.Count);
        }

        [TestMethod]
        public void WhenAPriceChangesTheInventoryPriceUpdates()
        {
            Assert.AreEqual(1.89m, _inventorySoup.Price);
            _shop.UpdatePrice("soup", 1.99m);
            Assert.AreEqual(1.99m, _inventorySoup.Price);
            Assert.AreEqual(5.99m, _inventoryGroundBeef.Price);
            _shop.UpdatePrice("ground beef", 6.29m);
            Assert.AreEqual(6.29m, _inventoryGroundBeef.Price);
        }

        [TestMethod]
        public void WhenAMarkdownIsAppliedItIsReflectedInInventoryMarkdown()
        {
            Assert.AreEqual(0, _inventorySoup.Markdown);
            _shop.UpdateMarkdown("soup", 25);
            Assert.AreEqual(.25, _inventorySoup.Markdown);
            _shop.UpdateMarkdown("soup", 0);
            Assert.AreEqual(0, _inventorySoup.Markdown);
        }

        [TestMethod]
        public void SupportBOGOSpecialOfferInInventory()
        {
            _shop.ConfigureBOGOSpecialOffer("soup", 2, 1, 100);
            Assert.AreEqual(2, _inventorySoup.Special.NormalPricedCount);
            Assert.AreEqual(1, _inventorySoup.Special.SpecialPricedCount);
            Assert.AreEqual(1, _inventorySoup.Special.Modifier);
        }

        [TestMethod]
        public void SupportDiscountSpecialOfferInInventory()
        {
            _shop.ConfigureDiscountSpecialOffer("soup", 3, 5);
            Assert.AreEqual(3, _inventorySoup.Special.SpecialPricedCount);
            Assert.AreEqual(5, _inventorySoup.Special.Modifier);
        }

        [TestMethod]
        public void ApplyLimitForBOGOSpecialOfferInInventory()
        {
            _shop.ConfigureBOGOSpecialOffer("soup", 2, 1, 100, 6);
            Assert.AreEqual(6, _inventorySoup.Special.Limit);
        }

        [TestMethod]
        public void ApplyLimitForDiscountSpecialOfferInInventory()
        {
            _shop.ConfigureDiscountSpecialOffer("soup", 3, 5, 6);
            Assert.AreEqual(6, _inventorySoup.Special.Limit);
        }

        //Cart Tests
        [TestMethod]
        public void WhenAnItemIsScannedItAddsToTheCart()
        {
            _shop.ScanItem("soup");
            Assert.AreEqual("soup", _shop.Cart.Find(x=> x.Name == "soup").Name);
            Assert.AreEqual(1.89m, _shop.Cart.Find(x=> x.Name == "soup").Price);
            Assert.AreEqual(1, _shop.Cart.Find(x=> x.Name == "soup").UnitCount);
        }

        [TestMethod]
        public void WhenAnItemIsAddedToCartItIncrementsQuantity()
        {
            _shop.ScanItem("soup");
            Assert.AreEqual(1, _shop.Cart.Find(x=> x.Name == "soup").UnitCount);
            _shop.ScanItem("soup");
            Assert.AreEqual(2, _shop.Cart.Find(x=> x.Name == "soup").UnitCount);
            _shop.ScanItem("soup", 2);
            Assert.AreEqual(4, _shop.Cart.Find(x=> x.Name == "soup").UnitCount);
        }

        [TestMethod]
        public void WhenAnExistingItemIsAddedToCartItUpdatesItem()
        {
            _shop.ScanItem("soup", 2);
            Assert.AreEqual(1, _shop.Cart.Count);
        }

        [TestMethod]
        public void WhenAnItemWithWeightIsAddedToCartItUpdatesTotalCostOfItem()
        {
            _shop.ScanItem("ground beef", 1);
            Assert.AreEqual(5.99m , _shop.GetCartTotal("ground beef"));
            _shop.ScanItem("ground beef", 0.5);
            Assert.AreEqual(8.98m , _shop.GetCartTotal("ground beef"));
        }

        [TestMethod]
        public void WhenAnItemIsAddedToCartItUpdatesTotalCost()
        {
            _shop.ScanItem("soup", 2);
            _shop.ScanItem("ground beef");
            Assert.AreEqual(9.77m, _shop.GetCartTotal());
        }

        [TestMethod]
        public void WhenAMarkdownIsAppliedItIsReflectedInCartItemPrice()
        {
            _shop.ScanItem("soup");
            _shop.UpdateMarkdown("soup", 25);
            Assert.AreEqual(1.42m, _shop.GetCartTotal("soup"));
            _shop.ScanItem("soup");
            Assert.AreEqual(2.84m, _shop.GetCartTotal("soup"));
            _shop.ScanItem("ground beef");
            _shop.UpdateMarkdown("ground beef", 50);
            Assert.AreEqual(5.84m, _shop.GetCartTotal());
        }

        [TestMethod]
        public void SupportBOGOSpecialOfferInCartTotal()
        {
            _shop.ConfigureBOGOSpecialOffer("soup", 2, 1, 100);
            _shop.ScanItem("soup", 3);
            Assert.AreEqual(3.78m, _shop.GetCartTotal("soup"));
            _shop.ScanItem("soup");
            Assert.AreEqual(5.67m, _shop.GetCartTotal("soup"));
        }

        [TestMethod]
        public void SupportDiscountSpecialOfferInCartTotal()
        {
            _shop.ConfigureDiscountSpecialOffer("soup", 3, 5);
            _shop.ScanItem("soup");
            Assert.AreEqual(1.89m, _shop.GetCartTotal("soup"));
            _shop.ScanItem("soup");
            Assert.AreEqual(3.78m, _shop.GetCartTotal("soup"));
            _shop.ScanItem("soup");
            Assert.AreEqual(5m, _shop.GetCartTotal("soup"));
            _shop.ScanItem("soup");
            Assert.AreEqual(6.89m, _shop.GetCartTotal("soup"));
        }

        [TestMethod]
        public void ApplyLimitForBOGOSpecialOfferInCartTotal()
        {
            _shop.ConfigureBOGOSpecialOffer("soup", 2, 1, 100, 6);
            _shop.ScanItem("soup", 9);
            Assert.AreEqual(9, _shop.Cart.Find(x=> x.Name == "soup").UnitCount);
        }

        

    }
}
