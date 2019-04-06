using UniRx;
using System;

namespace WCE.Players
{
    public interface IInputProvider
    {
        IObservable<Unit> OnDischarge { get; }
        IObservable<string> OnAttributeChange { get; }
        IReadOnlyReactiveProperty<bool> OnPause { get; }
    }
}