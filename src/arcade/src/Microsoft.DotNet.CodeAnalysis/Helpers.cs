// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis;

namespace Microsoft.DotNet.CodeAnalysis
{
    public static class Helpers
    {
        private static readonly SymbolDisplayFormat s_publicApiFormat =
                        new SymbolDisplayFormat(
                            globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.OmittedAsContaining,
                            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                            propertyStyle: SymbolDisplayPropertyStyle.ShowReadWriteDescriptor,
                            genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
                            memberOptions:
                                SymbolDisplayMemberOptions.IncludeParameters |
                                SymbolDisplayMemberOptions.IncludeContainingType |
                                SymbolDisplayMemberOptions.IncludeExplicitInterface |
                                SymbolDisplayMemberOptions.IncludeModifiers |
                                SymbolDisplayMemberOptions.IncludeType |
                                SymbolDisplayMemberOptions.IncludeAccessibility |
                                SymbolDisplayMemberOptions.IncludeConstantValue,
                            parameterOptions:
                                SymbolDisplayParameterOptions.IncludeExtensionThis |
                                SymbolDisplayParameterOptions.IncludeParamsRefOut |
                                SymbolDisplayParameterOptions.IncludeType |
                                SymbolDisplayParameterOptions.IncludeName |
                                SymbolDisplayParameterOptions.IncludeDefaultValue,
                            miscellaneousOptions:
                                SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

        internal static string GetMemberName(ISymbol symbol)
        {
            return symbol.ToDisplayString(s_publicApiFormat);
        }
    }
}
