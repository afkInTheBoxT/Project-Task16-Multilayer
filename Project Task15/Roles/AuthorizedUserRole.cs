using Project_Task15.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Project_Task15.Roles
{
    public class AuthorizedUserRole : AbstractRole
    {
        private List<ItemEntity> cart;
        public UserEntity currentUser { get; set; }


        public AuthorizedUserRole(IRepository storage) : base(storage)
        {
            cart = new List<ItemEntity>();

            options = new Dictionary<int, Option>()
            {
                { 0, new Option("Exit application", Exit)},
                { 1, new Option("Print all products", PrintAllItemsOperation) },
                { 2, new Option("Search the product by name", SearchByNameOperation) },
                { 3, new Option("Choose items to order", ChooseItemsToOrderOption) },
                { 4, new Option("Create order", CreateOrderOption) },
                { 5, new Option("See all orders", PrintAllOrdersOperation) },
                { 6, new Option("Mark the order as received", ReceiveOrderOperation) },
                { 7, new Option("Change account information", ChangeAccountInformation) },
                { 8, new Option("Log out", LogOutOperation) },
                { 9, new Option("Clear all cart", ClearCartOperation) },
                { 10, new Option("Cancel the order", CancelOrderOpertion) }
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
            Printer.Print("\n");
            foreach (var item in cart)
            {
                Printer.Print(item.ToString() + "\n");
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
                    catch(FormatException)
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

        public void PrintAllOrdersOperation()
        {
            Printer.Print("Order history:");
            var userOrders = storage.GetUserOrders(currentUser.Email, currentUser.Password);
            foreach (var order in userOrders)
            {
                Printer.Print(order.ToString());
            }
        }

        public void ReceiveOrderOperation()
        {
            Printer.Print("Accept order:");
            PrintAllOrdersOperation();
            Printer.Print("Type the number of the order you want to mark as received:");
            int ordernumber = Convert.ToInt32(Printer.Read());            
            Printer.Print(storage.AcceptOrder(ordernumber));
        }

        public void ChangeAccountInformation()
        {
            Printer.Print("Change account information:");
            Printer.Print("What do you want to edit? \n1\tEmail \n2\tPassword");
            int editOption = Convert.ToInt32(Printer.Read());

            switch (editOption)
            {
                case 1:
                    ChangeEmail();
                    break;
                case 2:
                    ChangePassword();
                    break;
                default:
                    Printer.Print("You have chosen the wrong command. Try again.");
                    ChangeAccountInformation();
                    break;
            }
        }

        public void LogOutOperation() 
        {
            //LogOutEventHandler();
            Notify(null);
        }

        public void ClearCartOperation()
        {
            Printer.Print("Cancel order");
            Printer.Print("You are going to clear your cart. Are you sure?");
            cart = new List<ItemEntity>();
        }

        public void CancelOrderOpertion()
        {
            Printer.Print("Cancel order");
            Printer.Print("Input Id of the order you want to cancel:");
            try
            {
                int id = Convert.ToInt32(Printer.Read());
                if (!storage.GetUserOrders(currentUser.Email, currentUser.Password)
                    .Select(order => order.Id).Contains(id))
                {
                    Printer.Print("There is no order with such Id.");
                    return;
                }
                var order = storage.GetOrderById(id);

                if (order.OrderStatus != OrderEntity.OrderStatuses.New && 
                    order.OrderStatus != OrderEntity.OrderStatuses.ReceivedPayment)
                {
                    Printer.Print("You can't cancel this order now.");
                    return;
                }

                order.OrderStatus = OrderEntity.OrderStatuses.CancelledByUser;
                currentUser.Balance += order.Items.Sum(item => item.Price);
                Printer.Print($"Order with ID {id} was cancelled. Money were returned.");
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

        public void ChangePassword()
        {
            Printer.Print("Change password:");
            string password = Printer.Read();
            currentUser.Password = password;
            Printer.Print("You have successfully changed and email.");
            storage.UpdateUser(currentUser);
        }

        public void ChangeEmail()
        {
            Printer.Print("Change email:");
            string email = Printer.Read();
            currentUser.Email = email;
            Printer.Print("You have successfully changed and email.");
            storage.UpdateUser(currentUser);
        }

    }
}
