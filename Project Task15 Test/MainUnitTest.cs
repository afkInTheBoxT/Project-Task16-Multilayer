using Project_Task15;
using Project_Task15.Models;
using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using Moq;
using Project_Task15.Roles;

namespace Project_Task15_Test
{
    public class MainUnitTest
    {
        private List<OrderEntity> cancelOrderData = new List<OrderEntity>()
        {
            new OrderEntity(1, new List<ItemEntity>()),
            null
        };



        #region Low
        [Fact]
        public void GetAmountOfItemsReturnsCorrectValue()
        {
            // Arrange
            Repository storage = new Repository(false);
            ItemEntity item1 = new ItemEntity("Item1", 10);
            storage.AddNewItem(item1, 5);
            storage.AddNewItem(new ItemEntity("Item2", 10), 3);
            int expected = 5;

            // Act
            int actual = storage.GetAmountOfItems(item1);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetAmountOfItemsSetsCorrectValue()
        {
            // Arrange
            Repository storage = new Repository(false);
            ItemEntity item1 = new ItemEntity("Item1", 10);
            storage.AddNewItem(item1, 5);
            int expected = 33;
            storage.SetAmountOfItems(item1, expected);

            // Act
            int actual = storage.GetAmountOfItems(item1);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateNewOrderSetCorrectValue()
        {
            // Arrange
            Repository storage = new Repository(false);
            ItemEntity item1 = new ItemEntity("Item1", 10);
            ItemEntity item2 = new ItemEntity("Item2", 10);
            storage.AddNewItem(item1, 5);
            storage.AddNewItem(item2, 3);

            List<ItemEntity> expected = new List<ItemEntity>() { item1, item1, item2 };
            storage.RegisterNewUser("test", "password");
            storage.AddBalance(storage.GetUser(UserEntity.counter), 1000);

            // Act
            storage.CreateNewOrder(expected, UserEntity.counter);

            // Assert
            Assert.Equal(expected, storage.GetOrderById(OrderEntity.counter).Items);
        }

        [Fact]
        public void CreateNewOrderRemovesItems()
        {
            // Arrange
            Repository storage = new Repository(false);
            ItemEntity item1 = new ItemEntity("Item1", 10);
            ItemEntity item2 = new ItemEntity("Item2", 10);
            storage.AddNewItem(item1, 5);
            storage.AddNewItem(item2, 3);

            List<ItemEntity> itemList = new List<ItemEntity>() { item1, item1, item2 };
            int[] expected = { 3, 2 };
            storage.RegisterNewUser("test", "password");
            storage.AddBalance(storage.GetUser(UserEntity.counter), 1000);

            // Act
            storage.CreateNewOrder(itemList, UserEntity.counter);

            // Assert
            Assert.True(expected[0] == storage.GetAmountOfItems(item1) &&
                expected[1] == storage.GetAmountOfItems(item2));
        }

        [Fact]
        public void AcceptOrderSetCorrectValue()
        {
            // Arrange
            Repository storage = new Repository(false);
            ItemEntity item1 = new ItemEntity("Item1", 10);
            ItemEntity item2 = new ItemEntity("Item2", 10);
            storage.AddNewItem(item1, 5);
            storage.AddNewItem(item2, 3);
            storage.RegisterNewUser("test1", "password1");
            storage.AddBalance(storage.GetUser(UserEntity.counter), 1000);
            

            List<ItemEntity> expected = new List<ItemEntity>() { item1, item1, item2 };
            storage.CreateNewOrder(expected, UserEntity.counter);

            // Act
            storage.AcceptOrder(OrderEntity.counter);

            // Assert
            Assert.True(storage.GetOrderById(OrderEntity.counter).OrderStatus == OrderEntity.OrderStatuses.Received);
        }

        [Fact]
        public void EditItemSetCorrectValue()
        {
            // Arrange
            Repository storage = new Repository(false);
            ItemEntity item1 = new ItemEntity("Item1", 10);
            storage.AddNewItem(item1, 5);
            ItemEntity expected = new ItemEntity("Check", 55);
            storage.AddNewItem(expected, 5);

            // Act
            storage.EditItem(expected);

            // Assert
            Assert.Equal(expected, storage.GetItemById(2));
        }

        [Fact]
        public void FindItemByNameFindCorrectValue()
        {
            // Arrange
            Repository storage = new Repository(false);
            ItemEntity item1 = new ItemEntity("Item1", 10);
            storage.AddNewItem(item1, 5);
            ItemEntity expected = new ItemEntity("Check", 55);
            storage.AddNewItem(expected, 5);

            // Act
            var actual = storage.FindItemByName("eck");

            // Assert
            Assert.Equal(expected, actual[0]);
        }

        [Fact]
        public void GetItemsReturnsCorrectValue()
        {
            // Arrange
            Repository storage = new Repository(false);
            ItemEntity item1 = new ItemEntity("Item1", 10);
            storage.AddNewItem(item1, 5);
            ItemEntity item2 = new ItemEntity("Check", 55);
            storage.AddNewItem(item2, 5);
            var expected = new List<ItemEntity>() { item1, item2 };

            // Act
            var actual = storage.GetItems();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetUserReturnsCorrectValue()
        {
            // Arrange
            Repository storage = new Repository(false);
            storage.RegisterNewUser("testEmail", "testPassword", UserEntity.Roles.Admin);
            UserEntity expected = new UserEntity("testEmail", "testPassword", UserEntity.Roles.Admin) { Id = --UserEntity.counter };

            // Act
            var actual = storage.GetUser("testEmail", "testPassword");

            // Assert
            Assert.True(expected.Email == actual.Email && expected.Password == expected.Password && 
                expected.Balance == actual.Balance && expected.Id == actual.Id);
        }

        [Fact]
        public void GetUserOrdersReturnsCorrectValue()
        {
            // Arrange
            Repository storage = new Repository(false);
            ItemEntity item1 = new ItemEntity("Item1", 10);
            ItemEntity item2 = new ItemEntity("Item2", 10);
            storage.AddNewItem(item1, 5);
            storage.AddNewItem(item2, 3);

            List<ItemEntity> expectedItems = new List<ItemEntity>() { item1, item1, item2 };
            storage.RegisterNewUser("test", "password");
            storage.AddBalance(storage.GetUser(UserEntity.counter), 1000);
            OrderEntity expected = new OrderEntity(UserEntity.counter, expectedItems);

            // Act
            storage.CreateNewOrder(expectedItems, UserEntity.counter);

            // Assert
            var tt = storage.GetOrderById(OrderEntity.counter).Items;
            var t = expected.Items.Equals(tt);
            Assert.True(expected.Items.All(item => storage.GetOrderById(OrderEntity.counter).Items.Any(i => i == item)) && 
                expected.OrderStatus == storage.GetOrderById(OrderEntity.counter).OrderStatus && 
                expected.UserId == storage.GetOrderById(OrderEntity.counter).UserId);
        }

        #endregion
        

        #region Advance
        [Fact]
        public void GetUsersNotNull()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetUsers()).Returns(new List<UserEntity>());

            // Act
            List<UserEntity> result = mock.Object.GetUsers();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetUserNotNull()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetUser("email", "pass")).
                Returns(new UserEntity("email", "password") { Id = --UserEntity.counter });

            // Act
            UserEntity result = mock.Object.GetUser("email", "pass");

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CancelOrderByAdminReturnsCorrectValue()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.CancelOrderByAdmin(cancelOrderData[0])).
                Returns("Success");
            mock.Setup(a => a.CancelOrderByAdmin(cancelOrderData[1])).
                Returns("The order was null.");
            string expected1 = "Success";
            string expected2 = "The order was null.";

            // Act
            string result1 = mock.Object.CancelOrderByAdmin(cancelOrderData[0]);
            string result2 = mock.Object.CancelOrderByAdmin(cancelOrderData[1]);

            // Assert
            Assert.Equal(new string[] { result1, result2 }, new string[] { expected1, expected2 });
        }

        [Fact]
        public void LogInSetsRoleAndUserCorrectly()
        {
            //Arange
            Repository storage = new Repository();
            GuestRole guest = new GuestRole(storage);
            Shop shop = new Shop();
            guest.Attach(shop);
            shop.currentRole = guest;
            UserEntity user = new UserEntity("test", "test", UserEntity.Roles.Admin);

            //Act
            guest.Notify(user);

            //Assert
            Assert.True(shop.curUser == user && shop.currentRole is AdminRole);
        }
        #endregion
    }
}
