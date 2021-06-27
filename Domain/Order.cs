using System;
using System.Collections.Generic;
using System.Text;
using static Entities.OrderEntity;

namespace Domain
{
    class Order
    {
        public int UserId { get; set; }
        public List<Item> Items { get; set; }
        public OrderStatuses OrderStatus { get; set; }

        public static int counter = 0;

    }
}
