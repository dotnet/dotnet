// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.RazorExtension.Options;
internal class LocDescriptionAttribute : DescriptionAttribute
{
    private readonly string _resourceKey;

    public LocDescriptionAttribute(string resourceKey) : base(resourceKey)
    {
        _resourceKey = resourceKey;
    }

    public override string Description => VSPackage.GetResourceString(_resourceKey);
}
