using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Entities;

namespace Data
{
    public class Repository : IRepository
    {
        private static List<UserEntity> users;
        private static Dictionary<ItemEntity, int> items;
        private static List<OrderEntity> orders;


        public Repository(bool toFill = true)
        {
            users = new List<UserEntity>();
            items = new Dictionary<ItemEntity, int>();
            orders = new List<OrderEntity>();

            if (toFill) FillData();
        }

        public int GetAmountOfItems(ItemEntity item)
        {
            return items[item];
        }

        public void SetAmountOfItems(ItemEntity item, int amount)
        {
            if (amount < 0) throw new ArgumentException("Incorrect value");
            items[item] = amount;
        }

        public void FillData()
        {
            users.Add(new UserEntity("john.br1@gmail.com", "12345Qw"));
            users.Add(new UserEntity("john.br2@gmail.com", "12345Qwe", UserEntity.Roles.Admin));
            users.Add(new UserEntity("john.br3@gmail.com", "12345Qwer"));
            foreach (var user in users)
            {
                user.Balance += 1000;
            }

            items.Add(new ItemEntity("Bottle of water", 5.5m), 25);
            items.Add(new ItemEntity("Apples", 5m), 50);
            items.Add(new ItemEntity("Chips", 10m), 10);
        }

        public string AcceptOrder(int orderNumber)
        {
            try
            {
                orders.First(or => or.Id == orderNumber).OrderStatus = OrderEntity.OrderStatuses.Received;
            }
            catch (InvalidOperationException)
            {
                return "There is no order with such number.";
            }
            return "Success.";
        }

        public string AddNewItem(ItemEntity item, int amount)
        {
            if (item == null) return("The item was null.");

            items.Add(item, amount);
            return "Success";
        }

        public string CancelOrderByUser(OrderEntity order)
        {
            if (order == null) return("The order was null.");

            orders.First(or => or.Id == order.Id).OrderStatus = OrderEntity.OrderStatuses.CancelledByUser;
            return "Success";
        }

        public string CancelOrderByAdmin(OrderEntity order)
        {
            if (order == null) return("The order was null.");

            orders.First(or => or.Id == order.Id).OrderStatus = OrderEntity.OrderStatuses.CancelledByAdmin;
            return "Success";
        }


        public string CreateNewOrder(IEnumerable<ItemEntity> orderItems, int userId)
        {
            var user = users.FirstOrDefault(user => user.Id == userId);
            if (user == null) return "No such user";
            if (user.Balance < orderItems.Sum(item => item.Price)) return "Not enough money";


            orders.Add(new OrderEntity(userId, orderItems));

            foreach(var item in orderItems)
            {
                items[item]--;
                user.Balance -= item.Price;
            }
            return "Success";
        }

        public ItemEntity GetItemById(int id)
        {
            return items.Keys.FirstOrDefault(it => it.Id == id);
        }

        public void EditItem(ItemEntity item)
        {
            ItemEntity itemToCnhange = items.Keys.First(it => it.Id == item.Id);
            itemToCnhange.Name = item.Name;
            itemToCnhange.Price = item.Price;
        }

        public List<ItemEntity> FindItemByName(string name)
        {
            return items.Keys.Where(item => item.Name.Contains(name)).ToList();
        }

        public List<ItemEntity> GetItems() => items.Keys.ToList();



        public UserEntity GetUser(string email, string password) => 
            users.FirstOrDefault(us => us.Email == email && us.Password == password);

        public UserEntity GetUser(int userId) => users.First(user => user.Id == userId);

        public List<UserEntity> GetUsers() => users;

        public List<OrderEntity> GetUserOrders(string email, string password)
        {
            UserEntity user = users.Find(us => us.Email == email && us.Password == password);

            return orders.FindAll(order => order.UserId == user.Id);
        }

        public OrderEntity GetOrderById(int id) => orders.FirstOrDefault(order => order.Id == id);


        public void RegisterNewUser(string email, string password, UserEntity.Roles role = UserEntity.Roles.User)
        {
            users.Add(new UserEntity(email, password, role));
        }

        public string UpdateOrderStatus(int orderId, OrderEntity.OrderStatuses status)
        {
            if (orders.First(order => order.Id == orderId) == null) return "No order with this number";
            orders.First(order => order.Id == orderId).OrderStatus = status;
            return "Success";
        }

        public void UpdateUser(UserEntity user)
        {
            UserEntity userToChange = users.First(us => us.Id == user.Id);
            userToChange = user;
        }

        public void AddBalance(UserEntity user, decimal balance)
        {
            user.Balance += balance;
        }
    }
}
