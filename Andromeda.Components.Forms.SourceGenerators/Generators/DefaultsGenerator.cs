using Andromeda.Collections.Extensions;
using Andromeda.Components.Forms.SourceGenerators.Errors;
using Andromeda.CSharp.Enums;
using Andromeda.CSharp.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using static Andromeda.Components.Forms.SourceGenerators.InternalConsts;
using static Andromeda.CSharp.Consts.Comments;
using static Andromeda.CSharp.Consts.Directives;
using static Andromeda.CSharp.Consts.Extensions;
using static Andromeda.CSharp.Consts.Keywords;
using static Andromeda.CSharp.Consts.Prefixes;

namespace Andromeda.Components.Forms.SourceGenerators.Generators
{
    [Generator(LanguageNames.CSharp)]
    public partial class DefaultsGenerator : IIncrementalGenerator
    {
        public void Initialize(
            IncrementalGeneratorInitializationContext context
        )
        {
            var propertiesWithDefault = context.SyntaxProvider
                .ForAttributeWithMetadataName(
                    $"{NS_Forms_FormFields}.{A_HasDefaultFull}",
                    IsProperty,
                    TransformProperty
                )
                .Where(record => record is not null)
                .Collect();

            context.RegisterSourceOutput(
                propertiesWithDefault,
                GenerateSourceCode
            );
        }

        private const string _propPrefix = "Default_";

        private record GeneratedPropertyInfo(
            ITypeSymbol Class,
            IPropertySymbol Property,
            INamedTypeSymbol Attribute,
            AccessModifier GetModifier,
            AccessModifier? SetModifier,
            bool IsInit,
            bool IsReactive
        );

        private static bool IsProperty(
            SyntaxNode node,
            CancellationToken token
        ) => node is PropertyDeclarationSyntax;

        private static GeneratedPropertyInfo? TransformProperty(
            GeneratorAttributeSyntaxContext ctx,
            CancellationToken token
        )
        {
            if (ctx.TargetSymbol is not IPropertySymbol propType)
            {
                return null;
            }

            // Checking that the attribute has correct
            // name and namespace

            var attr = ctx.Attributes
                .SingleOrDefault(
                    attrData => attrData.AttributeClass?.ToDisplayString(
                        SymbolDisplayFormat.FullyQualifiedFormat
                    ) == $"{PRE_Global}{NS_Forms_FormFields}.{A_HasDefaultFull}"
                );

            if (attr is null)
            {
                return null;
            }

            // Getting property's parent class

            var classParent = ctx.TargetNode.Parent;

            if (classParent is not ClassDeclarationSyntax classSyntax)
            {
                return null;
            }

            var classSymbol = ctx.SemanticModel
                .GetDeclaredSymbol(classSyntax, token);

            if (classSymbol is not ITypeSymbol classType)
            {
                return null;
            }

            // Checking that the parent class implements the required
            // interface

            var hasInterface = classType.AllInterfaces
                .Any(symbol => symbol.ToDisplayString(
                    SymbolDisplayFormat.FullyQualifiedFormat
                ) == $"{PRE_Global}{NS_Forms_Abstractions}.{I_IForm}");

            if (!hasInterface)
            {
                return null;
            }

            // Getting attribute's properties

            IDictionary<string, object?> namedArgs = attr
                .NamedArguments
                .Select(pair => new KeyValuePair<string, object?>(
                    pair.Key,
                    pair.Value.Value
                 ))
                .ToImmutableDictionary();

            return new(
                classType,
                propType,
                attr.AttributeClass!,
                (AccessModifier)namedArgs.GetValueOrDefault(
                    P_HasDefault_GetAccess,
                    (byte)AccessModifier.Public
                ),
                (AccessModifier?)namedArgs.GetValueOrDefault<string, byte>(
                    P_HasDefault_SetAccess,
                    null
                ),
                namedArgs.GetValueOrDefault(
                    P_HasDefault_IsInit,
                    false
                ),
                namedArgs.GetValueOrDefault(
                    P_HasDefault_IsReactive,
                    true
                )
            );
        }

        private static void GenerateSourceCode(
            SourceProductionContext ctx,
            ImmutableArray<GeneratedPropertyInfo?> source
        )
        {
            var groups = source
                .GroupBy<GeneratedPropertyInfo, ITypeSymbol>(
                    info => info!.Class,
                    SymbolEqualityComparer.Default
                );

            foreach (var group in groups)
            {
                var className = group.Key
                    .ToDisplayString(
                        SymbolDisplayFormat.FullyQualifiedFormat
                    )
                    .Substring(PRE_Global.Length);

                try
                {
                    ctx.AddSource(
                        $"{className}{EXT_GeneratedCSharp}",
                        GenerateClass(group)
                    );
                }
                catch (GeneratorException ex)
                {
                    ctx.ReportDiagnostic(
                        Diagnostic.Create(
                            ex.DiagnosticDescriptor,
                            ex.Location,
                            ex.Args
                        )
                    );
                }
            }
        }

        private static string GenerateClass(
            IGrouping<ISymbol, GeneratedPropertyInfo> group
        )
        {
            var items = new List<string>();
            var namespaces = new HashSet<string>();

            foreach (var pi in group)
            {
                var (prop, nspace) = GenerateProperty(pi);

                items.Add($"{PropTab}{prop}");
                namespaces.Add(nspace);
            }

            if (group.Any(x => x.IsReactive))
            {
                namespaces.Add($"{PRE_Global}{NS_Fody}");
            }

            var ns = namespaces
                .Select(x => $"{KW_Using} {x};")
                .OrderBy(x => x);

            return $@"{C_Autogenerated}

{DIR_NullableRestore}

{string.Join(NewLine, ns)}

{KW_Namespace} {group.Key.ContainingNamespace.ToDisplayString()}
{{
    {KW_Partial} {KW_Class} {group.Key.Name}
    {{
{string.Join(NewLine, items)}
    }}
}}
";
        }

        private static (string Prop, string Ns) GenerateProperty(
            GeneratedPropertyInfo pi
        )
        {
            var prop = new List<string>
            {
                pi.GetModifier.AsString(),
                pi.Property.Type.ToDisplayString(),
                $"{_propPrefix}{pi.Property.Name}",
                $"{{ {KW_Get};"
            };

            if (pi.SetModifier is not null)
            {
                var restriction = pi.GetModifier
                    .Restriction(pi.SetModifier.Value);

                var location = pi.Property.Locations.FirstOrDefault();
                var attrName = pi.Attribute?.Name ?? Unknown;

                switch (restriction)
                {
                    case AccessModifierRestriction.MoreRestrict:
                        throw new GeneratorException(
                            GeneratorErrors.GetSetError,
                            location,
                            [attrName]
                        );
                    case AccessModifierRestriction.Error:
                        throw new GeneratorException(
                            GeneratorErrors.IncorrectModifierError,
                            location,
                            [attrName]
                        );
                    case AccessModifierRestriction.LessRestrict:
                        prop.Add(pi.SetModifier.Value.AsString());
                        break;
                }

                if (pi.IsInit)
                {
                    prop.Add($"{KW_Init};");
                }
                else
                {
                    prop.Add($"{KW_Set};");
                }

                if (pi.IsReactive)
                {
                    prop.Insert(0, Reactive);
                }
            }

            prop.Add("}");

            return (
                string.Join(" ", prop),
                pi.Property.ContainingNamespace.ToDisplayString(
                    SymbolDisplayFormat.FullyQualifiedFormat
                )
            );
        }
    }
}
