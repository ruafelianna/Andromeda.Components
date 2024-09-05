using Andromeda.Components.Forms.Abstractions;

namespace Andromeda.Components.Forms.DataTypes
{
    public record ChoiceDataType(
        string OptionsPropertyName,
        string UpdateCommandName
    ) : IFormFieldDataType;
}
