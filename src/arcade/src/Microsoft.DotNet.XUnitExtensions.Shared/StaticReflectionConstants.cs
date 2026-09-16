// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Xunit;

internal static class StaticReflectionConstants
{
    // ConditionalTestDiscoverer looks at all fields/methods/properties, recursively.
    internal const DynamicallyAccessedMemberTypes ConditionalMemberKinds =
        DynamicallyAccessedMemberTypes.AllMethods | DynamicallyAccessedMemberTypes.AllFields | DynamicallyAccessedMemberTypes.AllProperties;
}
