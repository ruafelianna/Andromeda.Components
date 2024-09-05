using System;

namespace Andromeda.Components.Forms.Abstractions
{
    public interface IFormFieldDataType
    {
        RuntimeTypeHandle Handle => GetType().TypeHandle;
    }
}
