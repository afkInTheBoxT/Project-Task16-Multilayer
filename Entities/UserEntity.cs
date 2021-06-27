using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class UserEntity : AbstractEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
        public decimal Balance { get; set; }
        public static int counter = 0;


        public UserEntity(string email, string password, Roles role = Roles.User, decimal balance = 0)
        {
            Id = ++counter;
            Email = email;
            Password = password;
            Role = role;
            Balance = balance;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Email: {Email}, Password: {Password}," +
                $" Role: {Role}, Balance: {Balance}";
        }

        public enum Roles
        {
            User,
            Admin
        }
    }
}
