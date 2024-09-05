using ReactiveUI;
using ReactiveUI.Validation.Abstractions;

namespace Andromeda.Components.Forms.Abstractions
{
    public interface IValidatableForm :
        IReactiveObject,
        IValidatableViewModel
    {
    }
}
