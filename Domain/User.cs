using System;
using System.Collections.Generic;
using System.Text;
using static Entities.UserEntity;

namespace Domain
{
    class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
        public decimal Balance { get; set; }
        public static int counter = 0;
    }
}
