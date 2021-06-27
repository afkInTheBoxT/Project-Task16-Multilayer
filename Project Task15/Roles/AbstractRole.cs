using Project_Task15.Models;
using Project_Task15.Observer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Task15.Roles
{
    public class AbstractRole : ISubject
    {
        protected delegate void MyAction();

        protected MyAction optionChooser;
        protected Dictionary<int, Option> options;
        protected readonly IRepository storage;
        public bool isOpen;

        private List<IObserver> observers = new List<IObserver>();

        public AbstractRole(IRepository storage)
        {
            options = new Dictionary<int, Option>()
            {
                { 0, new Option("Exit application", Exit)},
                { 1, new Option("Print all products", PrintAllItemsOperation) },
                { 2, new Option("Search the product by name", SearchByNameOperation) }
            };
            this.storage = storage;
            isOpen = true;
        }

        public virtual void Start(UserEntity currUs = null)
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

        public void Exit()
        {
            isOpen = false;
        }

        public void PrintAllItems()
        {
            foreach (var item in GetItems())
            {
                Printer.Print(item.ToString());
            }
        }

        public void PrintOptions()
        {
            Printer.Print("Menu:\n");
            for (int i = 0; i < options.Count; i++)
            {
                Printer.Print(i + " " + options[i].OptionDescription);
            }
        }

        public void PrintAllItemsOperation()
        {
            Printer.Print("The list of all products: ");
            PrintAllItems();
        }

        


        public void PerformAction(int optionNumber)
        {
            try
            {
                options[optionNumber].OptionFunction?.Invoke();
            } 
            catch (KeyNotFoundException)
            {
                Printer.Print("Wrong command. Try again.");
            }
        }

        public virtual void ChooseOptionDelegate(int chosenOption)
        {
            Printer.Clear();
            PerformAction(chosenOption);
        }


        public void Attach(IObserver observer)
        {
            this.observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            this.observers.Remove(observer);
        }

        public void Notify(UserEntity user)
        {
            int obsLen = observers.Count;

            for (int i = 0; i < obsLen; i++)
            {
                observers[i].Update(this, user);
            }            
            
        }


        protected class Option
        {
            public string OptionDescription { get; set; }
            public MyAction OptionFunction { get; set; }


            public Option(string desciption, MyAction function)
            {
                OptionDescription = desciption;
                OptionFunction = function;
            }
        }
    }
}
