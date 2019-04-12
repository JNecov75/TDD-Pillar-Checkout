using System;
using System.Collections.Generic;

namespace PointOfSale.Library
{
    public struct Item {

        public Item(string name = "", double price = 0) {
              Name = name;
              Price = price;
        }
        public string Name {get;set;}
        public double Price {get;set;}
    }

    public class Shop
    {
        public List<Item> Cart;
        public List<Item> Inventory;

        public Shop() {
            Cart = new List<Item>();
            Inventory = new List<Item>();
        }

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
        
    }
}
