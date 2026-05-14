// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK
using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

/// <summary>
/// Suspends the instrumentation (for code coverage) of the modules which are loaded
/// during this object is created and disposed
/// exceeded.
/// </summary>
public class SuspendCodeCoverage : IDisposable
{
    private const string SuspendCodeCoverageEnvVarName = "__VANGUARD_SUSPEND_INSTRUMENT__";
    private const string SuspendCodeCoverageEnvVarTrueValue = "TRUE";

    private readonly string? _prevEnvValue;

    /// <summary>
    /// Whether the object is disposed or not.
    /// </summary>
    private bool _isDisposed;

    /// <summary>
    /// Constructor. Code Coverage instrumentation of the modules, which are loaded
    /// during this object is created and disposed, is disabled.
    /// </summary>
    public SuspendCodeCoverage()
    {
        _prevEnvValue = Environment.GetEnvironmentVariable(SuspendCodeCoverageEnvVarName, EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable(SuspendCodeCoverageEnvVarName, SuspendCodeCoverageEnvVarTrueValue, EnvironmentVariableTarget.Process);
    }

    /// <summary>
    /// Disposes this instance
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes instance.
    /// </summary>
    /// <param name="disposing"> Should dispose. </param>
    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            Environment.SetEnvironmentVariable(SuspendCodeCoverageEnvVarName, _prevEnvValue, EnvironmentVariableTarget.Process);
        }

        _isDisposed = true;
    }
}

#endif
