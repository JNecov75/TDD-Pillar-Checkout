using System;
using System.Collections.Generic;

namespace PointOfSale.Library
{
    public class Special {
        public string SpecialType {get;set;}
        public int NormalPricedCount {get;set;}
        public int SpecialPricedCount {get;set;}
        public double Modifier {get;set;}
        public double Limit {get;set;}
        public bool IsActive {get;set;}
    }

    public class Item {

        public Item(string name = "", decimal price = 0) {
              Name = name;
              Price = price;
              Special = new Special();
        }

        public string Name {get; set;}
        public decimal Price {get; set;}
        public double Markdown {get; set;}
        public double UnitCount {get; set;}

        public Special Special {get;set;}
    }

    public class Shop
    {
        public List<Item> Cart;
        public List<Item> Inventory;

        public Shop() {
            Cart = new List<Item>();
            Inventory = new List<Item>();
        }

        //Inventory Methods
        public void InitializeInventory() {
            var tempList = new List<Item>();
            tempList.Add(new Item("soup", 1.89m));
            tempList.Add(new Item("ground beef", 5.99m));
            foreach(var Item in tempList) {
                AddToInventory(Item);
            }
        }
        public void AddToInventory(Item product) {
            Inventory.Add(product);
        }
        
        public void UpdatePrice(string productName, decimal newPrice) {
            Inventory.Find(x=> x.Name == productName).Price = newPrice;
        }

        public void UpdateMarkdown(string productName, double markdownPercent) {
            Inventory.Find(x=> x.Name == productName).Markdown = markdownPercent/100;
        }

        public void ConfigureBOGOSpecialOffer(string productName, int firstQuantity, int secondQuantity, double modifier, double limit = -1) {
            var currItem = Inventory.Find(x=> x.Name == productName);
            currItem.Special.SpecialType = "Buy N items get M at %X off.";
            currItem.Special.NormalPricedCount = firstQuantity;
            currItem.Special.SpecialPricedCount = secondQuantity;
            currItem.Special.Modifier = modifier/100;
            currItem.Special.Limit = limit;
            currItem.Special.IsActive = true;
        }

        public void ConfigureDiscountSpecialOffer(string productName, int specialQuantity, double modifier, double limit = -1) {
            var currItem = Inventory.Find(x=> x.Name == productName);
            currItem.Special.SpecialType = "N for $X.";
            currItem.Special.SpecialPricedCount = specialQuantity;
            currItem.Special.Modifier = modifier;
            currItem.Special.Limit = limit;
            currItem.Special.IsActive = true;
        }

        //Cart Methods
        public void ScanItem(string productName, int unitCount = 1) {
            var newCartItem = new Item();
            newCartItem = Inventory.Find(x=> x.Name == productName);
            newCartItem.UnitCount += unitCount;
            if(Cart.Find(x=> x.Name == newCartItem.Name) == null) {
                Cart.Add(newCartItem);
            }
        }
        
        public void ScanItem(string productName, double weight) {
            var newCartItem = new Item();
            newCartItem = Inventory.Find(x=> x.Name == productName);
            newCartItem.UnitCount += weight;
            if(Cart.Find(x=> x.Name == newCartItem.Name) == null) {
                Cart.Add(newCartItem);
            }
        }
        public decimal GetCartTotal(string productName) {
            return GetCost(Cart.Find(x=> x.Name == productName));
        }

        public decimal GetCartTotal() {
            decimal totalCost = 0;
            foreach(var Item in Cart) {
                totalCost += GetCost(Item);
            }
            return totalCost;
        }

        private decimal GetCost(Item currItem) {
            if(currItem.Special.SpecialType == "Buy N items get M at %X off.") {
                return GetBOGOSpecialCost(currItem);
            } else if(currItem.Special.SpecialType == "N for $X.") {
                return GetDiscountSpecialCost(currItem);
            } else {
                return Math.Round((
                    (decimal)currItem.UnitCount * (currItem.Price * (decimal)(1 - currItem.Markdown))), 2);
            }
        }

        private decimal GetBOGOSpecialCost(Item currItem) {
            double adjustedCount = currItem.Special.Limit > -1 ? currItem.Special.Limit : currItem.UnitCount;
            double numItemsInASpecial = currItem.Special.NormalPricedCount + currItem.Special.SpecialPricedCount;

            double numSpecialsPurchased = Math.Floor(adjustedCount / numItemsInASpecial);

            double itemsNotInSpecial = currItem.UnitCount - (numItemsInASpecial * numSpecialsPurchased);
            double numNormalPrice =  numSpecialsPurchased * currItem.Special.NormalPricedCount + itemsNotInSpecial;
            double numSpecialPrice  = numSpecialsPurchased * currItem.Special.SpecialPricedCount;
            return Math.Round((
                (decimal)numNormalPrice * currItem.Price +
                (decimal)numSpecialPrice * (currItem.Price * (decimal)(1 - currItem.Special.Modifier))), 2);
        }

        private decimal GetDiscountSpecialCost(Item currItem) {
            double adjustedCount = currItem.Special.Limit > -1 ? currItem.Special.Limit : currItem.UnitCount;

            double numSpecialsPurchased = Math.Floor(adjustedCount / currItem.Special.SpecialPricedCount);

            double itemsNotInSpecial = currItem.UnitCount - (currItem.Special.SpecialPricedCount * numSpecialsPurchased);
            double numNormalPrice =  itemsNotInSpecial;
            return Math.Round((
                ((decimal)numNormalPrice * currItem.Price) + 
                (decimal)numSpecialsPurchased * (decimal)currItem.Special.Modifier), 2);
        }
    }
}
