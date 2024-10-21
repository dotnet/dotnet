// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Razor.Language.Intermediate;

public sealed class BaseTypeWithModel 
{
    const string ModelGenericParameter = "<TModel>";

    public BaseTypeWithModel(string baseType, SourceSpan? location = null)
    {
        if (baseType.EndsWith(ModelGenericParameter, System.StringComparison.Ordinal))
        {
            BaseType = IntermediateToken.CreateCSharpToken(baseType[0..^ModelGenericParameter.Length]);
            GreaterThan = IntermediateToken.CreateCSharpToken("<");
            ModelType = IntermediateToken.CreateCSharpToken("TModel");
            LessThan = IntermediateToken.CreateCSharpToken(">");

            if (location.HasValue)
            {
                var openBracketPosition = baseType.Length - ModelGenericParameter.Length;
                BaseType.Source = location.Value[..openBracketPosition];
                GreaterThan.Source = location.Value[openBracketPosition..(openBracketPosition + 1)];
                ModelType.Source = location.Value[(openBracketPosition + 1)..^1];
                LessThan.Source = location.Value[^1..];
            }
        }
        else
        {
            BaseType = IntermediateToken.CreateCSharpToken(baseType, location);  
        }
    }

    public IntermediateToken BaseType { get; set; }

    public IntermediateToken? GreaterThan { get; set; }

    public IntermediateToken? ModelType { get; set; }

    public IntermediateToken? LessThan { get; set; }
}
