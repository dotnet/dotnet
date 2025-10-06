// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class RegistrationExtensionResult
{
    public RegistrationExtensionResult(string serverCapability, object options)
    {
        if (serverCapability is null)
        {
            throw new ArgumentNullException(nameof(serverCapability));
        }

        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        ServerCapability = serverCapability;
        Options = options;
    }

    public string ServerCapability { get; }

    public object Options { get; }
}
