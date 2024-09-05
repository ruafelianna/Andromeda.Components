using ReactiveUI;
using System.Collections.Generic;

namespace Andromeda.Components.Forms.Abstractions
{
    public interface IForm : IReactiveObject
    {
        string? Header { get; }

        int Order { get; }

        FormState State { get; }

        IEnumerable<IFormFieldInfo> FormFields { get; }
    }
}
