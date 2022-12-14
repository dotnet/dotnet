// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Used to fetch <see cref="IChangeToken"/> used for tracking options changes.
    /// </summary>
    /// <typeparam name="TOptions">The options type being changed.</typeparam>
    public interface IOptionsChangeTokenSource<out TOptions>
    {
        /// <summary>
        /// Returns a <see cref="IChangeToken"/> which can be used to register a change notification callback.
        /// </summary>
        /// <returns>Change token.</returns>
        IChangeToken GetChangeToken();

        /// <summary>
        /// The name of the option instance being changed.
        /// </summary>
        string? Name { get; }
    }
}
