using System;
using System.Collections.Generic;

namespace model
{
    public interface IObserver
    {
        void Notify(); 
    }

    public interface IObservable
    {
        void NotifayObservers();
    }
}