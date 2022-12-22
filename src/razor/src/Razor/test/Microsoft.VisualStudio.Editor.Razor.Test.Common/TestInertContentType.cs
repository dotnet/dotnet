// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.Test;

public class TestInertContentType : IContentType
{
    public static readonly IContentType Instance = new TestInertContentType();

    public string TypeName => "inert";

    public string DisplayName => TypeName;

    public IEnumerable<IContentType> BaseTypes => Enumerable.Empty<IContentType>();

    public bool IsOfType(string type) => string.Equals(type, TypeName, StringComparison.OrdinalIgnoreCase);
}
