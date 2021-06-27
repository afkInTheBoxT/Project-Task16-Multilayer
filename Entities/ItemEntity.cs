using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class ItemEntity : AbstractEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public static int counter = 0;


        public ItemEntity(string name, decimal price)
        {
            Id = ++counter;
            Name = name;
            Price = price;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Price: {Price}.";
        }
    }
}
