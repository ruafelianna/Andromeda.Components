using System;

namespace Andromeda.Components.Forms.Abstractions
{
    [Flags]
    public enum FormState : byte
    {
        Create = 0b0000_0001,
        Edit   = 0b0000_0010,
        View   = 0b0000_0100,
    }
}
