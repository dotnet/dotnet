// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using Microsoft.Cci;
using Microsoft.Cci.Differs;
using Microsoft.Cci.Mappings;
using Microsoft.Cci.Writers;
using Microsoft.Cci.Writers.Syntax;

namespace Microsoft.DotNet.AsmDiff.CSV
{
    public sealed class DiffTokensCsvColumn : DiffCsvColumn
    {
        private readonly DiffCSharpWriter _diffWriter;

        public DiffTokensCsvColumn(DiffConfiguration diffConfiguration)
            : base(diffConfiguration)
        {
            var stringWriter = new StringWriter();
            var writer = new TextSyntaxWriter(stringWriter);
            var mappingSettings = DiffEngine.GetMappingSettings(diffConfiguration);
            _diffWriter = new DiffCSharpWriter(writer, mappingSettings);
        }

        public override string Name
        {
            get { return "Tokens"; }
        }

        private string GetTokenString<T>(ElementMapping<T> mapping)
            where T : class, IDefinition
        {
            return mapping.Difference == DifferenceType.Changed
                ? GetTokenDiff(mapping)
                : GetTokenStringForSingleItem(mapping);
        }

        private string GetTokenDiff<T>(ElementMapping<T> mapping)
            where T : class, IDefinition
        {
            var diff = _diffWriter.GetTokenDiff(mapping[0], mapping[1]);
            var tokenStrings = from t in diff
                               let prefix = GetDifferencePrefix(t.Item1)
                               let token = t.Item2.Token
                               let text = string.IsNullOrWhiteSpace(token)
                                              ? token
                                              : prefix + token
                               select text;
            return string.Concat(tokenStrings);
        }

        private string GetTokenStringForSingleItem<T>(ElementMapping<T> mapping)
            where T : class, IDefinition
        {
            var tokens = _diffWriter.GetTokenList(mapping.Representative);
            return string.Concat(tokens.Select(t => t.Token));
        }

        private static string GetDifferencePrefix(DifferenceType differenceType)
        {
            switch (differenceType)
            {
                case DifferenceType.Unknown:
                case DifferenceType.Unchanged:
                    return string.Empty;
                case DifferenceType.Added:
                    return "+";
                case DifferenceType.Removed:
                    return "-";
                // DifferenceType.Changed can never occur during a token diff.
                default:
                    throw new ArgumentOutOfRangeException("differenceType");
            }
        }

        public override string GetValue(NamespaceMapping mapping)
        {
            return GetTokenString(mapping);
        }

        public override string GetValue(TypeMapping mapping)
        {
            return GetTokenString(mapping);
        }

        public override string GetValue(MemberMapping mapping)
        {
            return GetTokenString(mapping);
        }
    }
}
