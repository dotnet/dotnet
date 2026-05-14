// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform;

/// <summary>
/// Custom Attribute to specify the exact types which should be loaded from assembly
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
internal sealed class TestExtensionTypesAttribute : Attribute
{
    public TestExtensionTypesAttribute(params Type[] types)
    {
        Types = types;
    }

    public Type[] Types { get; }
}
