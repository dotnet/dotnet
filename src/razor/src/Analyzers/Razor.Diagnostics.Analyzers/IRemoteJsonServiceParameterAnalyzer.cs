// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using static Razor.Diagnostics.Analyzers.Resources;

namespace Razor.Diagnostics.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class IRemoteJsonServiceParameterAnalyzer : DiagnosticAnalyzer
{
    internal static readonly DiagnosticDescriptor Rule = new(
        DiagnosticIds.IRemoteJsonServiceParameter,
        CreateLocalizableResourceString(nameof(IRemoteJsonServiceParameterTitle)),
        CreateLocalizableResourceString(nameof(IRemoteJsonServiceParameterMessage)),
        DiagnosticCategory.Reliability,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: CreateLocalizableResourceString(nameof(IRemoteJsonServiceParameterDescription)));

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
    }

    private static void AnalyzeSymbol(SymbolAnalysisContext context)
    {
        var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

        if (namedTypeSymbol.TypeKind == TypeKind.Interface &&
            namedTypeSymbol.AllInterfaces.Any(i => i.ToDisplayString() == WellKnownTypeNames.IRemoteJsonService))
        {
            foreach (var method in namedTypeSymbol.GetMembers().OfType<IMethodSymbol>())
            {
                foreach (var parameter in method.Parameters)
                {
                    if (parameter.Type.ToDisplayString() is
                        WellKnownTypeNames.RazorPinnedSolutionInfoWrapper or
                        WellKnownTypeNames.DocumentId)
                    {
                        var diagnostic = Diagnostic.Create(
                            Rule,
                            parameter.Locations.FirstOrDefault(),
                            parameter.Name,
                            namedTypeSymbol.Name,
                            method.Name,
                            parameter.Type.Name);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}
