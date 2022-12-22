// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Basic metadata for ITestPlaform.
/// </summary>
/// <remarks>
/// This interface is only public due to limitations in MEF which require metadata interfaces
/// to be public.  This interface is not intended for external consumption.
/// </remarks>
public interface ITestPlatformCapabilities
{
    /// <summary>
    /// Type of testPlatform
    /// </summary>
    TestPlatformType TestPlatformType { get; }
}

public enum TestPlatformType
{
    InProc,
    OutOfProc
}
