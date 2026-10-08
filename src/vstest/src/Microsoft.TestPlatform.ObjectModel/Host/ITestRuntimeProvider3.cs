// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;

/// <summary>
/// Interface to define a TestRuntimeProvider that can report whether the test host it launches
/// performs the connection identity check that protects the runner/test host channel.
/// </summary>
public interface ITestRuntimeProvider3 : ITestRuntimeProvider2
{
    /// <summary>
    /// Gets a value indicating whether the test host launched by this provider performs the
    /// connection identity check, reading its secrets from the <c>VSTEST_CONNECTION_SECRETS</c>
    /// environment variable.
    /// </summary>
    /// <remarks>
    /// There is no unauthenticated mode, so a test host that reports <see langword="false"/> (or a
    /// provider that only implements <see cref="ITestRuntimeProvider"/> or
    /// <see cref="ITestRuntimeProvider2"/>) is refused before it is launched.
    /// The value is only meaningful after <see cref="ITestRuntimeProvider.GetTestHostProcessStartInfo"/>
    /// has been called for the current run, because that is when the test host (and therefore its
    /// version) is resolved.
    /// </remarks>
    bool SupportsConnectionSecrets { get; }

    /// <summary>
    /// Gets the product version of the resolved test host, used to tell the user which version was
    /// found when it is too old. Null when the version could not be determined.
    /// </summary>
    string? TestHostVersion { get; }

    /// <summary>
    /// Gets the full path of the resolved test host, so the user is told which file was inspected
    /// rather than being left to guess which of several test hosts was picked. Null when the path is
    /// not known.
    /// </summary>
    string? TestHostPath { get; }
}
