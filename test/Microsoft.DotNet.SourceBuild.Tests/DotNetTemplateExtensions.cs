// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.DotNet.SourceBuild.Tests;

public static class DotNetTemplateExtensions
{
    public static string GetName(this DotNetTemplate template) => Enum.GetName(template)?.ToLowerInvariant() ?? throw new NotSupportedException();
}
