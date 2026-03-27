// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests;

internal class ResetableEnvironment
{
    /// <summary>
    /// Sets environment variable and resets it when returned object is disposed. Use with `using`.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DisposableVariable SetEnvironmentVariable(string name, string? value)
    {
        string? originalValue = Environment.GetEnvironmentVariable(name);

        Environment.SetEnvironmentVariable(name, value);

        return new DisposableVariable(name, originalValue);
    }
}

internal class DisposableVariable : IDisposable
{
    private readonly string _name;
    private readonly string? _originalValue;

    public DisposableVariable(string name, string? originalValue)
    {
        _name = name;
        _originalValue = originalValue;
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable(_name, _originalValue);
    }
}
