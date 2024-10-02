// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable enable

using System;
using Microsoft.SymbolStore;

namespace Microsoft.DotNet.Internal.SymbolHelper;

internal static class SymbolRequestHelpers
{
    internal static void ValidateRequestName(string? name, ITracer logger)
    {
        if (name is null or "")
        {
            logger.Error("Can't create a request with an empty name.");
            throw new ArgumentException("Name must be specified", nameof(name));
        }

        if (name.Contains('+'))
        {
            // This is a restriction of the symbol request pipeline and not of symbol.exe
            // we share this between upload and promotion to prevent downstream issues.
            logger.Error("Requests can't contain '+' in their name");
            throw new ArgumentException("Request can't contain a '+'", nameof(name));
        }
    }
}
