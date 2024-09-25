using Microsoft.CodeAnalysis;
using System;

namespace Andromeda.Components.Forms.SourceGenerators.Errors
{
    internal class GeneratorException : Exception
    {
        public GeneratorException(
            DiagnosticDescriptor desc,
            Location? location = null,
            params string?[]? args
        )
        {
            DiagnosticDescriptor = desc;
            Location = location;
            Args = args;
        }

        public DiagnosticDescriptor DiagnosticDescriptor { get; }

        public Location? Location { get; }

        public string?[]? Args { get; }
    }
}
