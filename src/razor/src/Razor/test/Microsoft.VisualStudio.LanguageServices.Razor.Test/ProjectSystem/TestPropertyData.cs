// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class TestPropertyData
{
    public string Category { get; set; }

    public string PropertyName { get; set; }

    public object Value { get; set; }

    public List<object> SetValues { get; }
}
