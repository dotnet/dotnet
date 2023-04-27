// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Composition;

namespace Microsoft.Cci.Differs
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportDifferenceRuleAttribute : ExportAttribute
    {
        public ExportDifferenceRuleAttribute()
            : base(typeof(IDifferenceRule))
        {
        }

        public bool NonAPIConformanceRule { get; set; }
        public bool MdilServicingRule { get; set; }
        public bool OptionalRule { get; set; }
    }
}
