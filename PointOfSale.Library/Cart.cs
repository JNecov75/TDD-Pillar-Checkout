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
    }

    public class Item {

        public Item(string name = "", decimal price = 0, bool weighted = false) {
              Name = name;
              Price = price;
              Special = new Special();
              Weighted = weighted;
        }

        public string Name {get; set;}
        public decimal Price {get; set;}
        public double Markdown {get; set;}
        public double UnitCount {get; set;}
        public bool Weighted {get;set;}

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
            tempList.Add(new Item("ground beef", 5.99m, true));
            tempList.Add(new Item("ham", 3.99m, true));
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
        }

        public void ConfigureWeightBOGOSpecialOffer(int firstQuantity, int secondQuantity, double modifier) {
            foreach(var currItem in Inventory) {
                if(currItem.Weighted) {
                    currItem.Special.SpecialType = "Buy N get M of equal or lower weight at %X off.";
                    currItem.Special.NormalPricedCount = firstQuantity;
                    currItem.Special.SpecialPricedCount = secondQuantity;
                    currItem.Special.Modifier = modifier/100;
                }
            }   
        }

        public void ConfigureDiscountSpecialOffer(string productName, int specialQuantity, double modifier, double limit = -1) {
            var currItem = Inventory.Find(x=> x.Name == productName);
            currItem.Special.SpecialType = "N for $X.";
            currItem.Special.SpecialPricedCount = specialQuantity;
            currItem.Special.Modifier = modifier;
            currItem.Special.Limit = limit;
        }

        //Cart Methods
        public void ScanItem(string productName, double unitCount = 1) {
            var newCartItem = new Item();
            newCartItem = Inventory.Find(x=> x.Name == productName);
            newCartItem.UnitCount += unitCount;
            if(Cart.Find(x=> x.Name == newCartItem.Name) == null) {
                Cart.Add(newCartItem);
            }
        }

        public void RemoveItem(string productName, double unitCount = 1) {
            var newCartItem = new Item();
            newCartItem = Inventory.Find(x=> x.Name == productName);
            newCartItem.UnitCount -= unitCount;
            if(newCartItem.UnitCount <= 0) {
                Cart.Remove(newCartItem);
            }
        }
        
        public decimal GetCartTotal(string productName) {
            return GetCost(Cart.Find(x=> x.Name == productName));
        }

        public decimal GetCartTotal() {
            decimal totalCost = 0;
            Console.WriteLine(totalCost);
            Console.WriteLine(Cart.Count);
            foreach(var Item in Cart) {
                totalCost += GetCost(Item);
            } 
            Console.WriteLine(totalCost);
            if ( Cart.Find(x => x.Special.SpecialType == "Buy N get M of equal or lower weight at %X off.") != null) {
                var tempCost = totalCost;
                totalCost = AdjustCostForWeightBOGOSpecial(tempCost);
                Console.WriteLine(totalCost);
            }
            return totalCost;
        }

        private decimal AdjustCostForWeightBOGOSpecial(decimal totalCost) {
            double totalWeightedItems = 0;
            List<Item> weightedCartItems = new List<Item>();
            foreach (var Item in Cart) {
                if(Item.Weighted) {
                    weightedCartItems.Add(Item);
                }
            }
            Item cheapestItem = new Item();
            cheapestItem.Price = weightedCartItems[0].Price;
            double numItemsInASpecial = weightedCartItems[0].Special.NormalPricedCount + weightedCartItems[0].Special.SpecialPricedCount;

            foreach (var Item in weightedCartItems) {
                totalWeightedItems += Item.UnitCount;
                cheapestItem.Price = Item.Price < cheapestItem.Price ? Item.Price : cheapestItem.Price;
            }
            double numSpecialsPurchased = Math.Floor(totalWeightedItems / numItemsInASpecial);
            double numAtSpecialPrice  = numSpecialsPurchased * weightedCartItems[0].Special.SpecialPricedCount;
            for(int i = 0; i < numAtSpecialPrice; i++) {
                totalCost -= cheapestItem.Price;
            }
            
            return totalCost;
        }
        private decimal GetCost(Item currItem) {
            if(currItem.Special.SpecialType == "Buy N items get M at %X off.") {
                return GetBOGOSpecialCost(currItem);
            } else if(currItem.Special.SpecialType == "N for $X.") {
                return GetDiscountSpecialCost(currItem);
            } else {
                Console.WriteLine((decimal)currItem.UnitCount + " " + currItem.Price + " " + (decimal)(1 - currItem.Markdown));
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
