using Project_Task15.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Task15.Roles
{
    public class GuestRole : AbstractRole
    {
        public GuestRole(IRepository storage) : base (storage)
        {
            options = new Dictionary<int, Option>()
            {
                { 0, new Option("Exit application", Exit)},
                { 1, new Option("Print all products", PrintAllItemsOperation) },
                { 2, new Option("Search the product by name", SearchByNameOperation) },
                { 3, new Option("Sign in", SignInOperation) },
                { 4, new Option("Log in", LogInOperation) }
            };
        }

        public override void Start(UserEntity currUs = null)
        {
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
    }
}
