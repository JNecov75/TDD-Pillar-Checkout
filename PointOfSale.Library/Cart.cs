﻿using System;
using System.Collections.Generic;

namespace PointOfSale.Library
{
    public class Item {

        public Item(string name = "", double price = 0) {
              Name = name;
              Price = price;
        }

        public string Name {get;set;}
        public double Price {get;set;}
        public int Quantity {get; set;}
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
                Console.WriteLine(Item.Name + " added to inventory.");
            }
            Console.WriteLine("Current Inventory Size: " + Inventory.Count);
        }
        public void AddToInventory(Item product) {
            Inventory.Add(product);
        }
        
        public void UpdatePrice(string productName, double newPrice) {
            Inventory.Find(x=> x.Name == productName).Price = newPrice;
        }

        //Cart Methods
        public void ScanItem(string productName) {
            var newCartItem = new Item();
            newCartItem = Inventory.Find(x=> x.Name == productName);
            if(Cart.Find(x=> x.Name == newCartItem.Name) == null) {
                newCartItem.Quantity = 1;
                Cart.Add(newCartItem);
            } else {
                newCartItem.Quantity++;
            }
        }

        public double GetCartTotal() {
            double totalCost = 0;
            foreach(var Item in Cart) {
                totalCost += Item.Price * Item.Quantity;
            }
            return totalCost;
        }
    }
}
