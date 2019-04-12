using System;
using System.Collections.Generic;

namespace PointOfSale.Library
{
    public struct Item {

        public Item(string name = "", string price = "") {
              Name = name;
              Price = price;
        }
        public string Name {get;set;}
        public string Price {get;set;}
    }

    public class Shop
    {
        public List<Item> Cart;
        public List<Item> Inventory;

        public Shop() {
            Cart = new List<Item>();
            Inventory = new List<Item>(1);
        }
        public void AddToInventory(Item product) {
            Inventory.Add(product);
        }
        
    }
}
