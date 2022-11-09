// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Composition;
using Microsoft.Cci;
using Microsoft.Cci.Differs;
using Microsoft.Cci.Differs.Rules;

namespace Microsoft.DotNet.ApiCompat
{
    public class ExportCciSettings
    {
        public static IEqualityComparer<ITypeReference> StaticSettings { get; set; }
        public static IDifferenceOperands  StaticOperands { get; set; }
        public static IAttributeFilter StaticAttributeFilter { get; set; }

        public static IRuleSettings StaticRuleSettings { get; set; }

        public ExportCciSettings()
        {
            Settings = StaticSettings;
            Operands = StaticOperands;
            AttributeFilter = StaticAttributeFilter;
            RuleSettings = StaticRuleSettings;
        }

        [Export(typeof(IEqualityComparer<ITypeReference>))]
        public IEqualityComparer<ITypeReference> Settings { get; }

        [Export(typeof(IDifferenceOperands))]
        public IDifferenceOperands Operands { get; }

        [Export(typeof(IAttributeFilter))]
        public IAttributeFilter AttributeFilter { get; }

        [Export(typeof(IRuleSettings))]
        public IRuleSettings RuleSettings { get; }
    }
}
