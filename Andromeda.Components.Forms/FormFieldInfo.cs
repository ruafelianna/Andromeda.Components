using Andromeda.Components.Forms.Abstractions;
using System;

namespace Andromeda.Components.Forms
{
    public record FormFieldInfo(
        string PropertyName,
        Type PropertyType,
        int Order,
        string? Label,
        string? Hint,
        IObservable<bool> CanRead,
        IObservable<bool> CanWrite,
        IFormFieldDataType DataType
    ) : IFormFieldInfo;
}
