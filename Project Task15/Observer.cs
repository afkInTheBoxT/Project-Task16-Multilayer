using Project_Task15.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Project_Task15.Observer
{
    public interface IObserver
    {
        void Update(ISubject subject, UserEntity user);
    }

    public interface ISubject
    {
        void Attach(IObserver observer);

        void Detach(IObserver observer);

        void Notify(UserEntity user);
    }
}