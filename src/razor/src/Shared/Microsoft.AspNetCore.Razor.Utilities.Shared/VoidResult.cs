// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor;

/// <summary>
/// Explicitly indicates result is void
/// </summary>
internal readonly struct VoidResult : IEquatable<VoidResult>
{
    public override bool Equals(object? obj)
        => obj is VoidResult;

    public override int GetHashCode()
        => 0;

    public bool Equals(VoidResult other)
        => true;
}
