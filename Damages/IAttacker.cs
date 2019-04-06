using System;

namespace WCE.Damages
{
    public interface IAttacker
    {
        IObservable <Damage> StatusObserver { get; }
    }
}