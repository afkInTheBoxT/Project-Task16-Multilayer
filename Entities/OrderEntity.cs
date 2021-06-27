using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Entities
{
    public class OrderEntity : AbstractEntity
    {
        public int UserId { get; set; }
        public List<ItemEntity> Items { get; set; }
        public OrderStatuses OrderStatus { get; set; }

        public static int counter = 0;


        public OrderEntity(int userId, IEnumerable<ItemEntity> items)
        {
            Id = ++counter;
            UserId = userId;
            Items = new List<ItemEntity>(items);
            OrderStatus = OrderStatuses.New;
        }

        public override string ToString()
        {
            return $"Order №{Id}. \nItems:\n " + string.Join("\n", Items) + $"\nOrderStatus: {OrderStatus}";
        }


        public enum OrderStatuses
        { 
            New,
            CancelledByAdmin,
            ReceivedPayment,
            Sent,
            Received,
            Finished,
            CancelledByUser
        }
    }
}
