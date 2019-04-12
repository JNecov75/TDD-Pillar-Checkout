﻿using System;
using System.Collections.Generic;

namespace PointOfSale.Library
{
    public struct Item {
        public Item(string name) {
              Name = name;
        }
        public string Name {get;set;}
    }

    public class Cart
    {
        public List<Item> Items;

        public Cart() {
            Items = new List<Item>();
        }
        public void Add(Item product) {
            Items.Add(product);
        }
    }
}
