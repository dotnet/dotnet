// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Intent;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
public class ExcludeAttribute : Attribute
{
    public ExcludeAttribute(string? reason = null)
    {
        Reason = reason;
    }

    public string? Reason { get; }
}
