// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.Utilities.Shared.Test.TestData;

public readonly record struct ValueHolder<T>(T Value)
{
    public static implicit operator ValueHolder<T>(T value)
        => new(value);
}
