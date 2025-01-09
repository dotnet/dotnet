// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence;

internal static class Argument
{
#if NETFRAMEWORK

    /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
    /// <param name="argument">The reference type argument to validate as non-null.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
    internal static void ThrowIfNull([NotNull] object? argument, string? paramName)
    {
        if (argument is null)
        {
            Throw(paramName);
        }
    }

    /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null or empty.</summary>
    /// <param name="argument">The reference type argument to validate as non-null or empty.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
    internal static void ThrowIfNullOrEmpty([NotNull] string? argument, string? paramName)
    {
        if (argument.IsNullOrEmpty())
        {
            Throw(paramName);
        }
    }

    [DoesNotReturn]
    internal static void Throw(string? paramName) => throw new ArgumentNullException(paramName);

#else

    /// <inheritdoc cref="ArgumentNullException.ThrowIfNull(object?, string?)"/>
    internal static void ThrowIfNull([NotNull] object? argument, [System.Runtime.CompilerServices.CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        ArgumentNullException.ThrowIfNull(argument, paramName);
    }

    /// <inheritdoc cref="ArgumentException.ThrowIfNullOrEmpty(string?, string?)"/>
    internal static void ThrowIfNullOrEmpty([NotNull] string? argument, [System.Runtime.CompilerServices.CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(argument, paramName);
    }

#endif
}
