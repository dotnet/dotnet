// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;

namespace BenchmarkDotNet.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly)]
internal class ParameterizedJobConfigAttribute: AspNetCoreBenchmarkAttribute
{
    public ParameterizedJobConfigAttribute(Type configType) : base(configType)
    {
    }
}
