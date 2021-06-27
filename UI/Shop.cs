using Project_Task15.Observer;
using System;

namespace Project_Task15
{
    public class Shop : IObserver
    {
        private IRepository storage;
        public UserEntity curUser;
        public AbstractRole currentRole;


        public Shop()
        {
            storage = new Repository();
        }

        public void DoAction()
        {
            Printer.Print("Welcome to the shop");
            GuestRole guest = new GuestRole(storage);
            AuthorizedUserRole authorizedUser = new AuthorizedUserRole(storage);
            AdminRole admin = new AdminRole(storage);
            guest.Attach(this);

            currentRole = guest;
            guest.Start();


            while (true)
            {
                currentRole.Start(curUser);
            }
        }


        public void Update(ISubject subject, UserEntity user)
        {
            if (user == null)
            {
                currentRole.Exit();
                curUser = null;
                currentRole = new GuestRole(storage);
                return;
            }

            switch (user.Role)
            {
                case UserEntity.Roles.User:
                    if (((AbstractRole)subject).isOpen)
                    {
                        currentRole.Exit();
                        curUser = user;
                    }
                    else
                    {
                        currentRole.Exit();
                        curUser = null;
                        currentRole = new GuestRole(storage);
                    }
                    currentRole = new AuthorizedUserRole(storage);
                    break;
                case UserEntity.Roles.Admin:
                    if (((AbstractRole)subject).isOpen)
                    {
                        currentRole.Exit();
                        curUser = user;
                    }
                    else
                    {
                        currentRole.Exit();
                        curUser = null;
                        currentRole = new GuestRole(storage);
                    }
                    currentRole = new AdminRole(storage);
                    break;
            }


            currentRole.Attach(this);
        }


    }
}
