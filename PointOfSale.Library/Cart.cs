using System;
using System.Collections.Generic;

namespace PointOfSale.Library
{
    public class Special {
        public int NormalPricedCount {get;set;}
        public int SpecialPricedCount {get;set;}
        public double Markdown {get;set;}
        public bool IsActive {get;set;}
    }

    public class Item {

        public Item(string name = "", double price = 0) {
              Name = name;
              Price = price;
              Special = new Special();
        }

        public string Name {get; set;}
        public double Price {get; set;}
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
            tempList.Add(new Item("soup", 1.89));
            tempList.Add(new Item("ground beef", 5.99));
            foreach(var Item in tempList) {
                AddToInventory(Item);
            }
        }
        public void AddToInventory(Item product) {
            Inventory.Add(product);
        }
        
        public void UpdatePrice(string productName, double newPrice) {
            Inventory.Find(x=> x.Name == productName).Price = newPrice;
        }

        public void UpdateMarkdown(string productName, double markdownPercent) {
            Inventory.Find(x=> x.Name == productName).Markdown = markdownPercent/100;
        }

        public void ConfigureSpecialOffer(string productName, int firstQuantity, int secondQuantity, double markdown) {
            var currItem = Inventory.Find(x=> x.Name == productName);
            currItem.Special.NormalPricedCount = firstQuantity;
            currItem.Special.SpecialPricedCount = secondQuantity;
            currItem.Special.Markdown = markdown/100;
            currItem.Special.IsActive = true;
        }

        public void ConfigureSpecialOffer(string productName, int specialQuantity, double markdown) {
            var currItem = Inventory.Find(x=> x.Name == productName);
            currItem.Special.SpecialPricedCount = specialQuantity;
            currItem.Special.Markdown = markdown/100;
            currItem.Special.IsActive = true;
        }

        //Cart Methods
        public void ScanItem(string productName) {
            var newCartItem = new Item();
            newCartItem = Inventory.Find(x=> x.Name == productName);
            if(Cart.Find(x=> x.Name == newCartItem.Name) == null) {
                newCartItem.UnitCount = 1;
                Cart.Add(newCartItem);
            } else {
                newCartItem.UnitCount++;
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
        public double GetCartTotal(string productName) {
            return GetCost(Cart.Find(x=> x.Name == productName));
        }

        public double GetCartTotal() {
            double totalCost = 0;
            foreach(var Item in Cart) {
                totalCost += GetCost(Item);
            }
            return totalCost;
        }

        private double GetCost(Item currItem) {
            if(currItem.UnitCount > currItem.Special.NormalPricedCount && currItem.Special.IsActive) {
                return Math.Round((
                    currItem.Special.NormalPricedCount * currItem.Price +
                    currItem.Special.SpecialPricedCount * (currItem.Price * (1 - currItem.Special.Markdown))), 2);
            } else {
                return Math.Round((
                    currItem.UnitCount * (currItem.Price * (1 - currItem.Markdown))), 2);
            }
            
        }
    }
}
