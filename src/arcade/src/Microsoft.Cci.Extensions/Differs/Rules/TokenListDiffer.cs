// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using Microsoft.Cci.Writers;
using Microsoft.Cci.Writers.Syntax;
using Microsoft.Cci.Writers.CSharp;
using Microsoft.Cci.Filters;

namespace Microsoft.Cci.Differs.Rules
{
    [ExportDifferenceRule]
    public class TokenListDiffer : DifferenceRule
    {
        [Import(AllowDefault = true)]
        public IDiffingService DiffingService { get; set; }

        private CSDeclarationHelper _declHelper = null;

        public override DifferenceType Diff(IDifferences differences, ITypeDefinitionMember item1, ITypeDefinitionMember item2)
        {
            return Diff(differences, item1, item2);
        }

        public override DifferenceType Diff(IDifferences differences, ITypeDefinition item1, ITypeDefinition item2)
        {
            return Diff(differences, item1, item2);
        }

        public override DifferenceType Diff(IDifferences differences, INamespaceDefinition item1, INamespaceDefinition item2)
        {
            return Diff(differences, item1, item2);
        }

        private DifferenceType Diff(IDifferences differences, IDefinition item1, IDefinition item2)
        {
            if (item1 == null || item2 == null)
                return DifferenceType.Unknown;

            var tokens1 = GetTokenList(item1);
            var tokens2 = GetTokenList(item2);

            // TODO: Add a difference to differences
            if (!TokensAreEqual(tokens1, tokens2))
                return DifferenceType.Changed;

            return DifferenceType.Unchanged;
        }

        private IEnumerable<SyntaxToken> GetTokenList(IDefinition item)
        {
            // If we have a contextual based service use it otherwise fall back to the simple one
            if (DiffingService != null)
                return DiffingService.GetTokenList(item);

            if (_declHelper == null)
                _declHelper = new CSDeclarationHelper(new PublicOnlyCciFilter());

            return _declHelper.GetTokenList(item);
        }

        private bool TokensAreEqual<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            return list1.SequenceEqual(list2);
        }
    }
}
