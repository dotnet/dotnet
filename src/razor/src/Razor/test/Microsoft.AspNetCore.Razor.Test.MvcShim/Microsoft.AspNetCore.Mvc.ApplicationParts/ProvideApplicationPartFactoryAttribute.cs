// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;

namespace Microsoft.AspNetCore.Mvc.ApplicationParts;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
public sealed class ProvideApplicationPartFactoryAttribute : Attribute
{
    public ProvideApplicationPartFactoryAttribute(Type factoryType)
    {
    }

    public ProvideApplicationPartFactoryAttribute(string factoryTypeName)
    {
    }
}
