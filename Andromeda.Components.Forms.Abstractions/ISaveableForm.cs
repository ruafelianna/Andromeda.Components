using ReactiveUI;
using System;

namespace Andromeda.Components.Forms.Abstractions
{
    public interface ISaveableForm : IValidatableForm
    {
        IObservable<bool> CanSave { get; }

        IObservable<bool> IsChanged { get; }

        IReactiveCommand Save { get; }
    }
}
