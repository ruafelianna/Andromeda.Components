using ReactiveUI;
using System.Reactive;

namespace Andromeda.Components.Forms.Abstractions
{
    public interface IResettableForm : IReactiveObject
    {
        IReactiveCommand Reset { get; }
    }

    public interface IResettableForm<TReset> : IResettableForm
    {
        new IReactiveCommand<TReset, Unit> Reset { get; }
    }
}
