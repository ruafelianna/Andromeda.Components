using Microsoft.CodeAnalysis;

namespace Andromeda.Components.Forms.SourceGenerators.Errors
{
    internal static class GeneratorErrors
    {
        private const string _GetSetError_Name = "ANDR0001";

        private const string _IncorrectModifierError_Name = "ANDR0002";

        private const string _Category_AttributeError = "AttributeError";

        public static readonly DiagnosticDescriptor GetSetError
            = new(
                _GetSetError_Name,
                "Getter or setter wrong access modifier",
                "[{0}] The set access modifier cannot be less restrict than the get one",
                _Category_AttributeError,
                DiagnosticSeverity.Error,
                true
            );

        public static readonly DiagnosticDescriptor IncorrectModifierError
            = new(
                _IncorrectModifierError_Name,
                "Incorrect access modifier",
                "[{0}] Such access modifier is not supported in the given context",
                _Category_AttributeError,
                DiagnosticSeverity.Error,
                true
            );
    }
}
