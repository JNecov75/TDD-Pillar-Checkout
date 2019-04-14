using System;
using System.Collections.Generic;

namespace PointOfSale.Library
{
    public class Item {

        public Item(string name = "", double price = 0) {
              Name = name;
              Price = price;
        }

        public string Name {get; set;}
        public double Price {get; set;}
        public double Markdown {get; set;}
        public double UnitCount {get; set;}
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

        public double GetCost(Item currItem) {
            return Math.Round((
                currItem.UnitCount * (currItem.Price * (1 - currItem.Markdown))), 2);
        }
    }
}
