using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    class Item
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public static int counter = 0;
    }
}
