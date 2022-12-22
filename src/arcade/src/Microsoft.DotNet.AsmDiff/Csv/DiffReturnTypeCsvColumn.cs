// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Mappings;

namespace Microsoft.DotNet.AsmDiff.CSV
{
    public sealed class DiffReturnTypeCsvColumn : DiffCsvColumn
    {
        public DiffReturnTypeCsvColumn(DiffConfiguration diffConfiguration)
            : base(diffConfiguration)
        {
        }

        public override string Name
        {
            get { return "ReturnType"; }
        }

        public override string GetValue(MemberMapping mapping)
        {
            var signature = mapping.Representative as ISignature;
            return signature == null
                       ? null
                       : signature.Type.GetTypeName(NameFormattingOptions.DocumentationId);
        }
    }
}
