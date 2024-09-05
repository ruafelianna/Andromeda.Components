using System;

namespace Andromeda.Components.Forms.Abstractions
{
    public interface IFormFieldInfo
    {
        string PropertyName { get; }

        Type PropertyType { get; }

        int Order { get; }

        string? Label { get; }

        string? Hint { get; }

        IObservable<bool> CanRead { get; }

        IObservable<bool> CanWrite { get; }

        IFormFieldDataType DataType { get; }
    }
}
