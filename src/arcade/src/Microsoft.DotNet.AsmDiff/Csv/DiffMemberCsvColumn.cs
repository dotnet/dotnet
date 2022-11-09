// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci;
using Microsoft.Cci.Mappings;

namespace Microsoft.DotNet.AsmDiff.CSV
{
    public sealed class DiffMemberCsvColumn : DiffCsvColumn
    {
        public DiffMemberCsvColumn(DiffConfiguration diffConfiguration)
            : base(diffConfiguration)
        {
        }

        public override string Name
        {
            get { return "Member"; }
        }

        public override string GetValue(MemberMapping mapping)
        {
            var typeDefinitionMember = mapping.Representative;
            const NameFormattingOptions options = NameFormattingOptions.OmitContainingNamespace |
                                                  NameFormattingOptions.OmitContainingType |
                                                  NameFormattingOptions.TypeParameters |
                                                  NameFormattingOptions.Signature;
            return MemberHelper.GetMemberSignature(typeDefinitionMember, options);
        }
    }
}
