using Data;
using System;
using System.Collections.Generic;
using System.Text;



namespace Services
{
    class GeneralService
    {
        private readonly IRepository repository;


        public GeneralService(IRepository repository)
        {
            this.repository = repository;
        }


        public List<Item> GetItems()
        {
            return storage.GetItems();
        }

        public void SearchByNameOperation()
        {
            Printer.Print("Search product by name: ");
            Printer.Print("Write the name of the product: ");
            string nameToSearch = Printer.Read();
            var searchRes = SearchItemByName(nameToSearch);
            Printer.Print("\nFounded items: \n");
            foreach (var item in searchRes)
            {
                Printer.Print(item.ToString());
            }
        }

        public void SignInOperation()
        {
            Printer.Print("Signing in: ");
            string email = "";
            string password = "";

            do
            {
                Printer.Print("Write email: ");
                email = Printer.Read();
                if (!email.Contains("@"))
                {
                    Printer.Print("It isn't a valid email address. Try again");
                }
            } while (!email.Contains("@"));

            Printer.Print("Write password: ");
            password = Printer.Read();

            Register(email, password);
            Printer.Print("You have successfully signed in.");
        }

        public void LogInOperation()
        {
            Printer.Print("Logging in: ");
            string email = "";
            string password = "";

            do
            {
                Printer.Print("Write email: ");
                email = Printer.Read();
                if (!email.Contains("@"))
                {
                    Printer.Print("It isn't a valid email address. Try again.");
                }
            } while (!email.Contains("@"));

            Printer.Print("Write password: ");
            password = Printer.Read();

            UserEntity user = LogIn(email, password);
            if (user == null)
            {
                Printer.Print("There isn't this account. Try again.");
                LogInOperation();
                return;
            }
            else
            {
                Printer.Print("You have logged in successfully.");
                //LogInEventHandler(user);
                Notify(user);
                return;
            }
        }

        public List<ItemEntity> SearchItemByName(string name)
        {
            return storage.FindItemByName(name);
        }

        public void Register(string email, string password)
        {
            storage.RegisterNewUser(email, password);
        }

        public UserEntity LogIn(string email, string password)
        {
            return storage.GetUser(email, password);
        }
    }
}
