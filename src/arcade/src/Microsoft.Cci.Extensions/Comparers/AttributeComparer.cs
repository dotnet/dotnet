// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Filters;
using Microsoft.Cci.Writers.CSharp;

namespace Microsoft.Cci.Comparers
{
    public class AttributeComparer : StringKeyComparer<ICustomAttribute>
    {
        private readonly CSDeclarationHelper _helper;

        public AttributeComparer()
            : this(new IncludeAllFilter(), false)
        {
        }

        public AttributeComparer(ICciFilter filter, bool forCompilation)
        {
            _helper = new CSDeclarationHelper(filter, forCompilation);
        }

        public override string GetKey(ICustomAttribute c)
        {
            return _helper.GetString(c);
        }
    }
}
