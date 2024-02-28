// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.CodeAnalysis.Razor;

internal class DefaultTypeNameFeature : TypeNameFeature
{
    public override IReadOnlyList<string> ParseTypeParameters(string typeName)
    {
        if (typeName == null)
        {
            throw new ArgumentNullException(nameof(typeName));
        }

        var parsed = SyntaxFactory.ParseTypeName(typeName);

        if (parsed is IdentifierNameSyntax identifier)
        {
            return Array.Empty<string>();
        }

        if (TryParseCore(parsed) is { IsDefault: false } list)
        {
            return list;
        }

        return parsed.DescendantNodesAndSelf()
            .OfType<TypeArgumentListSyntax>()
            .SelectMany(arg => arg.Arguments)
            .SelectMany(t => ParseCore(t)).ToArray();

        static ImmutableArray<string> TryParseCore(TypeSyntax parsed)
        {
            if (parsed is ArrayTypeSyntax array)
            {
                return ParseCore(array.ElementType);
            }

            if (parsed is TupleTypeSyntax tuple)
            {
                return tuple.Elements.SelectManyAsArray(a => ParseCore(a.Type));
            }

            return default;
        }

        static ImmutableArray<string> ParseCore(TypeSyntax parsed)
        {
            // Recursively drill into arrays `T[]` and tuples `(T, T)`.
            if (TryParseCore(parsed) is { IsDefault: false } list)
            {
                return list;
            }

            // When we encounter an identifier, we assume it's a type parameter.
            if (parsed is IdentifierNameSyntax identifier)
            {
                return ImmutableArray.Create(identifier.Identifier.Text);
            }

            // Generic names like `C<T>` are ignored here because we will visit their type argument list
            // via the `.DescendantNodesAndSelf().OfType<TypeArgumentListSyntax>()` call above.
            return ImmutableArray<string>.Empty;
        }
    }

    public override TypeNameRewriter CreateGenericTypeRewriter(Dictionary<string, string> bindings)
    {
        if (bindings == null)
        {
            throw new ArgumentNullException(nameof(bindings));
        }

        return new GenericTypeNameRewriter(bindings);
    }

    public override TypeNameRewriter CreateGlobalQualifiedTypeNameRewriter(ICollection<string> ignore)
    {
        if (ignore == null)
        {
            throw new ArgumentNullException(nameof(ignore));
        }

        return new GlobalQualifiedTypeNameRewriter(ignore);
    }

    public override bool IsLambda(string expression)
    {
        var parsed = SyntaxFactory.ParseExpression(expression);
        return parsed is LambdaExpressionSyntax;
    }
}
