using Project_Task15.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace Project_Task15.Roles
{
    public class AdminRole : AbstractRole
    {
        private List<ItemEntity> cart;
        public UserEntity currentUser { get; set; }


        public AdminRole(IRepository storage) : base(storage)
        {
            cart = new List<ItemEntity>();
            options = new Dictionary<int, Option>()
            {
                { 0, new Option("Exit application", Exit)},
                { 1, new Option("Print all products", PrintAllItemsOperation) },
                { 2, new Option("Search the product by name", SearchByNameOperation) },
                { 3, new Option("Choose items to order", ChooseItemsToOrderOption) },
                { 4, new Option("Create order", CreateOrderOption) },
                { 5, new Option("Print users information", PrintEditUsersInfofrmation) },
                { 6, new Option("Add new item", AddNewItemOperation) },
                { 7, new Option("Edit item", EditItemOperation) },
                { 8, new Option("Change order status", ChangeOrderStatusOperation) },
                { 9, new Option("Log out", LogOutOperation) },
                { 10, new Option("Clear cart", ClearCartOperation) },
                { 11, new Option("Cancel the order", CancelOrderOperation) }
            };
        }

        public override void Start(UserEntity currUs = null)
        {
            currentUser = currUs;
            int optionNumber = 0;

            while (isOpen)
            {
                PrintOptions();
                string input = Printer.Read();

                try
                {
                    optionNumber = Convert.ToInt32(input);
                    ChooseOptionDelegate(optionNumber);
                }
                catch (FormatException)
                {
                    Printer.Print("Wrong command. Try again.");
                    continue;
                }
            }
        }

        public void PrintCart()
        {
            foreach (var item in cart)
            {
                Printer.Print(item.ToString());
            }
        }

        public void ChooseItemsToOrderOption()
        {
            Printer.Print("Choose items to order:");
            Printer.Print("Would you like to see the full list of items?");
            if (Printer.Read() == "yes") PrintAllItems();

            Printer.Print("Type items numbers or \"exit\" to leave. To remove type remove and number.");
            string input = Printer.Read();
            while (input != "exit")
            {
                if (input.Contains("remove "))
                {
                    try
                    {
                        int number = Convert.ToInt32(input.Split(" ")[1]);
                        RemoveFromCart(number);
                    }
                    catch (FormatException)
                    {
                        Printer.Print("Wrong command.");
                    }
                }
                else
                {
                    try
                    {
                        int number = Convert.ToInt32(input);
                        AddToCart(number);
                    }
                    catch (FormatException)
                    {
                        Printer.Print("Wrong command.");
                    }
                }
                input = Printer.Read();
            }
        }

        public void CreateOrderOption()
        {
            Printer.Print("Create order:");
            Printer.Print("Your cart:");
            PrintCart();
            Printer.Print("Do you confirm the order?");
            string input = Printer.Read();

            while (input != "yes" || input != "no")
            {
                if (input == "yes")
                {
                    storage.CreateNewOrder(cart, currentUser.Id);
                    return;
                }
                else if (input == "no")
                {
                    return;
                }
                else
                {
                    Printer.Print("Unknown command. Try again.");
                    input = Printer.Read();
                }
            }

        }

        public void PrintEditUsersInfofrmation()
        {
            Printer.Print("Print users information.\n" +
                "To edit user information type the id and \" edit\".\n" +
                "To leave the list of the users type \"exit\"");
            List<UserEntity> users = storage.GetUsers();
            foreach (var user in users)
            {
                Printer.Print(user.ToString());
            }

            string input = Printer.Read();
            if (input.Contains("edit"))
            {
                int userId = Convert.ToInt32(input.Split(" ")[0]);
                UserEntity userToEdit = storage.GetUser(userId);
                Printer.Print("What do you want to edit? \n1\tEmail \n2\tPassword \n3 \tRole");
                int editOption = Convert.ToInt32(Printer.Read());
                switch (editOption)
                {
                    case 1:
                        ChangeEmail(userToEdit);
                        break;
                    case 2:
                        ChangePassword(userToEdit);
                        break;
                    case 3:
                        ChangeRole(userToEdit);
                        break;
                    default:
                        Printer.Print("You have chosen the wrong command. Try again.");
                        PrintEditUsersInfofrmation();
                        break;
                }
            }
        }

        public void AddNewItemOperation()
        {
            Printer.Print("Add new product");
            Printer.Print("Fill in the form to add a new product");
            Printer.Print("Name: ");
            string name = Printer.Read();
            Printer.Print("Price: ");
            string price = Printer.Read();
            Printer.Print("Amount: ");
            string amount = Printer.Read();
            ItemEntity itemToAdd = new ItemEntity(name, Convert.ToDecimal(price));            
            Printer.Print(storage.AddNewItem(itemToAdd, Convert.ToInt32(amount)));
        }

        public void EditItemOperation()
        {
            Printer.Print("Edit product");
            Printer.Print("What product do you want to edit? Write id.");
            int itemId = Convert.ToInt32(Printer.Read());
            ItemEntity itemToEdit = storage.GetItemById(itemId);
            while (itemToEdit == null)
            {
                Printer.Print("There is no such item. Try again.");
                itemId = Convert.ToInt32(Printer.Read());
                itemToEdit = storage.GetItemById(itemId);
            }

            Printer.Print("Fill in the form to edit a product:");
            Printer.Print($"Name: ({itemToEdit.Name})");
            string name = Printer.Read();
            Printer.Print($"Price: ({itemToEdit.Price})");
            string price = Printer.Read();
            Printer.Print($"Amount: ({storage.GetAmountOfItems(itemToEdit)})");
            string amount = Printer.Read();


            storage.SetAmountOfItems(itemToEdit, Convert.ToInt32(amount));
            itemToEdit.Name = name;
            itemToEdit.Price = Convert.ToDecimal(price);
            Printer.Print("You have successfully edited a product.");
        }

        public void ChangeOrderStatusOperation()
        {
            Printer.Print("Change order status:");
            Printer.Print("Select a user whose order you want to change (id):");
            foreach (var user in storage.GetUsers())
            {
                Printer.Print(user.ToString());
            }
            int userNumber = Convert.ToInt32(Printer.Read());
            UserEntity userChangeOrder = storage.GetUser(userNumber);
            Printer.Print("Select an order you want to change (id):");
            PrintAllUsersOrders(userChangeOrder);
            OrderEntity orderToChange = storage.GetOrderById(Convert.ToInt32(Printer.Read()));

            Printer.Print("Select a new status of the order:");
            for (int i = 0; i < Enum.GetValues(typeof(OrderEntity.OrderStatuses)).Length; i++)
            {
                Printer.Print(i + "\t" + ((OrderEntity.OrderStatuses)i).ToString());
            }
            int orderStatusNumber = Convert.ToInt32(Printer.Read());
            switch (orderStatusNumber)
            {
                case 0:
                    orderToChange.OrderStatus = 0;
                    break;
                case 1:
                    orderToChange.OrderStatus = (OrderEntity.OrderStatuses)1;
                    break;
                case 2:
                    orderToChange.OrderStatus = (OrderEntity.OrderStatuses)2;
                    break;
                case 3:
                    orderToChange.OrderStatus = (OrderEntity.OrderStatuses)3;
                    break;
                case 4:
                    orderToChange.OrderStatus = (OrderEntity.OrderStatuses)4;
                    break;
                case 5:
                    orderToChange.OrderStatus = (OrderEntity.OrderStatuses)5;
                    break;
                case 6:
                    orderToChange.OrderStatus = (OrderEntity.OrderStatuses)6;
                    break;
            }

            Printer.Print("You have changed the status of the order.");
        }

        public void LogOutOperation() 
        {
            
        }

        public void ClearCartOperation()
        {
            Printer.Print("Cancel order");
            Printer.Print("You are going to clear your cart. Are you sure?");
            cart = new List<ItemEntity>();
        }

        public void CancelOrderOperation()
        {
            Printer.Print("Cancel order");
            try
            {
                Printer.Print("Input Id of the user:");
                int userId = Convert.ToInt32(Printer.Read());
                UserEntity user = storage.GetUser(userId);
                if (user == null)
                {
                    Printer.Print("There is no such user");
                    return;
                }
                Printer.Print("Input Id of the order you want to cancel:");
                int orderId = Convert.ToInt32(Printer.Read());
                if (!storage.GetUserOrders(user.Email, user.Password)
                    .Select(order => order.Id).Contains(orderId))
                {
                    Printer.Print("There is no order with such Id.");
                    return;
                }
                var order = storage.GetOrderById(orderId);

                if (order.OrderStatus != OrderEntity.OrderStatuses.Finished)
                {
                    Printer.Print("You can't cancel this order now.");
                    return;
                }

                order.OrderStatus = OrderEntity.OrderStatuses.CancelledByAdmin;
                user.Balance += order.Items.Sum(item => item.Price);
                Printer.Print($"Order with ID {orderId} was cancelled. Money were returned.");
            }
            catch (FormatException)
            {
                Printer.Print("Not valid Id!");
            }
        }




        public void RemoveFromCart(int number)
        {
            try
            {
                cart.RemoveAt(number);
            }
            catch (ArgumentOutOfRangeException)
            {
                Printer.Print("There is no such item");
            }
        }

        public void AddToCart(int number)
        {
            ItemEntity itemByNumber = storage.GetItemById(number);
            if (itemByNumber == null)
            {
                Printer.Print("There is no item with such number. Try again.");
            }
            else
            {
                if (storage.GetAmountOfItems(itemByNumber) <= 0)
                {
                    Printer.Print("The shop is out of this items.");
                }
                else
                {
                    if (currentUser.Balance < cart.Sum(item => item.Price) + itemByNumber.Price)
                    {
                        Printer.Print("You don't have enough money");
                        return;
                    }
                    cart.Add(itemByNumber);
                    Printer.Print("The item was added to your cart.");
                }
            }
        }

        public void PrintAllUsersOrders(UserEntity user)
        {
            Printer.Print("User orders:");
            var userOrders = storage.GetUserOrders(user.Email, user.Password);

            foreach (var order in userOrders)
            {
                Printer.Print(order.ToString());
            }
        }

        public void ChangePassword(UserEntity userToEdit)
        {
            Printer.Print("Change password:");
            string password = Printer.Read();
            userToEdit.Password = password;
            Printer.Print("You have successfully changed and email.");
            storage.UpdateUser(currentUser);
        }

        public void ChangeEmail(UserEntity userToEdit)
        {
            Printer.Print("Change email:");
            string email = Printer.Read();
            userToEdit.Email = email;
            Printer.Print("You have successfully changed and email.");
            storage.UpdateUser(currentUser);
        }

        public void ChangeRole(UserEntity userToEdit)
        {
            Printer.Print("Change role:");
            string role = Printer.Read();
            switch (role)
            {
                case "user":
                    userToEdit.Role = UserEntity.Roles.User;
                    break;
                case "admin":
                    userToEdit.Role = UserEntity.Roles.Admin;
                    break;
            }

            Printer.Print("You have successfully changed and email.");
            storage.UpdateUser(currentUser);
        }
    }
}
