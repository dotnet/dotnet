// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.Utilities;

internal static class ThrowHelper
{
    /// <summary>
    ///  This is present to help the JIT inline methods that need to throw an <see cref="InvalidOperationException"/>.
    /// </summary>
    [DoesNotReturn]
    public static T ThrowInvalidOperation<T>(string message)
        => throw new InvalidOperationException(message);
}
