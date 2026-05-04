// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Collections.Generic;

namespace Microsoft.VisualStudio.Razor.ProjectSystem;

public class TestPropertyData
{
    public string Category { get; set; }

    public string PropertyName { get; set; }

    public object Value { get; set; }

    public List<object> SetValues { get; }
}
