// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.Editor.Razor;

public class LegacyCoreContentType : IContentType
{
    public string TypeName => throw new NotImplementedException();

    public string DisplayName => throw new NotImplementedException();

    public IEnumerable<IContentType> BaseTypes => throw new NotImplementedException();

    public bool IsOfType(string type)
    {
        return type == RazorConstants.LegacyCoreContentType;
    }
}
