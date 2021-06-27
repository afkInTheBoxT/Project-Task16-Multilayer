using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public interface IRepository
    {
        List<ItemEntity> GetItems();
        List<ItemEntity> FindItemByName(string name);
        string AddNewItem(ItemEntity item, int amount);
        void EditItem(ItemEntity item);
        ItemEntity GetItemById(int id);
        void SetAmountOfItems(ItemEntity item, int amount);
        int GetAmountOfItems(ItemEntity item);


        string CreateNewOrder(IEnumerable<ItemEntity> items, int userId);
        string AcceptOrder(int orderNumber);
        string CancelOrderByUser(OrderEntity order);
        string CancelOrderByAdmin(OrderEntity order);

        List<OrderEntity> GetUserOrders(string email, string password);
        string UpdateOrderStatus(int orderId, OrderEntity.OrderStatuses status);
        OrderEntity GetOrderById(int id);


        void RegisterNewUser(string email, string password, UserEntity.Roles role = UserEntity.Roles.User);
        UserEntity GetUser(string email, string password);
        UserEntity GetUser(int userId);
        List<UserEntity> GetUsers();
        void UpdateUser(UserEntity user);
        void AddBalance(UserEntity user, decimal balance);
    }
}
